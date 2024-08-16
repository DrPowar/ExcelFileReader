using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Platform.Storage;
using DynamicData;
using ExcelFileReader.DataTransfer;
using ExcelFileReader.InterfaceConverters;
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
        private ObservableCollection<Person> _persons = new();
        private ObservableCollection<string> _files = new();

        public ObservableCollection<string> Files
        {
            get => _files;
            set => this.RaiseAndSetIfChanged(ref _files, value);
        }

        internal ObservableCollection<Person> Persons
        {
            get => _persons;
            set => this.RaiseAndSetIfChanged(ref _persons, value);
        }

        public DelegateCommand OpenFilePicker { get; init; }

        public MainWindowViewModel(Window window)
        {
            _client = new Client();

            OpenFilePicker = new DelegateCommand(OpenFileButton_Clicked);
            _window = window;
        }

        public async void OpenFileButton_Clicked()
        {
            TopLevel topLevel = TopLevel.GetTopLevel(_window);

            IReadOnlyCollection<IStorageFile> files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open Text File",
                AllowMultiple = false,
            });

            FileUploadRequest filesRequests = await GetSingleFileContent(files.First());

            if (filesRequests != null)
            {
                ParsingResponse response = await _client.SendFile(filesRequests.FileContent, filesRequests.FileName);

                if (response.IsValid)
                {
                    Persons.AddRange(response.Persons);
                }
                else
                {
                   //Show message on interface
                }
            }
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
    }
}