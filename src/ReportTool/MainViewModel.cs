﻿namespace ReportTool
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

        private readonly IScatterReportCalculator _calculator;

        private IEnumerable<Dictionary<string, double>> _currentData;

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

        private string _errorMessage;

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            private set
            {
                _errorMessage = value;
                RaisePropertyChanged();
            }
        }

        private string _columnNameForX;

        public string ColumnNameForX
        {
            get
            {
                return _columnNameForX;
            }

            set
            {
                _columnNameForX = value;
            }
        }

        private string _columnNameForY;

        public string ColumnNameForY
        {
            get
            {
                return _columnNameForY;
            }

            set
            {
                _columnNameForY = value;
            }
        }

        private ScatterReportData _report;

        public ScatterReportData Report
        {
            get
            {
                return _report;
            }

            private set
            {
                _report = value;
            }
        }

        public ICommand LoadDataCommand { get; }

        public ICommand CalculateScatterGraphDataCommand { get; }

        public MainViewModel(IDataProvider provider, IScatterReportCalculator calculator)
        {
            _provider = provider;
            _calculator = calculator;
                
            Columns = new ReadOnlyCollection<string>(new List<string>());
            LoadDataCommand = new Command(param => LoadData((string)param));
            CalculateScatterGraphDataCommand = new Command(param => CalculateScatterReportData());
        }

        private void LoadData(string path)
        {
            var dataResult = _provider.GetFrom(path);
            _currentData = dataResult.Data;
            var uniqueColumns = dataResult.Data.SelectMany(row => row.Keys).Distinct().ToList();
            Columns = new ReadOnlyCollection<string>(uniqueColumns);
            ErrorMessage = dataResult.ErrorMessage;
        }

        private void CalculateScatterReportData()
        {
            var inputData = new ScatterInputData
            {
                Data = _currentData,
                ColumnNameForX = ColumnNameForX,
                ColumnNameForY = ColumnNameForY
            };

            Report = _calculator.Calculate(inputData);
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
