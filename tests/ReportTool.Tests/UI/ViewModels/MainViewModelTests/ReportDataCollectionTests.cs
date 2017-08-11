namespace ReportTool.Tests.UI.ViewModels.MainViewModelTests
{
    using FluentAssertions;
    using NSubstitute;
    using NUnit.Framework;
    using ReportTool.Reports;
    using System.Linq;

    [TestFixture]
    class ReportDataCollectionTests : MainViewModelTestsBase
    {
        [Test]
        public void ShouldContainAllCalculatedPlotPoints()
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

            ViewModel.GenerateReportDataCommand.Execute(null);

            var actualPoints = ViewModel.ReportDataCollection.OfType<ScatterPoint>();

            actualPoints.ShouldBeEquivalentTo(plotPoints);
        }

        [Test]
        public void ShouldContainCalculatedTrendLine()
        {
            var plotPoints = new[]
            {
                new ScatterPoint(1.0, 0.20),
                new ScatterPoint(4.2, 0.22),
                new ScatterPoint(2.0, 1.31),
            };
            var trendLine = new ScatterLine(new ScatterPoint(1, 4), new ScatterPoint(32, 22));

            var reportData = new ScatterReportData(plotPoints, trendLine);
            FakeScatterReportCalculator.Calculate(Arg.Any<ScatterInputData>()).Returns(reportData);

            ViewModel.GenerateReportDataCommand.Execute(null);

            var actualLine = ViewModel.ReportDataCollection.OfType<ScatterLine>().First();

            actualLine.ShouldBeEquivalentTo(trendLine);
        }
    }
}
