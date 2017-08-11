namespace ReportTool.Tests.UI.ViewModels.MainViewModelTests
{
    using NSubstitute;
    using NUnit.Framework;
    using ReportTool.DataProviders;
    using ReportTool.Reports;
    using ReportTool.UI.ViewModels;

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
    }
}
