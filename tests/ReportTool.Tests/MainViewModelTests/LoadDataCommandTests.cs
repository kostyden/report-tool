namespace ReportTool.Tests.MainViewModelTests
{
    using FluentAssertions;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    class LoadDataCommandTests : MainViewModelTestsBase
    {
        [Test]
        public void LoadDataCommand_WhenGetSuccessfulResultShouldUpdateColumnsWithUniqueValues()
        {
            var validPath = @"somefile.xls";
            var data = new[]
            {
                new Dictionary<string, double> { { "one", 1.23 }, { "two", 45 } },
                new Dictionary<string, double> { { "one", 1.0 }, { "two", 128.7 } },
                new Dictionary<string, double> { { "one", 3.14 }, { "two", 0.123 } }
            };
            var result = DataResult.CreateSuccessful(data);
            FakeProvider.GetFrom(validPath).Returns(result);
            var expectedColumns = new[]
            {
                new DataColumnViewModel("one"),
                new DataColumnViewModel("two")
            };

            ViewModel.LoadDataCommand.Execute(validPath);

            ViewModel.Columns.ShouldBeEquivalentTo(expectedColumns);
        }

        [Test]
        public void LoadDataCommand_WhenGetSuccessfulResultShouldRaiseOnPropertyChangedForColumns()
        {
            var validPath = @"anotherFile.xls";
            var data = new[]
            {
                new Dictionary<string, double> { { "three", 1.23 }, { "five", 4.785 }, { "two", 42 } }
            };
            var result = DataResult.CreateSuccessful(data);
            FakeProvider.GetFrom(validPath).Returns(result);
            ViewModel.MonitorEvents();

            ViewModel.LoadDataCommand.Execute(validPath);

            ViewModel.ShouldRaisePropertyChangeFor(viewModel => viewModel.Columns);
        }

        [Test]
        public void LoadDataCommand_WhenGetSuccessfulResultShouldClearErrorMessage()
        {
            var validPath = @"file.csv";
            var data = new[]
            {
                new Dictionary<string, double> { { "three", 0.2 }, { "four", -45.34 } },
                new Dictionary<string, double> { { "three", 0}, { "four", 1.55 } }
            };
            var result = DataResult.CreateSuccessful(data);
            FakeProvider.GetFrom(validPath).Returns(result);

            ViewModel.LoadDataCommand.Execute(validPath);

            ViewModel.ErrorMessage.Should().BeNull();
        }

        [Test]
        public void LoadDataCommand_WhenGetSuccessfulResultShouldRaisePropertyChangedForErrorMessage()
        {
            var validPath = @"data.xlsx";
            var data = new[]
            {
                new Dictionary<string, double> { { "five", 0}, { "six", 0 } }
            };
            var result = DataResult.CreateSuccessful(data);
            FakeProvider.GetFrom(validPath).Returns(result);
            ViewModel.MonitorEvents();

            ViewModel.LoadDataCommand.Execute(validPath);

            ViewModel.ShouldRaisePropertyChangeFor(viewModel => viewModel.ErrorMessage);
        }

        [Test]
        public void LoadDataCommand_WhenGetFailedResultShouldClearColumns()
        {
            var path = @"notExistedFile.xls";
            var failedResult = DataResult.CreateFailed("File not found");
            FakeProvider.GetFrom(path).Returns(failedResult);

            ViewModel.LoadDataCommand.Execute(path);

            ViewModel.Columns.Should().BeEmpty();
        }

        [Test]
        public void LoadDataCommand_WhenGetFailedResultShouldRaiseOnPropertyChangedForColumns()
        {
            var path = @"wrongType.xls";
            var failedResult = DataResult.CreateFailed("Provider doesn not support this type of file");
            FakeProvider.GetFrom(path).Returns(failedResult);
            ViewModel.MonitorEvents();

            ViewModel.LoadDataCommand.Execute(path);

            ViewModel.ShouldRaisePropertyChangeFor(viewModel => viewModel.Columns);
        }

        [Test]
        public void LoadDataCommand_WhenGetFailedResultShouldRaiseOnPropertyChangedForErrorMessage()
        {
            var path = @"file.txt";
            var failedResult = DataResult.CreateFailed("Provider doesn not support this type of file");
            FakeProvider.GetFrom(path).Returns(failedResult);
            ViewModel.MonitorEvents();

            ViewModel.LoadDataCommand.Execute(path);

            ViewModel.ShouldRaisePropertyChangeFor(viewModel => viewModel.ErrorMessage);
        }
    }
}
