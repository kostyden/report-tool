﻿namespace ReportTool.Tests.UI.ViewModels.MainViewModelTests
{
    using FluentAssertions;
    using NSubstitute;
    using NUnit.Framework;
    using ReportTool.DataProviders;
    using ReportTool.Reports;
    using ReportTool.UI;
    using ReportTool.UI.ViewModels;
    using System.Collections.Generic;

    class LoadDataCommandTests : MainViewModelTestsBase
    {
        [Test]
        public void ShouldUpdateColumnsWithUniqueValuesWhenGetSuccessfulResult()
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
                new DataColumnViewModel("one") { SelectionType = SelectionType.NotSelected },
                new DataColumnViewModel("two") { SelectionType = SelectionType.NotSelected }
            };

            ViewModel.LoadDataCommand.Execute(validPath);

            ViewModel.Columns.ShouldBeEquivalentTo(expectedColumns);
        }

        [Test]
        public void ShouldRaiseOnPropertyChangedForColumnsWhenGetSuccessfulResult()
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
        public void ShouldRaiseCanExecuteForGenerateReportDataCommand()
        {
            var validPath = @"anotherFile.xls";
            var data = new[]
            {
                new Dictionary<string, double> { { "three", 1.23 }, { "five", 4.785 }, { "two", 42 } }
            };
            var result = DataResult.CreateSuccessful(data);
            FakeProvider.GetFrom(validPath).Returns(result);
            ViewModel.GenerateReportDataCommand.MonitorEvents();

            ViewModel.LoadDataCommand.Execute(validPath);

            ViewModel.GenerateReportDataCommand.ShouldRaise("CanExecuteChanged");
        }

        [Test]
        public void ShouldRaisePropertyChangedForAbscissaColumnName()
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

            ViewModel.ShouldRaisePropertyChangeFor(viewmodel => viewmodel.AbscissaColumnName);
        }

        [Test]
        public void ShouldRaisePropertyChangedForOrdinateColumnName()
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

            ViewModel.ShouldRaisePropertyChangeFor(viewmodel => viewmodel.OrdinateColumnName);
        }

        [Test]
        public void ShouldResetReportData()
        {
            var inputData = new ScatterInputData
            {
                Data = new[]
                {
                    new Dictionary<string, double> { { "seven", 0.00052 }, { "eight", 1.000012}, { "nine", 1.1} },
                    new Dictionary<string, double> { { "seven", 0.0000010101 }, { "eight", 0.12456 }, { "nine", 1.2 } }
                },
                AbscissaColumnName = "seven",
                OrdinateColumnName = "eight"
            };

            var dataResult = DataResult.CreateSuccessful(inputData.Data);
            FakeProvider.GetFrom(null).ReturnsForAnyArgs(dataResult);

            var dummyReportData = GenerateDummyReportData();
            FakeScatterReportCalculator.Calculate(null).ReturnsForAnyArgs(dummyReportData);

            ViewModel.LoadDataCommand.Execute("dummy.path");
            var (abscissaColumn, ordinateColumn) = SelectColumnsForReport("seven", "nine");
            ViewModel.GenerateReportDataCommand.Execute(null);

            ViewModel.LoadDataCommand.Execute("dummy2.path");

            ViewModel.Report.ShouldBeEquivalentTo(ScatterReportData.Empty);
        }

        [Test]
        public void ShouldClearErrorMessageWhenGetSuccessfulResult()
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
        public void ShouldRaisePropertyChangedForErrorMessageWhenSuccessfulResultChangedToFailed()
        {
            var validPath = @"data.xlsx";
            var data = new[]
            {
                new Dictionary<string, double> { { "five", 0}, { "six", 0 } }
            };
            var successfulResult = DataResult.CreateSuccessful(data);
            FakeProvider.GetFrom(validPath).Returns(successfulResult);

            var invalidPath = @"notexisted.file";
            var failedResult = DataResult.CreateFailed("File not found");
            FakeProvider.GetFrom(invalidPath).Returns(failedResult);

            ViewModel.LoadDataCommand.Execute(validPath);

            ViewModel.MonitorEvents();

            ViewModel.LoadDataCommand.Execute(invalidPath);

            ViewModel.ShouldRaisePropertyChangeFor(viewModel => viewModel.ErrorMessage);
        }

        [Test]
        public void ShouldRaisePropertyChangedForErrorMessageWhenFailedResultChangedToSuccessful()
        {
            var validPath = @"data.xlsx";
            var data = new[]
            {
                new Dictionary<string, double> { { "five", 0}, { "six", 0 } }
            };
            var successfulResult = DataResult.CreateSuccessful(data);
            FakeProvider.GetFrom(validPath).Returns(successfulResult);

            var invalidPath = @"notexisted.file";
            var failedResult = DataResult.CreateFailed("File not found");
            FakeProvider.GetFrom(invalidPath).Returns(failedResult);

            ViewModel.LoadDataCommand.Execute(invalidPath);

            ViewModel.MonitorEvents();

            ViewModel.LoadDataCommand.Execute(validPath);

            ViewModel.ShouldRaisePropertyChangeFor(viewModel => viewModel.ErrorMessage);
        }

        [Test]
        public void ShouldClearColumnsWhenGetFailedResult()
        {
            var path = @"notExistedFile.xls";
            var failedResult = DataResult.CreateFailed("File not found");
            FakeProvider.GetFrom(path).Returns(failedResult);

            ViewModel.LoadDataCommand.Execute(path);

            ViewModel.Columns.Should().BeEmpty();
        }

        [Test]
        public void ShouldRaiseOnPropertyChangedForColumnsWhenGetFailedResult()
        {
            var path = @"wrongType.xls";
            var failedResult = DataResult.CreateFailed("Provider doesn not support this type of file");
            FakeProvider.GetFrom(path).Returns(failedResult);
            ViewModel.MonitorEvents();

            ViewModel.LoadDataCommand.Execute(path);

            ViewModel.ShouldRaisePropertyChangeFor(viewModel => viewModel.Columns);
        }

        [Test]
        public void ShouldRaiseOnPropertyChangedForErrorMessageWhenGetFailedResult()
        {
            var path = @"file.txt";
            var failedResult = DataResult.CreateFailed("Provider doesn not support this type of file");
            FakeProvider.GetFrom(path).Returns(failedResult);
            ViewModel.MonitorEvents();

            ViewModel.LoadDataCommand.Execute(path);

            ViewModel.ShouldRaisePropertyChangeFor(viewModel => viewModel.ErrorMessage);
        }

        private ScatterReportData GenerateDummyReportData()
        {
            var points = new[]
            {
                new ScatterPoint(1.2, 4.3),
                new ScatterPoint(1.3, 4.2),
                new ScatterPoint(1.4, 4.1),
                new ScatterPoint(1.5, 4.0)
            };
            var trendLine = new ScatterLine(new ScatterPoint(1.0, 4.15), new ScatterPoint(1.5, 4.15));

            return new ScatterReportData(points, trendLine);
        }
    }
}
