namespace ReportTool.Tests.UI.ViewModels.MainViewModelTests
{
    using FluentAssertions;
    using NSubstitute;
    using NUnit.Framework;
    using ReportTool.DataProviders;
    using ReportTool.Reports;
    using ReportTool.UI;
    using System.Collections.Generic;
    using System.Linq;

    class GenerateReportDataCommandTests : MainViewModelTestsBase
    {
        [Test]
        public void ShouldUpdateReportData()
        {
            var plotPoints = new[]
            {
                new ScatterPoint(1.23, 1.9),
                new ScatterPoint(1.59, 24.5),
                new ScatterPoint(0.5, 15),
            };
            var trendLine = new ScatterLine(new ScatterPoint(1, 12.12), new ScatterPoint(2, 16.34));
            var reportData = new ScatterReportData(plotPoints, trendLine);
            FakeScatterReportCalculator.Calculate(Arg.Any<ScatterInputData>()).Returns(reportData);

            ViewModel.GenerateReportDataCommand.Execute(null);

            ViewModel.Report.ShouldBeEquivalentTo(reportData);
        }

        [Test]
        public void ShouldRaisePropertyChangedForReportData()
        {
            var plotPoints = new[]
            {
                new ScatterPoint(1.23, 1.9),
                new ScatterPoint(1.59, 24.5),
                new ScatterPoint(0.5, 15),
            };
            var trendLine = new ScatterLine(new ScatterPoint(1, 12.12), new ScatterPoint(2, 16.34));

            var reportData = new ScatterReportData(plotPoints, trendLine);
            FakeScatterReportCalculator.Calculate(Arg.Any<ScatterInputData>()).Returns(reportData);
            ViewModel.MonitorEvents();

            ViewModel.GenerateReportDataCommand.Execute(null);

            ViewModel.ShouldRaisePropertyChangeFor(viewmodel => viewmodel.Report);
        }

        [Test]
        public void ShouldRaisePropertyChangedForReportDataCollection()
        {
            var plotPoints = new[]
            {
                new ScatterPoint(1, 2),
                new ScatterPoint(42, 22),
                new ScatterPoint(2, 3),
            };
            var trendLine = new ScatterLine(new ScatterPoint(1, 4), new ScatterPoint(32, 22));

            var reportData = new ScatterReportData(plotPoints, trendLine);
            FakeScatterReportCalculator.Calculate(Arg.Any<ScatterInputData>()).Returns(reportData);
            ViewModel.MonitorEvents();

            ViewModel.GenerateReportDataCommand.Execute(null);

            ViewModel.ShouldRaisePropertyChangeFor(viewmodel => viewmodel.ReportDataCollection);
        }

        [Test]
        public void ShouldPassCorrectInputValuesToTheCalculatorWhenSuccessfulyGetData()
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

            var dummyReportData = new ScatterReportData(Enumerable.Empty<ScatterPoint>(), new ScatterLine(new ScatterPoint(), new ScatterPoint()));
            FakeScatterReportCalculator.Calculate(Arg.Do<ScatterInputData>(input => actualInputData = input)).ReturnsForAnyArgs(dummyReportData);

            ViewModel.LoadDataCommand.Execute("dummy.path");
            ViewModel.Columns.First(column => column.Name.Equals("seven")).SelectionType = SelectionType.Abscissa;
            ViewModel.Columns.First(column => column.Name.Equals("eight")).SelectionType = SelectionType.Ordinate;

            ViewModel.GenerateReportDataCommand.Execute(null);

            actualInputData.ShouldBeEquivalentTo(expectedInputData);
        }

        [Test]
        public void CanExecute_ShouldReturnTrueWhenAbscissaAndOrdinateColumnsSelected()
        {
            ConfigureDataForCanExecuteTests();
            ViewModel.Columns.First(column => column.Name.Equals("one")).SelectionType = SelectionType.Abscissa;
            ViewModel.Columns.First(column => column.Name.Equals("three")).SelectionType = SelectionType.Ordinate;

            var canExecute = ViewModel.GenerateReportDataCommand.CanExecute(null);

            canExecute.Should().BeTrue();
        }

        [Test]
        public void CanExecute_ShouldReturnFalseWhenNoColumnsSelected()
        {
            ConfigureDataForCanExecuteTests();

            var canExecute = ViewModel.GenerateReportDataCommand.CanExecute(null);

            canExecute.Should().BeFalse();
        }

        [Test]
        public void CanExecute_ShouldReturnFalseWhenOnlyAbscissaColumnSelected()
        {
            ConfigureDataForCanExecuteTests();

            var canExecute = ViewModel.GenerateReportDataCommand.CanExecute(null);
            ViewModel.Columns.First(column => column.Name.Equals("two")).SelectionType = SelectionType.Abscissa;

            canExecute.Should().BeFalse();
        }

        [Test]
        public void CanExecute_ShouldReturnFalseWhenOnlyOrdinateColumnSelected()
        {
            ConfigureDataForCanExecuteTests();

            var canExecute = ViewModel.GenerateReportDataCommand.CanExecute(null);
            ViewModel.Columns.First(column => column.Name.Equals("three")).SelectionType = SelectionType.Ordinate;

            canExecute.Should().BeFalse();
        }

        private void ConfigureDataForCanExecuteTests()
        {
            var data = new[]
            {
                new Dictionary<string, double> { { "one", 0.00052 }, { "two", 1.000012}, { "three", 1.1} }
            };

            var dataResult = DataResult.CreateSuccessful(data);
            FakeProvider.GetFrom(null).ReturnsForAnyArgs(dataResult);

            ViewModel.LoadDataCommand.Execute("dummy.path");
        }
    }
}
