namespace ReportTool
{
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
