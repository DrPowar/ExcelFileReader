using Avalonia.Controls;
using Avalonia.Interactivity;
using ExcelFileReader.Models;
using ExcelFileReader.ViewModels;
using System;
using System.Linq;

namespace ExcelFileReader.Views
{
    public partial class AddPersonWindow : Window
    {
        internal Person NewPerson { get; set; }
        

        public AddPersonWindow()
        {
            InitializeComponent();
            GenderComboBox.ItemsSource = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
        }

        private void OnCancelClick(object? sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnConfirmClick(object? sender, RoutedEventArgs e)
        {
            try
            {
                NewPerson = new Person(uint.Parse(IdTextBox.Text), FirstNameTextBox.Text, LastNameTextBox.Text, (Gender)GenderComboBox.SelectedValue, CountryTextBox.Text, (DateTimeOffset)BirthdayDatePicker.SelectedDate);
            }
            catch
            {
                NewPerson = null;
            }

            this.Close();
        }
    }
}