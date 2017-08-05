namespace ReportTool.Tests
{
    using FluentAssertions;
    using NSubstitute;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    class MainViewModelTests
    {
        private IDataProvider _fakeProvider;

        private MainViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _fakeProvider = Substitute.For<IDataProvider>();
            _viewModel = new MainViewModel(_fakeProvider);
        }

        [Test]
        public void Columns_ShouldBeEmptyAfterInitialization()
        {
            _viewModel.Columns.Should().BeEmpty();
        }

        [Test]
        public void Columns_ShouldContainLoadedColumnsAfterSuccessfulExecutingLoadDataCommand()
        {
            var validPath = @"somefile.xls";
            var loadedData = new[]
            {
                new Dictionary<string, double> { { "one", 1.23 }, { "two", 45 } },
                new Dictionary<string, double> { { "one", 1.0 }, { "two", 128.7 } },
                new Dictionary<string, double> { { "one", 3.14 }, { "two", 0.123 } }
            };
            _fakeProvider.GetFrom(validPath).Returns(loadedData);
            var expectedColumns = new[] { "one", "two" };

            _viewModel.LoadDataCommand.Execute(validPath);

            _viewModel.Columns.ShouldBeEquivalentTo(expectedColumns);
        }

        [Test]
        public void Columns_ShouldRaisePropertyChangedEventAfterSuccessfulExecutingLoadDataCommand()
        {
            var validPath = @"somefile.xls";
            var loadedData = new[]
            {
                new Dictionary<string, double> { { "three", 1.23 }, { "five", 4.785 }, { "two", 42 } }
            };
            _fakeProvider.GetFrom(validPath).Returns(loadedData);
            _viewModel.MonitorEvents();

            _viewModel.LoadDataCommand.Execute(validPath);

            _viewModel.ShouldRaisePropertyChangeFor(viewModel => viewModel.Columns);
        }
    }
}
