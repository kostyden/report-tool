namespace ReportTool.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Linq;

    [TestFixture]
    public class ScatterReportDataTests
    {
        private ScatterLine _dummyLine = new ScatterLine(new ScatterPoint(), new ScatterPoint());

        [Test]
        public void MinPoint_ShouldReturnPointWithMinXAndYValues()
        {
            var points = new[]
            {
                new ScatterPoint(10, 12),
                new ScatterPoint(13, 34),
                new ScatterPoint(2, 24)
            };
            var expectedMinPoint = new ScatterPoint(2, 12);

            var report = new ScatterReportData(points, _dummyLine);

            report.MinPoint.Should().Be(expectedMinPoint);
        }

        [Test]
        public void MinPoint_ShouldReturnZeroPointWhenNoPointsProvided()
        {
            var points = Enumerable.Empty<ScatterPoint>();
            var expectedMinPoint = new ScatterPoint(0, 0);

            var report = new ScatterReportData(points, _dummyLine);

            report.MinPoint.Should().Be(expectedMinPoint);
        }

        [Test]
        public void MaxPoint_ShouldReturnPointWithMinXAndYValues()
        {
            var points = new[]
            {
                new ScatterPoint(2.90, 1.0009),
                new ScatterPoint(2.9991, 0.009),
                new ScatterPoint(12.897, 0.009)
            };
            var expectedMinPoint = new ScatterPoint(12.897, 1.0009);

            var report = new ScatterReportData(points, _dummyLine);

            report.MaxPoint.Should().Be(expectedMinPoint);
        }

        [Test]
        public void MaxPoint_ShouldReturnZeroPointWhenNoPointsProvided()
        {
            var points = Enumerable.Empty<ScatterPoint>();
            var expectedMinPoint = new ScatterPoint(0, 0);

            var report = new ScatterReportData(points, _dummyLine);

            report.MaxPoint.Should().Be(expectedMinPoint);
        }
    }
}
