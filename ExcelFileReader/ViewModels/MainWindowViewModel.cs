using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ExcelFileReader.DataTransfer;
using ExcelFileReader.Models;
using Prism.Commands;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelFileReader.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly Client _client;
        private readonly Window _window;
        private ObservableCollection<ExcelError> _errors;
        private ObservableCollection<string> _files;

        public ObservableCollection<string> Files
        {
            get => _files;
            set => this.RaiseAndSetIfChanged(ref _files, value);
        }
        public ObservableCollection<ExcelError> Errors
        {
            get => _errors;
            set => this.RaiseAndSetIfChanged(ref _errors, value);
        }

        public DelegateCommand OpenFilePicker { get; init; }

        public MainWindowViewModel(Window window)
        {
            _client = new Client();
            Errors = new ObservableCollection<ExcelError>
            {
                new ExcelError("Error message 1."),
                new ExcelError("Error message 2."),
                new ExcelError("Error message 3."),
                new ExcelError("Error message 4.")
            };

            Files = new ObservableCollection<string>
            {
                "xml",
                "json",
                "txt",
                "xlsl",
                "docx"
            };

            OpenFilePicker = new DelegateCommand(OpenFileButton_Clicked);
            _window = window;
        }

        public async void OpenFileButton_Clicked()
        {
            TopLevel topLevel = TopLevel.GetTopLevel(_window);

            IReadOnlyList<IStorageFile> files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open Text File",
                AllowMultiple = false,
            });

            string fileContent = await GetSingleFileContent(files.FirstOrDefault());

            if (fileContent != null)
            {
                await _client.SendFile(fileContent);
            }
        }

        private async Task<List<string>> GetMultipleFilesContent(IReadOnlyList<IStorageFile> files)
        {
            List<string> fileContent = new List<string>();
            if (files.Count >= 1)
            {
                await using Stream stream = await files[0].OpenReadAsync();
                using StreamReader streamReader = new StreamReader(stream);
                fileContent.Add(await streamReader.ReadToEndAsync());
            }
            return fileContent;
        }

        private async Task<string> GetSingleFileContent(IStorageFile file)
        {
            string fileContent = string.Empty;

            if (file != null)
            {
                await using Stream stream = await file.OpenReadAsync();
                using StreamReader streamReader = new StreamReader(stream);
                fileContent = await streamReader.ReadToEndAsync();
            }
            
            return fileContent;
        }
    }
}