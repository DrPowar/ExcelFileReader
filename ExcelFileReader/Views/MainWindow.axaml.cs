using Avalonia.Controls;
using ExcelFileReader.ViewModels;
using System.Linq;

namespace ExcelFileReader.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(this);
        }
    }
}