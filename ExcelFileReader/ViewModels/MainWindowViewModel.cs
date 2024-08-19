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
        private const int PageSize = 100;
        private const int FirstPage = 1;
        private readonly ISubject<PageRequest> _pager;
        private readonly Client _client;
        private readonly Window _window;
        private readonly ISourceCache<Person, int> _personCache;
        private ObservableCollectionExtended<Person> _pagedPersons;
        private ObservableCollection<Person> _selecetdPersons;
        private bool _canUploadFile = true;
        private bool _canSaveData;
        private string _uploadingStatus = UploadingStatusMessages.UploadingAllowed;
        private int _currentPage;
        private int _totalItems;
        private int _totalPages;

        internal ObservableCollection<Gender> GenderOptions { get; } = new ObservableCollection<Gender> { Gender.Male, Gender.Female};

        internal int TotalItems
        {
            get => _totalItems;
            set => this.RaiseAndSetIfChanged(ref _totalItems, value);
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

        internal ObservableCollection<Person> SelectedPersons
        {
            get => _selecetdPersons;
            set => this.RaiseAndSetIfChanged(ref _selecetdPersons, value);
        }

        internal ObservableCollectionExtended<Person> Persons
        {
            get => _pagedPersons;
            set => this.RaiseAndSetIfChanged(ref _pagedPersons, value);
        }

        internal DelegateCommand OpenFilePicker { get; init; }
        internal DelegateCommand FirstPageCommand { get; init; }
        internal DelegateCommand PreviousPageCommand { get; init; }
        internal DelegateCommand NextPageCommand { get; init; }
        internal DelegateCommand LastPageCommand { get; init; }
        internal DelegateCommand SaveDataCommand { get; init; }

        internal MainWindowViewModel(Window window)
        {
            _selecetdPersons = new ObservableCollection<Person>();
            _pager = new BehaviorSubject<PageRequest>(new PageRequest(FirstPage, PageSize));
            _pagedPersons = new ObservableCollectionExtended<Person>();
            _personCache = PersonCacheInit(_pager, _pagedPersons);
            _client = new Client();

            OpenFilePicker = new DelegateCommand(OpenFileButton_Clicked);
            FirstPageCommand = new DelegateCommand(MoveToFirstPage);
            PreviousPageCommand = new DelegateCommand(MoveToPreviousPage);
            NextPageCommand = new DelegateCommand(MoveToNextPage);
            LastPageCommand = new DelegateCommand(MoveToLastPage);
            SaveDataCommand = new DelegateCommand(SaveDataButton_Click);
            _window = window;
        }

        internal async void OpenFileButton_Clicked()
        {
            TopLevel topLevel = TopLevel.GetTopLevel(_window);

            IReadOnlyCollection<IStorageFile> files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open Text File",
                AllowMultiple = false,
            });
            CanUploadFile = false;
            UploadingStatus = UploadingStatusMessages.WaitingForServerResponse;

            FileUploadRequest filesRequests = await GetSingleFileContent(files.First());

            if (filesRequests != null)
            {
                ParsingResponse response = await _client.SendFile(filesRequests.FileContent, filesRequests.FileName);
                UploadingStatus = UploadingStatusMessages.WaitingForDataGridUpdating;
                if (response.IsValid)
                {
                    _personCache.AddOrUpdate(response.Persons);

                    CanSaveData = true;
                    CanUploadFile = true;
                    UploadingStatus = UploadingStatusMessages.UploadingAllowed;
                }
                else
                {
                    //Show message on interface
                }
            }
        }

        internal async void SaveDataButton_Click()
        {
            UploadingStatus = UploadingStatusMessages.WaitingForServerResponse;
            if(_selecetdPersons.Count > 0 && _selecetdPersons.All(p => p.IsValid))
            {
                SavingDataResponse response = await _client.SaveDataOnServer(_selecetdPersons.ToList());

                if (response.IsValid)
                {
                    UploadingStatus = UploadingStatusMessages.DataSaveSuccess;
                    Persons.RemoveMany(_selecetdPersons);
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
            Person person = Persons.FirstOrDefault(p => p.Id == updatedPerson.Id);
            if (person != null)
            {
                person.UpdateIsValidProperty();
            }
            return person.IsValid;
        }

        private ISourceCache<Person, int> PersonCacheInit(ISubject<PageRequest> subject, ObservableCollectionExtended<Person> persons)
        {
            SourceCache<Person, int> cache = new SourceCache<Person, int>(person => (int)person.Number);

            cache.Connect()
                .Sort(SortExpressionComparer<Person>.Ascending(e => e.Number))
                .Page(subject)
                .Do(change => PagingUpdate(change.Response))
                .Bind(persons)
                .Subscribe();

            return cache;
        }

        private IObservable<IChangeSet<Person, int>> PersonsConnection() => _personCache.Connect();

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