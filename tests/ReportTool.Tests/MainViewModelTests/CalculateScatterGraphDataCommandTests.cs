﻿namespace ReportTool.Tests.MainViewModelTests
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
        public void CalculateScatterGraphDataCommand_ShouldUpdateReportData()
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
        public void CalculateScatterGraphDataCommand_ShouldRaisePropertyChangedForReportData()
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
            ViewModel.MonitorEvents();

            ViewModel.CalculateScatterGraphDataCommand.Execute(null);

            ViewModel.ShouldRaisePropertyChangeFor(viewmodel => viewmodel.Report);
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
                AbscissaColumnName = "seven",
                OrdinateColumnName = "eight"
            };

            var dataResult = DataResult.CreateSuccessful(expectedInputData.Data);
            FakeProvider.GetFrom(null).ReturnsForAnyArgs(dataResult);


            var dummyReportData = new ScatterReportData(Enumerable.Empty<ScatterPoint>(), Enumerable.Empty<ScatterPoint>());
            FakeScatterReportCalculator.Calculate(Arg.Do<ScatterInputData>(input => actualInputData = input)).ReturnsForAnyArgs(dummyReportData);

            ViewModel.LoadDataCommand.Execute("dummy.path");
            ViewModel.Columns.First(column => column.Name.Equals("seven")).SelectionType = SelectionType.Abscissa;
            ViewModel.Columns.First(column => column.Name.Equals("eight")).SelectionType = SelectionType.Ordinate;

            ViewModel.CalculateScatterGraphDataCommand.Execute(null);

            actualInputData.ShouldBeEquivalentTo(expectedInputData);
        }
    }
}
