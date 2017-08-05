namespace ReportTool
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IDataProvider _provider;

        public event PropertyChangedEventHandler PropertyChanged;

        private ReadOnlyCollection<string> _columns;
        public ReadOnlyCollection<string> Columns
        {
            get
            {
                return _columns;
            }

            private set
            {
                _columns = value;
                RaisePropertyChanged();
            }
        }

        public ICommand LoadDataCommand { get; }

        public MainViewModel(IDataProvider provider)
        {
            _provider = provider;
                
            Columns = new ReadOnlyCollection<string>(new List<string>());
            LoadDataCommand = new Command(param => LoadData((string)param));
        }

        private void LoadData(string path)
        {
            var data = _provider.GetFrom(path);
            var uniqueColumns = data.SelectMany(row => row.Keys).Distinct().ToList();
            Columns = new ReadOnlyCollection<string>(uniqueColumns);
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
