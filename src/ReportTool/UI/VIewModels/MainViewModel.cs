namespace ReportTool.UI.ViewModels
{
    using ReportTool.Commands;
    using ReportTool.DataProviders;
    using ReportTool.Reports;
    using System;
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

        private IEnumerable<SelectionType> _requiredSelectionTypes = new[] { SelectionType.Abscissa, SelectionType.Ordinate };

        public event PropertyChangedEventHandler PropertyChanged;

        private ReadOnlyCollection<DataColumnViewModel> _columns;

        public ReadOnlyCollection<DataColumnViewModel> Columns
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

        public string AbscissaColumnName
        {
            get
            {
                return GetColumnNameFor(SelectionType.Abscissa);
            }
        }

        public string OrdinateColumnName
        {
            get
            {
                return GetColumnNameFor(SelectionType.Ordinate);
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
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ReportDataCollection));
            }
        }

        public IEnumerable<object> ReportDataCollection
        {
            get
            {
                var collection = new List<object>();
                collection.AddRange(_report.PlotPoints.Cast<object>());
                collection.Add(_report.TrendLine);

                return collection;
            }
        }

        public ICommand LoadDataCommand { get; }

        public ICommand GenerateReportDataCommand { get; }

        public ICommand ToggleColumnSelectionCommand { get; }

        public MainViewModel(IDataProvider provider, IScatterReportCalculator calculator)
        {
            _provider = provider;
            _calculator = calculator;

            Columns = new ReadOnlyCollection<DataColumnViewModel>(new List<DataColumnViewModel>());
            Report = new ScatterReportData(Enumerable.Empty<ScatterPoint>(), ScatterLine.Zero);
            LoadDataCommand = new Command(param => LoadData((string)param));
            GenerateReportDataCommand = new Command(param => GenerateReportData(), param => IsAllRequiredColumnsSelected());
            ToggleColumnSelectionCommand = new Command(column => ToggleColumnSelection((DataColumnViewModel)column));
        }

        private void LoadData(string path)
        {
            var dataResult = _provider.GetFrom(path);
            _currentData = dataResult.Data;
            var uniqueColumns = dataResult.Data.SelectMany(row => row.Keys).Distinct().Select(columnName => new DataColumnViewModel(columnName)).ToList();
            Columns = new ReadOnlyCollection<DataColumnViewModel>(uniqueColumns);
            ErrorMessage = dataResult.ErrorMessage;
            Report = ScatterReportData.Empty;
            RaisePropertyChanged(nameof(AbscissaColumnName));
            RaisePropertyChanged(nameof(OrdinateColumnName));
            ((Command)GenerateReportDataCommand).RaiseCanExecuteChanged();
        }

        private void GenerateReportData()
        {
            var inputData = new ScatterInputData
            {
                Data = _currentData,
                AbscissaColumnName = AbscissaColumnName,
                OrdinateColumnName = OrdinateColumnName
            };

            Report = _calculator.Calculate(inputData);
        }

        private bool IsAllRequiredColumnsSelected()
        {
            var alreadySelectedTypes = Columns.Where(column => column.IsSelected).Select(column => column.SelectionType).ToSet();
            return _requiredSelectionTypes.All(type => alreadySelectedTypes.Contains(type));
        }

        private void ToggleColumnSelection(DataColumnViewModel column)
        {
            column.SelectionType = GetAvailableSelectionTypeFor(column);

            if (IsAllRequiredColumnsSelected() == false)
            {
                Report = ScatterReportData.Empty;
            }

            RaisePropertyChanged(nameof(AbscissaColumnName));
            RaisePropertyChanged(nameof(OrdinateColumnName));
            ((Command)GenerateReportDataCommand).RaiseCanExecuteChanged();
        }

        private SelectionType GetAvailableSelectionTypeFor(DataColumnViewModel viewmodel)
        {
            if (viewmodel.IsSelected)
            {
                return SelectionType.NotSelected;
            }

            var alreadySelectedTypes = Columns.Where(column => column.IsSelected).Select(column => column.SelectionType).ToSet();
            Func<SelectionType, bool> notSelectedYet = type => alreadySelectedTypes.Contains(type) == false;

            return _requiredSelectionTypes.Where(notSelectedYet)
                                           .DefaultIfEmpty(SelectionType.NotSelected)
                                           .First();
        }

        private string GetColumnNameFor(SelectionType type)
        {
            return Columns.Where(column => column.SelectionType == type)
                              .Select(column => column.Name)
                              .DefaultIfEmpty("not selected")
                              .First();
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
