using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using DynamicData;
using ExcelFileReader.Constants;
using ExcelFileReader.DataTransfer;
using ExcelFileReader.InterfaceConverters;
using ExcelFileReader.Models;
using ExcelFileReader.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ExcelFileReader.Views
{
    public partial class MainWindow : Window
    {   
        private MainWindowViewModel _viewModel;
        public MainWindow()
        {
            _viewModel = new MainWindowViewModel(this);
            DataContext = _viewModel;
            InitializeComponent();
        }

        public void RowsDataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
        {
            var row = e.Row;
            row.Bind(DataGridRow.BackgroundProperty, new Binding("IsValid")
            {
                Converter = new BoolToColorConverter(),
            });
        }

        public void RowDataGrid_EditingRow(object? sender, DataGridRowEditEndedEventArgs e)
        {
            if(e.Row.DataContext is Person)
            {
                bool isValidPerson = _viewModel.UpdatePerson(e.Row.DataContext as Person);
                if (isValidPerson)
                {
                    e.Row.Background = RGBBrush.GetBrushFromRGB(RGBColors.RGBSuccessRed, RGBColors.RGBSuccessGreen, RGBColors.RGBSuccessBlue);
                }
                else
                {
                    e.Row.Background = RGBBrush.GetBrushFromRGB(RGBColors.RGBFailureRed, RGBColors.RGBFailureGreen, RGBColors.RGBFailureBlue);
                }
            }
        }

        internal void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            DataGrid? dataGrid = sender as DataGrid;
            _viewModel.SelectedPersons.Clear();
            foreach(Person person in dataGrid!.SelectedItems)
            {
                _viewModel.SelectedPersons.Add(person);
            }
        }

        private void Binding(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
        }
    }
}