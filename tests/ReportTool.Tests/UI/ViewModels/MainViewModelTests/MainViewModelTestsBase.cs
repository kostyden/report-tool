namespace ReportTool.Tests.UI.ViewModels.MainViewModelTests
{
    using NSubstitute;
    using NUnit.Framework;
    using ReportTool.DataProviders;
    using ReportTool.Reports;
    using ReportTool.UI;
    using ReportTool.UI.ViewModels;
    using System.Linq;

    [TestFixture]
    class MainViewModelTestsBase
    {
        protected IDataProvider FakeProvider { get; private set; }

        protected IScatterReportCalculator FakeScatterReportCalculator { get; private set; }

        protected MainViewModel ViewModel { get; private set; }

        [SetUp]
        public void SetUpBase()
        {
            FakeProvider = Substitute.For<IDataProvider>();
            FakeScatterReportCalculator = Substitute.For<IScatterReportCalculator>();

            ViewModel = new MainViewModel(FakeProvider, FakeScatterReportCalculator);
        }

        protected (DataColumnViewModel abscissa, DataColumnViewModel ordinate) SelectColumnsForReport(string abscissaColumnName, string ordinateColumnName)
        {
            var abscissaColumn = ViewModel.Columns.First(column => column.Name.Equals(abscissaColumnName));
            var ordinateColumn = ViewModel.Columns.First(column => column.Name.Equals(ordinateColumnName));

            abscissaColumn.SelectionType = SelectionType.Abscissa;
            ordinateColumn.SelectionType = SelectionType.Ordinate;

            return (abscissaColumn, ordinateColumn);
        }

    }
}
