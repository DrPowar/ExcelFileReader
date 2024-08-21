using Avalonia.Controls;
using Avalonia.Platform.Storage;
using DynamicData;
using DynamicData.Binding;
using DynamicData.Operators;
using ExcelFileReader.Constants;
using ExcelFileReader.DataTransfer;
using ExcelFileReader.Models;
using Prism.Commands;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace ExcelFileReader.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private const int PageSize = 25;
        private const int FirstPage = 1;
        private readonly ISubject<PageRequest> _pager;
        private readonly Client _client;
        private readonly TopLevel _topLevel;
        private readonly PeopleService _peopleService;
        private ObservableCollectionExtended<Person> _pagedPeople;
        private ObservableCollection<Person> _selectedPeople;
        private bool _canUploadFile = true;
        private bool _canSaveData;
        private string _uploadingStatus = UploadingStatusMessages.UploadingAllowed;
        private int _currentPage;
        private int _totalItems;
        private int _validItems;
        private int _inValidItems;
        private int _totalPages;
        private string _searchField;

        internal ObservableCollection<Gender> GenderOptions { get; } = new ObservableCollection<Gender> { Gender.Male, Gender.Female };

        internal int TotalItems
        {
            get => _totalItems;
            set => this.RaiseAndSetIfChanged(ref _totalItems, value);
        }

        internal string SearchField
        {
            get => _searchField;
            set => this.RaiseAndSetIfChanged(ref _searchField, value);
        }

        internal int ValidItems
        {
            get => _validItems;
            set => this.RaiseAndSetIfChanged(ref _validItems, value);
        }

        internal int InValidItems
        {
            get => _inValidItems;
            set => this.RaiseAndSetIfChanged(ref _inValidItems, value);
        }

        internal int CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        internal int TotalPages
        {
            get => _totalPages;
            set => this.RaiseAndSetIfChanged(ref _totalPages, value);
        }

        internal string UploadingStatus
        {
            get => _uploadingStatus;
            set => this.RaiseAndSetIfChanged(ref _uploadingStatus, value);
        }

        internal bool CanUploadFile
        {
            get => _canUploadFile;
            set => this.RaiseAndSetIfChanged(ref _canUploadFile, value);
        }

        internal bool CanSaveData
        {
            get => _canSaveData;
            set => this.RaiseAndSetIfChanged(ref _canSaveData, value);
        }

        internal ObservableCollection<Person> SelectedPeople
        {
            get => _selectedPeople;
            set => this.RaiseAndSetIfChanged(ref _selectedPeople, value);
        }

        internal ObservableCollectionExtended<Person> People
        {
            get => _pagedPeople;
            set => this.RaiseAndSetIfChanged(ref _pagedPeople, value);
        }

        internal DelegateCommand OpenFilePicker { get; init; }
        internal DelegateCommand FirstPageCommand { get; init; }
        internal DelegateCommand PreviousPageCommand { get; init; }
        internal DelegateCommand NextPageCommand { get; init; }
        internal DelegateCommand LastPageCommand { get; init; }
        internal DelegateCommand SaveDataCommand { get; init; }
        internal DelegateCommand<string> SearchDataCommand { get; init; }
        internal DelegateCommand GetAllDataFromDBCommand { get; init; } 

        internal MainWindowViewModel(TopLevel topLevel)
        {
            _selectedPeople = new ObservableCollection<Person>();
            _pager = new BehaviorSubject<PageRequest>(new PageRequest(FirstPage, PageSize));
            _pagedPeople = new ObservableCollectionExtended<Person>();
            _peopleService = new PeopleService();
            PeopleCacheInit(_peopleService);
            _client = new Client();

            FirstPageCommand = new DelegateCommand(MoveToFirstPage, CanMoveToFirstPage)
                .ObservesProperty(() => CurrentPage);

            PreviousPageCommand = new DelegateCommand(MoveToPreviousPage, CanMoveToPreviousPage)
                .ObservesProperty(() => CurrentPage);

            NextPageCommand = new DelegateCommand(MoveToNextPage, CanMoveToNextPage)
                .ObservesProperty(() => CurrentPage)
                .ObservesProperty(() => TotalPages);

            LastPageCommand = new DelegateCommand(MoveToLastPage, CanMoveToLastPage)
                .ObservesProperty(() => CurrentPage)
                .ObservesProperty(() => TotalPages);


            OpenFilePicker = new DelegateCommand(OpenFileButton_Click);
            SaveDataCommand = new DelegateCommand(SaveDataButton_Click);
            SearchDataCommand = new DelegateCommand<string>(SearchDataButton_Click);
            GetAllDataFromDBCommand = new DelegateCommand(GetAllDataFromDBButton_Click);
            _topLevel = topLevel;
        }

        internal async void GetAllDataFromDBButton_Click()
        {
            GetAllDataResponse getAllDataResponse = await _client.GetAllDataFromDB();
            if(getAllDataResponse.Result)
            {
                _peopleService.ClearPeople();
                _peopleService.LoadData(getAllDataResponse.People);

                UpdateItemsCountFields();

                CanSaveData = true;
                CanUploadFile = true;
                UploadingStatus = UploadingStatusMessages.UploadingAllowed;
            }
            else
            {
                UploadingStatus = UploadingStatusMessages.UploadingAllowed;
            }
        }

        internal void SearchDataButton_Click(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                _peopleService.RestorePeopleFromTemp();
            }
            else
            {
                _peopleService.SavePeopleToTemp();
                _peopleService.ClearPeople();
                _peopleService.LoadData(SearchData(_peopleService.GetTempPeople(), query));
            }
        }

        internal async void OpenFileButton_Click()
        {
            IReadOnlyCollection<IStorageFile> files = await GetFiles(_topLevel);
            if (files.Count == 0)
            {
                return;
            }

            CanUploadFile = false;
            UploadingStatus = UploadingStatusMessages.WaitingForServerResponse;

            FileUploadRequest filesRequests = await GetSingleFileContent(files.First());

            if (filesRequests != null)
            {
                ParsingResponse response = await _client.SendFile(filesRequests.FileContent, filesRequests.FileName);
                UploadingStatus = UploadingStatusMessages.WaitingForDataGridUpdating;
                if (response.IsValid)
                {
                    _peopleService.ClearPeople();
                    _peopleService.LoadData(response.People);

                    UpdateItemsCountFields();

                    CanSaveData = true;
                    CanUploadFile = true;
                    UploadingStatus = UploadingStatusMessages.UploadingAllowed;
                }
                else
                {
                    UploadingStatus = response.Message;
                }
            }
        }

        internal async void SaveDataButton_Click()
        {
            UploadingStatus = UploadingStatusMessages.WaitingForServerResponse;
            CanSaveData = false;
            CanUploadFile = false;
            if (_selectedPeople.Count > 0 && _selectedPeople.All(p => p.IsValid))
            {
                SavingDataResponse response = await _client.SaveDataOnServer(_selectedPeople.ToList());

                if (response.IsValid)
                {
                    UploadingStatus = UploadingStatusMessages.DataSaveSuccess;

                    _peopleService.UpdateData(_selectedPeople);

                    UpdateItemsCountFields();
                    CanSaveData = true;
                    CanUploadFile = true;
                }
                else
                {
                    UploadingStatus = response.Message;
                }
            }
            else
            {
                UploadingStatus = UploadingStatusMessages.SelectValidData;
            }
        }

        internal bool UpdatePerson(Person updatedPerson)
        {
            Person person = People.FirstOrDefault(p => p.Id == updatedPerson.Id);
            if (person != null)
            {
                person.UpdateIsValidProperty();
            }
            return person.IsValid;
        }

        internal void UpdateItemsCountFields()
        {
            List<Person> people = _peopleService.GetPeople();
            InValidItems = people.Where(p => !p.IsValid).Count();
            ValidItems = people.Where(p => p.IsValid).Count();
        }

        private void UpdatePagesFields()
        {
            TotalPages = (int)Math.Ceiling((double)TotalItems / PageSize);
            CurrentPage = Math.Min(CurrentPage, TotalPages);
        }

        private async Task<IReadOnlyCollection<IStorageFile>> GetFiles(TopLevel topLevel)
        {
            return await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open Text File",
                AllowMultiple = false,
            });
        }

        private IEnumerable<Person> SearchData(IEnumerable<Person> people, string query)
        {
            return people.Where(person =>
                person.GetType().GetProperties()
                .Any(prop => prop.GetValue(person)?.ToString().Contains(query) == true)
            ).ToList();
        }

        private void PeopleCacheInit(PeopleService peopleService)
        {
            peopleService.PeopleConnection()
                .Sort(SortExpressionComparer<Person>.Ascending(e => e.Number))
                .Page(_pager)
                .Do(change => PagingUpdate(change.Response))
                .Bind(_pagedPeople)
                .Subscribe();
        }

        private void PagingUpdate(IPageResponse response)
        {
            TotalItems = response.TotalSize;
            CurrentPage = response.Page;
            TotalPages = response.Pages;
        }

        private async Task<FileUploadRequest> GetSingleFileContent(IStorageFile file)
        {
            byte[] fileContent = Array.Empty<byte>();

            if (file != null)
            {
                await using Stream stream = await file.OpenReadAsync();
                using MemoryStream memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                fileContent = memoryStream.ToArray();
            }

            return new FileUploadRequest(file.Name, fileContent);
        }

        private async Task<List<FileUploadRequest>> GetMultipleFilesContent(IReadOnlyCollection<IStorageFile> files)
        {
            List<FileUploadRequest> filesContent = new List<FileUploadRequest>();
            foreach (IStorageFile file in files)
            {
                if (file != null)
                {
                    await using Stream stream = await file.OpenReadAsync();
                    using MemoryStream memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    FileUploadRequest fileContent = new FileUploadRequest(file.Name, memoryStream.ToArray());
                    filesContent.Add(fileContent);
                }
            }
            return filesContent;
        }

        private void MoveToPreviousPage() =>
            _pager.OnNext(new PageRequest(_currentPage - 1, PageSize));

        private bool CanMoveToPreviousPage() => CurrentPage > FirstPage;

        private void MoveToNextPage() =>
            _pager.OnNext(new PageRequest(_currentPage + 1, PageSize));

        private bool CanMoveToNextPage() => CurrentPage < TotalPages;

        private void MoveToFirstPage() =>
            _pager.OnNext(new PageRequest(FirstPage, PageSize));

        private bool CanMoveToFirstPage() => CurrentPage > FirstPage;

        private void MoveToLastPage() =>
            _pager.OnNext(new PageRequest(_totalPages, PageSize));

        private bool CanMoveToLastPage() => CurrentPage < TotalPages;
    }
}