namespace ReportTool
{
    using System;
    using System.Collections.ObjectModel;

    public class MainViewModel
    {
        public ObservableCollection<string> Columns { get; set; }

        public MainViewModel()
        {
            Columns = new ObservableCollection<string>();
        }
    }
}
