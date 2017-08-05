namespace ReportTool
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;

    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IDataProvider _provider;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Columns { get; set; }

        public ICommand LoadDataCommand { get; }

        public MainViewModel(IDataProvider provider)
        {
            _provider = provider;
                
            Columns = new ObservableCollection<string>();
            LoadDataCommand = new Command(param => LoadData((string)param));
        }

        private void LoadData(string path)
        {
            var data = _provider.GetFrom(path);
            var uniqueColumns = data.SelectMany(row => row.Keys).Distinct();
            Columns = new ObservableCollection<string>(uniqueColumns);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Columns)));
        }
    }
}
