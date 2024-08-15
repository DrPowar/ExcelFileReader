using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ExcelFileReader.DataTransfer;
using ExcelFileReader.Models;
using Prism.Commands;
using ReactiveUI;
using SkiaSharp;
using System;
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
        private ObservableCollection<ExcelFileResponse> _excelFilesResponses;
        private ObservableCollection<string> _files;

        public ObservableCollection<string> Files
        {
            get => _files;
            set => this.RaiseAndSetIfChanged(ref _files, value);
        }
        public ObservableCollection<ExcelFileResponse> ExcelFilesResponses
        {
            get => _excelFilesResponses;
            set => this.RaiseAndSetIfChanged(ref _excelFilesResponses, value);
        }

        public DelegateCommand OpenFilePicker { get; init; }

        public MainWindowViewModel(Window window)
        {
            _client = new Client();
            ExcelFilesResponses = new ObservableCollection<ExcelFileResponse>();

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
                AllowMultiple = true,
            });

            List<FileUploadRequest> filesRequests = await GetMultipleFilesContent(files);

            if (filesRequests != null)
            {
                foreach(FileUploadRequest file in filesRequests)
                {
                    FileParsingResponse response = await _client.SendFile(file.FileContent, file.FileName);

                    ExcelFilesResponses.Add(new ExcelFileResponse(response.Id, response.Message, response.FileName, response.IsValid));
                }
            }
        }

        private async Task<byte[]> GetSingleFileContent(IStorageFile file)
        {
            byte[] fileContent = Array.Empty<byte>();

            if (file != null)
            {
                await using Stream stream = await file.OpenReadAsync();
                using MemoryStream memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                fileContent = memoryStream.ToArray();
            }

            return fileContent;
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
    }
}