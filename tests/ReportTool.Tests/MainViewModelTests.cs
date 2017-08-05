namespace ReportTool.Tests
{
    using FluentAssertions;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    class MainViewModelTests
    {
        [Test]
        public void Columns_ShouldBeEmptyAfterInitialization()
        {
            var fakeProvider = Substitute.For<IDataProvider>();
            var viewmodel = new MainViewModel(fakeProvider);

            viewmodel.Columns.Should().BeEmpty();
        }

        [Test]
        public void Columns_ShouldContainLoadedColumnsAfterSuccessfulExecutingLoadDataCommand()
        {
            var fakeProvider= Substitute.For<IDataProvider>();
            var viewmodel = new MainViewModel(fakeProvider);
            var validPath = @"somefile.xls";
            var loadedData = new[]
            {
                new Dictionary<string, double> { { "one", 1.23 }, { "two", 45 } },
                new Dictionary<string, double> { { "one", 1.0 }, { "two", 128.7 } },
                new Dictionary<string, double> { { "one", 3.14 }, { "two", 0.123 } }
            };
            fakeProvider.GetFrom(validPath).Returns(loadedData);
            var expectedColumns = new[] { "one", "two" };

            viewmodel.LoadDataCommand.Execute(validPath);

            viewmodel.Columns.ShouldBeEquivalentTo(expectedColumns);
        }

        [Test]
        public void Columns_ShouldRaisePropertyChangedEventAfterSuccessfulExecutingLoadDataCommand()
        {
            var fakeProvider = Substitute.For<IDataProvider>();
            var viewmodel = new MainViewModel(fakeProvider);
            var validPath = @"somefile.xls";
            var loadedData = new[]
            {
                new Dictionary<string, double> { { "three", 1.23 }, { "five", 4.785 }, { "two", 42 } }
            };
            fakeProvider.GetFrom(validPath).Returns(loadedData);
            viewmodel.MonitorEvents();

            viewmodel.LoadDataCommand.Execute(validPath);

            viewmodel.ShouldRaisePropertyChangeFor(viewModel => viewModel.Columns);
        }
    }
}
