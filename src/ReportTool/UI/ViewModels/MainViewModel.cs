namespace ReportTool.UI.ViewModels
{
    using ReportTool.UI.Commands;
    using ReportTool.DataProviders;
    using ReportTool.Extensions;
    using ReportTool.Reports;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    public class MainViewModel : ViewModel
    {
        public const string NOT_SELECTED = "not selected";

        private readonly IDataProvider _provider;

        private readonly IScatterReportCalculator _calculator;

        private IEnumerable<Dictionary<string, double>> _currentData;

        private IEnumerable<SelectionType> _requiredSelectionTypes = new[] { SelectionType.Abscissa, SelectionType.Ordinate };

        private ReadOnlyCollection<DataColumnViewModel> _columns;

        public ReadOnlyCollection<DataColumnViewModel> Columns
        {
            get
            {
                return _columns;
            }

            private set
            {
                SetValue(ref _columns, value);
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
                SetValue(ref _errorMessage, value);
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
                SetValue(ref _report, value);
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

        private Command GenerateReportDataCommandImpl
        {
            get
            {
                return (Command)GenerateReportDataCommand;
            }
        }

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

            Columns = GetColumnsFrom(_currentData);
            ErrorMessage = dataResult.ErrorMessage;

            Report = ScatterReportData.Empty;
            NotifyColumnsChanged();
        }

        private ReadOnlyCollection<DataColumnViewModel> GetColumnsFrom(IEnumerable<Dictionary<string, double>> data)
        {
            var uniqueColumns = data.SelectMany(row => row.Keys).ToSet().Select(columnName => new DataColumnViewModel(columnName)).ToList();
            return new ReadOnlyCollection<DataColumnViewModel>(uniqueColumns);
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

        private void ToggleColumnSelection(DataColumnViewModel column)
        {
            column.SelectionType = GetAvailableSelectionTypeFor(column);

            if (IsAllRequiredColumnsSelected() == false)
            {
                Report = ScatterReportData.Empty;
            }

            NotifyColumnsChanged();
        }

        private bool IsAllRequiredColumnsSelected()
        {
            var alreadySelectedTypes = GetSelectedTypes();
            return _requiredSelectionTypes.All(type => alreadySelectedTypes.Contains(type));
        }

        private void NotifyColumnsChanged()
        {
            RaisePropertyChanged(nameof(AbscissaColumnName));
            RaisePropertyChanged(nameof(OrdinateColumnName));
            GenerateReportDataCommandImpl.RaiseCanExecuteChanged();
        }

        private SelectionType GetAvailableSelectionTypeFor(DataColumnViewModel viewmodel)
        {
            if (viewmodel.IsSelected)
            {
                return SelectionType.NotSelected;
            }

            var alreadySelectedTypes = GetSelectedTypes();
            Func<SelectionType, bool> notSelectedYet = type => alreadySelectedTypes.Contains(type) == false;

            return _requiredSelectionTypes.Where(notSelectedYet)
                                          .DefaultIfEmpty(SelectionType.NotSelected)
                                          .First();
        }

        private string GetColumnNameFor(SelectionType type)
        {
            return Columns.Where(column => column.SelectionType == type)
                          .Select(column => column.Name)
                          .DefaultIfEmpty(NOT_SELECTED)
                          .First();
        }

        private HashSet<SelectionType> GetSelectedTypes()
        {
            return Columns.Where(column => column.IsSelected).Select(column => column.SelectionType).ToSet();
        }
    }
}
