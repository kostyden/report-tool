namespace ReportTool
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public class MainViewModel
    {
        public ObservableCollection<string> Columns { get; set; }

        public ICommand LoadDataCommand { get; }

        public MainViewModel()
        {
            Columns = new ObservableCollection<string>();
            
        }
    }
}
