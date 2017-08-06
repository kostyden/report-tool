namespace ReportTool.Tests.MainViewModelTests
{
    using FluentAssertions;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class CalculateScatterGraphDataCommandTests : MainViewModelTestsBase
    {
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
            FakeScatterReportCalculator.Calculate(Arg.Any<ScatterInputData>()).Returns(reportData);

            ViewModel.CalculateScatterGraphDataCommand.Execute(null);

            ViewModel.Report.ShouldBeEquivalentTo(reportData);
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
            FakeProvider.GetFrom(null).ReturnsForAnyArgs(dataResult);

            var dummyReportData = new ScatterReportData(Enumerable.Empty<ScatterPoint>(), Enumerable.Empty<ScatterPoint>());
            FakeScatterReportCalculator.Calculate(Arg.Do<ScatterInputData>(input => actualInputData = input)).ReturnsForAnyArgs(dummyReportData);

            ViewModel.LoadDataCommand.Execute("dummy.path");
            ViewModel.ColumnNameForX = expectedInputData.ColumnNameForX;
            ViewModel.ColumnNameForY = expectedInputData.ColumnNameForY;

            ViewModel.CalculateScatterGraphDataCommand.Execute(null);

            actualInputData.ShouldBeEquivalentTo(expectedInputData);
        }
    }
}
