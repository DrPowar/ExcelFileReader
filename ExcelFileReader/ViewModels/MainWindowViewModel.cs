using Avalonia.Controls;
using Avalonia.Platform.Storage;
using DynamicData;
using DynamicData.Binding;
using DynamicData.Operators;
using ExcelFileReader.Constants;
using ExcelFileReader.DataTransfer;
using ExcelFileReader.Models;
using ExcelFileReader.Services;
using ExcelFileReader.Views;
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
        private const int FirstPage = 1;
        private int _pageSize = 25;

        private readonly ISubject<PageRequest> _pager;
        private readonly Client _client;
        private readonly TopLevel _topLevel;


        private readonly CacheService<Log, Guid> _logsCache;
        private ObservableCollectionExtended<Log> _pagedLogs;

        private readonly CacheService<Person, int> _peopleCache;
        private ObservableCollectionExtended<Person> _pagedPeople;

        private ObservableCollection<object> _selectedRows;
        private List<Person> _updatedPeople = new();
        private List<Log> _logs = new();

        private bool _dataBaseDataActive;
        private bool _canModifyLogs;
        private bool _canUploadFile = true;
        private bool _canSaveData;
        private bool _canAddPerson;

        private int _currentPage;
        private int _totalItems;
        private int _validItems;
        private int _inValidItems;
        private int _totalPages;
        private int _fontSize = 16;
        private string _selectedItemType;

        private bool _logsDataGridVisible;
        private bool _peopleDataGridVisible = true;

        private string _searchField = string.Empty;
        private string _programStatus = ProgramStatusMessages.UploadingAllowed;

        internal ObservableCollection<Gender> GenderOptions { get; } = new ObservableCollection<Gender> { Gender.Male, Gender.Female };
        internal ObservableCollection<int> PageSizes { get; } = new ObservableCollection<int> { 5, 10, 25, 50, 100 };
        internal int TotalItems
        {
            get => _totalItems;
            set => this.RaiseAndSetIfChanged(ref _totalItems, value);
        }

        internal int PageSize
        {
            get => _pageSize;
            set
            {
                this.RaiseAndSetIfChanged(ref _pageSize, value);
                UpdatePagesFields();
                _pager.OnNext(new PageRequest(CurrentPage, PageSize));
            }
        }

        internal bool LogsDataGridActive
        {
            get => _logsDataGridVisible;
            set => this.RaiseAndSetIfChanged(ref _logsDataGridVisible, value);
        }

        internal bool PeopleDataGridActive
        {
            get => _peopleDataGridVisible;
            set => this.RaiseAndSetIfChanged(ref _peopleDataGridVisible, value);
        }

        internal string SelectedItemType
        {
            get => _selectedItemType;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedItemType, value);
                Console.Write(value);
            }
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

        internal string ProgramStatus
        {
            get => _programStatus;
            set => this.RaiseAndSetIfChanged(ref _programStatus, value);
        }

        internal bool CanUploadFile
        {
            get => _canUploadFile;
            set => this.RaiseAndSetIfChanged(ref _canUploadFile, value);
        }

        internal bool CanAddPerson
        {
            get => _canAddPerson;
            set => this.RaiseAndSetIfChanged(ref _canAddPerson, value);
        }

        internal bool CanSaveData
        {
            get => _canSaveData;
            set => this.RaiseAndSetIfChanged(ref _canSaveData, value);
        }

        internal int FontSize
        {
            get => _fontSize;
            set => this.RaiseAndSetIfChanged(ref _fontSize, value);
        }

        internal bool CanModifyPeople
        {
            get => _dataBaseDataActive;
            set => this.RaiseAndSetIfChanged(ref _dataBaseDataActive, value);
        }

        internal bool CanModifyLogs
        {
            get => _canModifyLogs;
            set => this.RaiseAndSetIfChanged(ref _canModifyLogs, value);
        }

        internal ObservableCollection<object> SelectedItems
        {
            get => _selectedRows;
            set => this.RaiseAndSetIfChanged(ref _selectedRows, value);
        }

        internal ObservableCollectionExtended<Person> People
        {
            get => _pagedPeople;
            set => this.RaiseAndSetIfChanged(ref _pagedPeople, value);
        }

        internal ObservableCollectionExtended<Log> Logs
        {
            get => _pagedLogs;
            set => this.RaiseAndSetIfChanged(ref _pagedLogs, value);
        }

        internal DelegateCommand OpenGetFilePicker { get; init; }
        internal DelegateCommand OpenSaveFilePicker { get; init; }
        internal DelegateCommand FirstPageCommand { get; init; }
        internal DelegateCommand PreviousPageCommand { get; init; }
        internal DelegateCommand NextPageCommand { get; init; }
        internal DelegateCommand LastPageCommand { get; init; }
        internal DelegateCommand SaveDataCommand { get; init; }
        internal DelegateCommand<string> SearchDataCommand { get; init; }
        internal DelegateCommand GetPeopleDataFromDBCommand { get; init; }
        internal DelegateCommand UpdateDataCommand { get; init; }
        internal DelegateCommand DeleteDataCommand { get; init; }
        internal DelegateCommand GetLogsDataFromDBCommand { get; init; }
        internal DelegateCommand RemoveLogCommand { get; init; }
        internal DelegateCommand<ZoomCommands?> ZoomCommand { get; init; }
        internal DelegateCommand FullScreenCommnad { get; init; }
        internal DelegateCommand MinimizeWindowCommand { get; init; }
        internal DelegateCommand CloseWindowCommand { get; init; }

        internal MainWindowViewModel(TopLevel topLevel)
        {
            _selectedRows = new ObservableCollection<object>();
            _pager = new BehaviorSubject<PageRequest>(new PageRequest(FirstPage, PageSize));
            _pagedPeople = new ObservableCollectionExtended<Person>();
            _pagedLogs = new ObservableCollectionExtended<Log>();
            _peopleCache = new CacheService<Person, int>(e => (int)e.Number);
            _logsCache = new CacheService<Log, Guid>(e => e.Id);
            LogCacheInit(_logsCache);
            PeopleCacheInit(_peopleCache);
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


            OpenGetFilePicker = new DelegateCommand(OpenFileButton_Click);
            OpenSaveFilePicker = new DelegateCommand(SaveFileButton_Click);

            SaveDataCommand = new DelegateCommand(SaveDataButton_Click);
            SearchDataCommand = new DelegateCommand<string>(SearchDataButton_Click);

            GetPeopleDataFromDBCommand = new DelegateCommand(GetPeopleDataFromDBButton_Click);
            UpdateDataCommand = new DelegateCommand(UpdateDataButton_Click);
            DeleteDataCommand = new DelegateCommand(DeleteDataButton_Click);

            GetLogsDataFromDBCommand = new DelegateCommand(GetLogsDataFromDBButton_Click);
            RemoveLogCommand = new DelegateCommand(RemoveLogButton_Click);

            ZoomCommand = new DelegateCommand<ZoomCommands?>(ZoomButton_Click);
            FullScreenCommnad = new DelegateCommand(FullScreenButton_Click);
            MinimizeWindowCommand = new DelegateCommand(MinimizeWindowButton_Click);
            CloseWindowCommand = new DelegateCommand(CloseWindowButton_Click);
            _topLevel = topLevel;
        }

        internal void CloseWindowButton_Click()
        {
            if (_topLevel is Window window)
            {
                window.Close();
            }
        }

        internal void MinimizeWindowButton_Click()
        {
            if (_topLevel is Window window)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        internal void FullScreenButton_Click()
        {
            if (_topLevel is Window window)
            {
                if (window.WindowState == WindowState.FullScreen)
                {
                    window.WindowState = WindowState.Normal;
                }
                else
                {
                    window.WindowState = WindowState.FullScreen;
                }
            }
        }

        internal void ZoomButton_Click(ZoomCommands? command)
        {
            const int minFontSize = 8;
            const int maxFontSize = 52;

            if (command == ZoomCommands.ZoomIn && FontSize < maxFontSize)
            {
                FontSize++;
            }
            else if (command == ZoomCommands.ZoomOut && FontSize > minFontSize)
            {
                FontSize--;
            }

        }

        internal void AddNewPerson(Person person)
        {
            var people = _peopleCache.GetData();
            uint number = people.Max(p => p.Number);
            person.Number = number + 1;
            List<Person> list = new List<Person>() { person};
            _peopleCache.LoadData(list);
        }

        internal async void GetPeopleDataFromDBButton_Click()
        {
            CanSaveData = false;
            CanAddPerson = true;
            CanUploadFile = false;
            CanModifyLogs = false;
            GetPeopleDataResponse getAllDataResponse = await _client.GetPeople();

            if (getAllDataResponse.Result)
            {
                _logsCache.FullClear();
                CanModifyPeople = true;
                _peopleCache.FullClear();
                _peopleCache.LoadData(getAllDataResponse.People);

                UpdateItemsCountFields();

                CanUploadFile = true;
                LogsDataGridActive = false;
                PeopleDataGridActive = true;
                CanSaveData = true;
                ProgramStatus = ProgramStatusMessages.UploadingAllowed;
            }
            else
            {
                CanUploadFile = true;
                ProgramStatus = getAllDataResponse.Message;
            }
        }

        internal async void GetLogsDataFromDBButton_Click()
        {
            CanSaveData = false;
            CanAddPerson = false;
            CanUploadFile = false;
            CanModifyPeople = false;
            LogsDataGridActive = true;
            PeopleDataGridActive = false;

            GetLogsDataResponse getAllDataResponse = await _client.GetLogs();
            if (getAllDataResponse.Result)
            {
                _peopleCache.FullClear();
                CanModifyLogs = true;
                _logsCache.ClearData();
                _logsCache.LoadData(getAllDataResponse.Logs);

                UpdateItemsCountFields();

                CanUploadFile = true;
                CanSaveData = true;
                ProgramStatus = ProgramStatusMessages.UploadingAllowed;
            }
            else
            {
                CanUploadFile = true;
                ProgramStatus = getAllDataResponse.Message;
            }
        }

        internal async void RemoveLogButton_Click()
        {
            ProgramStatus = ProgramStatusMessages.WaitingForServerResponse;
            CanUploadFile = false;
            CanModifyLogs = false;

            SavingDataResponse response = await _client.DeleteLogsOnServer(_selectedRows.Cast<Log>().ToList());

            if (response.IsValid)
            {
                ProgramStatus = ProgramStatusMessages.DataDeleteSuccess;

                _logsCache.RemoveData(_selectedRows.Cast<Log>());

                UpdateItemsCountFields();
                CanModifyLogs = true;
                CanUploadFile = true;
            }
            else
            {
                ProgramStatus = response.Message;
            }
        }

        internal void ItemTypeCombobox_SelectionChanged(string query)
        {
            const string valid = "valid";
            const string inValid = "invalid";
            const string total = "total";

            _peopleCache.RestoreDataFromTemp();

            if (query != total)
            {
                _peopleCache.SaveDataToTemp();

                _peopleCache.ClearData();

                if (query == valid)
                {
                    _peopleCache.LoadData(_peopleCache.GetTempData().Where(p => p.IsValid));
                }
                else if (query == inValid)
                {
                    _peopleCache.LoadData(_peopleCache.GetTempData().Where(p => !p.IsValid));
                }
            }
        }


        internal void SearchDataButton_Click(string query)
        {
            if (LogsDataGridActive)
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    _logsCache.RestoreDataFromTemp();
                }
                else
                {
                    _logsCache.SaveDataToTemp();
                    _logsCache.ClearData();
                    _logsCache.LoadData(SearchData(_logsCache.GetTempData(), query));
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    _peopleCache.RestoreDataFromTemp();
                }
                else
                {
                    _peopleCache.SaveDataToTemp();
                    _peopleCache.ClearData();
                    _peopleCache.LoadData(SearchData(_peopleCache.GetTempData(), query));
                }
            }
        }

        internal async void SaveFileButton_Click()
        {
            ProgramStatus = ProgramStatusMessages.WaitingForServerResponse;
            CanSaveData = false;
            CanUploadFile = false;

            if (_peopleCache.GetData().Count > 0)
            {
                ParseDataToExcleFileResponse response = await _client.ParsePeopleToExcleFile(_peopleCache.GetData());
                await SaveData(response);
            }
            else if(_logsCache.GetData().Count > 0)
            {
                ParseDataToExcleFileResponse response = await _client.ParseLogsToExcelFile(_logsCache.GetData());
                await SaveData(response);
            }
            else
            {
                ProgramStatus = ProgramStatusMessages.NoDataToSave;
            }
            CanUploadFile = true;
            CanSaveData = true;
        }

        private async Task SaveData(ParseDataToExcleFileResponse response)
        {
            if (response.Result)
            {
                ProgramStatus = ProgramStatusMessages.DataParseToExcelSuccess;

                SaveFile(_topLevel, response.FileContent);

                UpdateItemsCountFields();
                CanSaveData = true;
                CanUploadFile = true;
            }
            else
            {
                ProgramStatus = response.Message;
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
            ProgramStatus = ProgramStatusMessages.WaitingForServerResponse;

            FileUploadRequest filesRequests = await GetSingleFileContent(files.First());

            if (filesRequests != null)
            {
                ParsingResponse response = await _client.SendFile(filesRequests.FileContent, filesRequests.FileName);
                ProgramStatus = ProgramStatusMessages.WaitingForDataGridUpdating;

                if (response.IsValid)
                {
                    _logsCache.FullClear();
                    CanModifyPeople = false;
                    _peopleCache.FullClear();
                    _peopleCache.LoadData(response.People);

                    UpdateItemsCountFields();

                    CanSaveData = true;
                    CanUploadFile = true;
                    LogsDataGridActive = false;
                    PeopleDataGridActive = true;
                    CanAddPerson = true;
                    ProgramStatus = ProgramStatusMessages.UploadingAllowed;
                }
                else
                {
                    ProgramStatus = response.Message;
                }
            }
            CanUploadFile = true;
        }

        internal async void SaveDataButton_Click()
        {
            ProgramStatus = ProgramStatusMessages.WaitingForServerResponse;
            CanSaveData = false;
            CanUploadFile = false;

            if (_selectedRows.Count > 0 && _selectedRows.All(p => (p as Person)?.IsValid == true))
            {
                SavingDataResponse response = await _client.SaveDataOnServer(_selectedRows.Cast<Person>().ToList());

                if (response.IsValid)
                {
                    ProgramStatus = ProgramStatusMessages.DataSaveSuccess;

                    _peopleCache.RemoveData(_selectedRows.Cast<Person>());

                    UpdateItemsCountFields();
                    CanSaveData = true;
                    CanUploadFile = true;
                }
                else
                {
                    ProgramStatus = response.Message;
                }
            }
            else
            {
                CanSaveData = true;
                CanUploadFile = true;
                ProgramStatus = ProgramStatusMessages.SelectValidData;
            }

        }

        private async void SaveLog(Log log)
        {
            SavingDataResponse logResponse = await _client.AddLog(log);

            if (!logResponse.IsValid)
            {
                ProgramStatus = logResponse.Message;
            }
        }

        internal async void UpdateDataButton_Click()
        {
            ProgramStatus = ProgramStatusMessages.WaitingForServerResponse;
            CanUploadFile = false;
            CanModifyPeople = false;

            if (_updatedPeople.Count > 0 && _updatedPeople.All(p => p.IsValid == true))
            {
                SavingDataResponse peopleResponse = await _client.UpdateData(_updatedPeople);

                SavingDataResponse logsResponse = await _client.AddLogs(_logs);

                if (peopleResponse.IsValid && logsResponse.IsValid)
                {
                    ProgramStatus = ProgramStatusMessages.DataUpdateSuccess;

                    CanModifyPeople = true;
                    CanUploadFile = true;
                }
                else
                {
                    ProgramStatus = peopleResponse.Message;
                }

                _updatedPeople.Clear();
                _logs.Clear();
            }
            else
            {
                CanSaveData = true;
                CanUploadFile = true;
                ProgramStatus = ProgramStatusMessages.SelectValidData;
            }
        }

        internal async void DeleteDataButton_Click()
        {
            ProgramStatus = ProgramStatusMessages.WaitingForServerResponse;
            CanUploadFile = false;
            CanModifyPeople = false;

            if (_selectedRows.Count > 0 && _selectedRows.All(p => (p as Person)?.IsValid == true))
            {
                SavingDataResponse response = await _client.DeletePeopleOnServer(_selectedRows.Cast<Person>().ToList());

                if (response.IsValid)
                {
                    ProgramStatus = ProgramStatusMessages.DataDeleteSuccess;

                    _peopleCache.RemoveData(_selectedRows.Cast<Person>());

                    UpdateItemsCountFields();

                    foreach (Person person in _selectedRows.Cast<Person>().ToList())
                    {
                        SaveLog(new Log(Guid.NewGuid(), person.Number, new OldNewValuePair(person.ToString(), null), LogActions.Deleted, DateTime.Now));
                    }

                    CanModifyPeople = true;
                    CanUploadFile = true;
                }
                else
                {
                    ProgramStatus = response.Message;
                }
            }
            else
            {
                ProgramStatus = ProgramStatusMessages.SelectValidData;
            }
        }

        internal bool SaveUpdatedPerson(Person oldPerson, Person updatedPerson)
        {
            try
            {
                KeyValuePair<string, OldNewValuePair> changes = GetChangedFields(oldPerson, updatedPerson).First();

                if (CanModifyPeople)
                {
                    updatedPerson.UpdateIsValidProperty();
                    Log log = new(updatedPerson.Number, new OldNewValuePair(changes.Value.OldValue, changes.Value.NewValue), LogActions.Updated);
                    if (_updatedPeople.Any(p => p.Number == updatedPerson.Number))
                    {
                        _updatedPeople.RemoveAll(p => p.Number == updatedPerson.Number);
                        _updatedPeople.Add(updatedPerson);
                    }
                    else
                    {
                        _updatedPeople.Add(updatedPerson);
                    }
                    _logs.Add(log);
                }
                return updatedPerson!.IsValid;
            }
            catch
            {
                return updatedPerson!.IsValid;
            }
        }

        internal void UpdateItemsCountFields()
        {
            if (_peopleCache.GetTempData().Count == 0)
            {
                List<Person> people = _peopleCache.GetData();
                InValidItems = people.Where(p => !p.IsValid).Count();
                ValidItems = people.Where(p => p.IsValid).Count();
                TotalItems = people.Count;
            }
            else
            {
                List<Person> people = _peopleCache.GetTempData();
                InValidItems = people.Where(p => !p.IsValid).Count();
                ValidItems = people.Where(p => p.IsValid).Count();
                TotalItems = people.Count;
            }
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

        private async void SaveFile(TopLevel topLevel, byte[] fileContent)
        {
            var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Save Text File",
                DefaultExtension = "xlsx"
            });

            if (file is not null)
            {
                await using var stream = await file.OpenWriteAsync();
                await stream.WriteAsync(fileContent);
            }
        }

        private IEnumerable<T> SearchData<T>(IEnumerable<T> items, string query)
        {
            return items.Where(item =>
                item.GetType().GetProperties()
                .Any(prop => prop.GetValue(item)?.ToString().Contains(query) == true)
            ).ToList();
        }

        private void PeopleCacheInit(CacheService<Person, int> peopleService)
        {
            peopleService.DataConnection()
                .Sort(SortExpressionComparer<Person>.Ascending(e => e.Number))
                .Page(_pager)
                .Do(change => PagingUpdate(change.Response))
                .Bind(_pagedPeople)
                .Subscribe();
        }

        private void LogCacheInit(CacheService<Log, Guid> logService)
        {
            logService.DataConnection()
                .Sort(SortExpressionComparer<Log>.Ascending(e => e.Id))
                .Page(_pager)
                .Do(change => PagingUpdate(change.Response))
                .Bind(_pagedLogs)
                .Subscribe();
        }

        private void PagingUpdate(IPageResponse response)
        {
            CurrentPage = response.Page;
            TotalPages = response.Pages;
        }

        private async Task<FileUploadRequest> GetSingleFileContent(IStorageFile file)
        {
            byte[] fileContent = Array.Empty<byte>();

            if (file != null)
            {
                try
                {
                    await using Stream stream = await file.OpenReadAsync();
                    using MemoryStream memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    fileContent = memoryStream.ToArray();
                }
                catch (IOException ex)
                {
                    ProgramStatus = ProgramStatusMessages.FileUsedByAnotherProgram;
                    return null;
                }
                catch (Exception ex)
                {
                    ProgramStatus = $"{ProgramStatusMessages.UnexpectedError}: {ex.Message}";
                    return null;
                }
            }

            return new FileUploadRequest(file!.Name, fileContent);
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

        private Dictionary<string, OldNewValuePair> GetChangedFields(Person original, Person updated)
        {
            Dictionary<string, OldNewValuePair> changes = new Dictionary<string, OldNewValuePair>();

            if (original.FirstName != updated.FirstName)
            {
                changes["FirstName"] = new(original.FirstName, updated.FirstName);
            }

            if (original.LastName != updated.LastName)
            {
                changes["LastName"] = new(original.LastName, updated.LastName);
            }

            if (original.Gender != updated.Gender)
            {
                changes["Gender"] = new(original.Gender.ToString(), updated.Gender.ToString());
            }

            if (original.Country != updated.Country)
            {
                changes["Country"] = new(original.Country, updated.Country);
            }

            if (original.Age != updated.Age)
            {
                changes["Age"] = new(original.Age.ToString(), updated.Age.ToString());
            }

            if (original.Birthday != updated.Birthday)
            {
                changes["Birthday"] = new(original.Birthday.ToString(), updated.Birthday.ToString());
            }

            return changes;
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