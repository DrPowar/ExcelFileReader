using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using DynamicData;
using ExcelFileReader.Constants;
using ExcelFileReader.InterfaceConverters;
using ExcelFileReader.Models;
using ExcelFileReader.ViewModels;
using System;
using System.Linq;

namespace ExcelFileReader.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;
        private bool _isDataPickerOpen = false;
        private bool _isGenderComboBoxOpen = false;
        private Person _originalPerson;

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

            var dataGrid = sender as DataGrid;
            if (dataGrid == null)
                return;

            var cell = dataGrid.Columns[1].GetCellContent(row)?.Parent as DataGridCell;
            if (cell != null)
            {
                cell.BorderThickness = new Thickness(10, 0, 0, 0);
            }

            cell.Bind(DataGridRow.BorderBrushProperty, new Binding("IsValid")
            {
                Converter = new BoolToColorConverter(),
            });
        }


        public void DataGrid_BeginningEdit(object? sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Row.DataContext is Person person)
            {
                _originalPerson = new Person
                {
                    Id = person.Id,
                    Number = person.Number,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Gender = person.Gender,
                    Country = person.Country,
                    Age = person.Age,
                    Birthday = person.Birthday,
                };
            }
        }



        public void RowDataGrid_EditingRow(object? sender, DataGridRowEditEndedEventArgs e)
        {
            if (e.Row.DataContext is Person editedPerson)
            {
                bool isValidPerson = _viewModel.SaveUpdatedPerson(_originalPerson, editedPerson);
                if (isValidPerson)
                {
                    e.Row.BorderBrush = new SolidColorBrush(Color.Parse(ColorsConst.Success));
                }
                else
                {
                    e.Row.BorderBrush = new SolidColorBrush(Color.Parse(ColorsConst.Error));
                }
            }
        }

        public void ItemsTypeCombobox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var selectedItem = comboBox.SelectedItem as ComboBoxItem;

            int itemCount = 0;

            if (selectedItem.Content.ToString().Contains("total items"))
            {
                _viewModel.ItemTypeCombobox_SelectionChanged("total");
            }
            else if (selectedItem.Content.ToString().Contains("invalid items"))
            {
                _viewModel.ItemTypeCombobox_SelectionChanged("invalid");
            }
            else if (selectedItem.Content.ToString().Contains("valid items"))
            {
                _viewModel.ItemTypeCombobox_SelectionChanged("valid");
            }
        }

        internal void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            DataGrid? dataGrid = sender as DataGrid;
            _viewModel.SelectedItems.Clear();
            foreach (object item in dataGrid!.SelectedItems)
            {
                _viewModel.SelectedItems.Add(item);
            }
            _viewModel.UpdateItemsCountFields();
        }

        private void DatePicker_TemplateApplied(object? sender, TemplateAppliedEventArgs e)
        {
            if (sender is DatePicker datePicker)
            {
                var popup = e.NameScope.Find<Popup>("PART_Popup");

                if (popup != null)
                {
                    popup.Closed += DatePickerPopupClosed;
                }
            }
        }

        private void GenderCombobox_TemplateApplied(object? sender, TemplateAppliedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                var popup = e.NameScope.Find<Popup>("PART_Popup");

                if (popup != null)
                {
                    popup.LostFocus += GenderComboBoxPopupClosed;
                }
            }
        }

        private void GenderComboBoxPopupClosed(object? sender, EventArgs e)
        {
            _isGenderComboBoxOpen = true;
        }

        private void GenderCombobox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (!_isGenderComboBoxOpen)
            {
                return;
            }

            ComboBox comboBox = sender as ComboBox;
            Person person = comboBox.DataContext as Person;

            if (person != null)
            {
                Person oldPerson = (Person)person.Clone();
                oldPerson.Gender = (Gender)e.RemovedItems[0];
                _viewModel.SaveUpdatedPerson(oldPerson, person);
            }

            _isGenderComboBoxOpen = false;
        }

        private void DatePickerPopupClosed(object? sender, EventArgs e)
        {
            _isDataPickerOpen = true;
        }

        private void DatePicker_SelectedDateChanged(object? sender, DatePickerSelectedValueChangedEventArgs e)
        {
            if (!_isDataPickerOpen)
            {
                return;
            }

            DatePicker datePicker = sender as DatePicker;
            Person person = datePicker.DataContext as Person;

            if (person != null)
            {
                Person oldPerson = (Person)person.Clone();
                person.Birthday = (DateTimeOffset)e.NewDate;
                _viewModel.SaveUpdatedPerson(oldPerson, person);
            }
            _isDataPickerOpen = false;
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