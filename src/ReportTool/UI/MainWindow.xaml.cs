namespace ReportTool
{
    using ReportTool.UI.ViewModels;
    using System.Windows;

    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewmodel)
        {
            InitializeComponent();
            DataContext = viewmodel;
        }
    }
}
