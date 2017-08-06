namespace ReportTool.Tests
{
    using FluentAssertions;
    using NSubstitute;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    class MainViewModelTests
    {
        private IDataProvider _fakeProvider;

        private IScatterReportCalculator _fakeScatterReportCalculator;

        private MainViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _fakeProvider = Substitute.For<IDataProvider>();
            _fakeScatterReportCalculator = Substitute.For<IScatterReportCalculator>();

            _viewModel = new MainViewModel(_fakeProvider, _fakeScatterReportCalculator);
        }

        [Test]
        public void Columns_ShouldBeEmptyAfterInitialization()
        {
            _viewModel.Columns.Should().BeEmpty();
        }

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
            _fakeProvider.GetFrom(validPath).Returns(result);
            var expectedColumns = new[] { "one", "two" };

            _viewModel.LoadDataCommand.Execute(validPath);

            _viewModel.Columns.ShouldBeEquivalentTo(expectedColumns);
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
            _fakeProvider.GetFrom(validPath).Returns(result);
            _viewModel.MonitorEvents();

            _viewModel.LoadDataCommand.Execute(validPath);

            _viewModel.ShouldRaisePropertyChangeFor(viewModel => viewModel.Columns);
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
            _fakeProvider.GetFrom(validPath).Returns(result);

            _viewModel.LoadDataCommand.Execute(validPath);

            _viewModel.ErrorMessage.Should().BeNull();
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
            _fakeProvider.GetFrom(validPath).Returns(result);
            _viewModel.MonitorEvents();

            _viewModel.LoadDataCommand.Execute(validPath);

            _viewModel.ShouldRaisePropertyChangeFor(viewModel => viewModel.ErrorMessage);
        }

        [Test]
        public void LoadDataCommand_WhenGetFailedResultShouldClearColumns()
        {
            var path = @"notExistedFile.xls";
            var failedResult = DataResult.CreateFailed("File not found");
            _fakeProvider.GetFrom(path).Returns(failedResult);

            _viewModel.LoadDataCommand.Execute(path);

            _viewModel.Columns.Should().BeEmpty();
        }

        [Test]
        public void LoadDataCommand_WhenGetFailedResultShouldRaiseOnPropertyChangedForColumns()
        {
            var path = @"wrongType.xls";
            var failedResult = DataResult.CreateFailed("Provider doesn not support this type of file");
            _fakeProvider.GetFrom(path).Returns(failedResult);
            _viewModel.MonitorEvents();

            _viewModel.LoadDataCommand.Execute(path);

            _viewModel.ShouldRaisePropertyChangeFor(viewModel => viewModel.Columns);
        }

        [Test]
        public void LoadDataCommand_WhenGetFailedResultShouldRaiseOnPropertyChangedForErrorMessage()
        {
            var path = @"file.txt";
            var failedResult = DataResult.CreateFailed("Provider doesn not support this type of file");
            _fakeProvider.GetFrom(path).Returns(failedResult);
            _viewModel.MonitorEvents();

            _viewModel.LoadDataCommand.Execute(path);

            _viewModel.ShouldRaisePropertyChangeFor(viewModel => viewModel.ErrorMessage);
        }

        [Test]
        public void CalculateScatterGraphDataCommand_ShouldUpdateGraphData()
        {
            var plotPoints = new[]
            {
                new ScatterPoint(1.23, 1.9),
                new ScatterPoint(1.59, 24.5),
                new ScatterPoint(0.5, 15),
            };
            var trendLinePoints = new[]
            {
                new ScatterPoint(1, 12.12),
                new ScatterPoint(2, 16.34)
            };
            var reportData = new ScatterReportData(plotPoints, trendLinePoints);
            _fakeScatterReportCalculator.Calculate(Arg.Any<ScatterInputData>()).Returns(reportData);

            _viewModel.CalculateScatterGraphDataCommand.Execute(null);

            _viewModel.Report.ShouldBeEquivalentTo(reportData);
        }

        [Test]
        public void CalculateScatterGraphDataCommand_ShouldPassCorrectInputValuesToTheCalculatorWhenSuccessfulyGetData()
        {
            ScatterInputData actualInputData = null;
            var expectedInputData = new ScatterInputData
            {
                Data = new[]
                {
                    new Dictionary<string, double> { { "seven", 0.00052 }, { "eight", 1.000012}, { "nine", 1.1} },
                    new Dictionary<string, double> { { "seven", 0.0000010101 }, { "eight", 0.12456 }, { "nine", 1.2 } }
                },
                ColumnNameForX = "seven",
                ColumnNameForY = "eight"
            };

            var dataResult = DataResult.CreateSuccessful(expectedInputData.Data);
            _fakeProvider.GetFrom(null).ReturnsForAnyArgs(dataResult);

            var dummyReportData = new ScatterReportData(Enumerable.Empty<ScatterPoint>(), Enumerable.Empty<ScatterPoint>());
            _fakeScatterReportCalculator.Calculate(Arg.Do<ScatterInputData>(input => actualInputData = input)).ReturnsForAnyArgs(dummyReportData);

            _viewModel.LoadDataCommand.Execute("dummy.path");
            _viewModel.ColumnNameForX = expectedInputData.ColumnNameForX;
            _viewModel.ColumnNameForY = expectedInputData.ColumnNameForY;

            _viewModel.CalculateScatterGraphDataCommand.Execute(null);

            actualInputData.ShouldBeEquivalentTo(expectedInputData);
        }
    }
}
