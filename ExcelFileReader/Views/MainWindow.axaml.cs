using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Data;
using Avalonia.Styling;
using ExcelFileReader.ViewModels;
using System.Linq;
using ExcelFileReader.InterfaceConverters;

namespace ExcelFileReader.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(this);
        }

        public void RowsDataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
        {
            var row = e.Row;
            row.Bind(DataGridRow.BackgroundProperty, new Binding("IsValid")
            {
                Converter = new BoolToColorConverter(),
            });
        }
    }
}