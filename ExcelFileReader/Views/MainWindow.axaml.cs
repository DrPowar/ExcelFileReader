using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using ExcelFileReader.Constants;
using ExcelFileReader.InterfaceConverters;
using ExcelFileReader.Models;
using ExcelFileReader.ViewModels;

namespace ExcelFileReader.Views
{
    public partial class MainWindow : Window
    {   
        private MainWindowViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainWindowViewModel(this);
            DataContext = _viewModel;
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
    }
}