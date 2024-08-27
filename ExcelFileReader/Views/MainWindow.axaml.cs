using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using DynamicData;
using ExcelFileReader.Constants;
using ExcelFileReader.InterfaceConverters;
using ExcelFileReader.Models;
using ExcelFileReader.ViewModels;
using System;

namespace ExcelFileReader.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;
        private bool _datePickerIsLoading = true;
        private bool _genderComboBoxIsLoading = true;
        public MainWindow()
        {
            TopLevel topLevel = GetTopLevel(this);
            _viewModel = new MainWindowViewModel(topLevel);
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
            if (e.Row.DataContext is Person)
            {
                bool isValidPerson = _viewModel.UpdatePerson(e.Row.DataContext as Person);
                if (isValidPerson)
                {
                    e.Row.Background = new SolidColorBrush(Color.Parse(ColorsConst.Success));
                }
                else
                {
                    e.Row.Background = new SolidColorBrush(Color.Parse(ColorsConst.Error));
                }
            }
        }

        public void RowDataGrid_EditingDateTime(object? sender, DatePickerSelectedValueChangedEventArgs e)
        {
            if(_datePickerIsLoading)
            {
                return;
            }

            Person person = (sender as DatePicker).DataContext as Person;

            _viewModel.UpdatePerson(person);
        }

        public void RowDataGrid_EditingGender(object? sender, SelectionChangedEventArgs e)
        {
            if (_genderComboBoxIsLoading)
            {
                return;
            }

            Person person = (sender as ComboBox).DataContext as Person;

            _viewModel.UpdatePerson(person);
        }

        internal void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            DataGrid? dataGrid = sender as DataGrid;
            _viewModel.SelectedPeople.Clear();
            foreach (Person person in dataGrid!.SelectedItems)
            {
                _viewModel.SelectedPeople.Add(person);
            }
            _viewModel.UpdateItemsCountFields();
        }

        private void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            _datePickerIsLoading = false;
        }

        private void GenderComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            _genderComboBoxIsLoading = false;
        }

        private void Window_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                BeginMoveDrag(e);
            }
        }
    }
}