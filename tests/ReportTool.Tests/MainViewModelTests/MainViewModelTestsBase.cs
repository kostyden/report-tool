namespace ReportTool.Tests.MainViewModelTests
{
    using FluentAssertions;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    class MainViewModelTestsBase
    {
        protected IDataProvider FakeProvider { get; private set; }

        protected IScatterReportCalculator FakeScatterReportCalculator { get; private set; }

        protected MainViewModel ViewModel { get; private set; }

        [SetUp]
        public void SetUp()
        {
            FakeProvider = Substitute.For<IDataProvider>();
            FakeScatterReportCalculator = Substitute.For<IScatterReportCalculator>();

            ViewModel = new MainViewModel(FakeProvider, FakeScatterReportCalculator);
        }

        [Test]
        public void Columns_ShouldBeEmptyAfterInitialization()
        {
            ViewModel.Columns.Should().BeEmpty();
        }
    }
}
