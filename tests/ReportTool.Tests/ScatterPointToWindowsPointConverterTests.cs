namespace ReportTool.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Windows;

    [TestFixture]
    class ScatterPointToWindowsPointConverterTests
    {
        [Test]
        public void Convert_ShouldReturnPointType()
        {
            var scatterPoint = new ScatterPoint(12.4, 0.23);
            var converter = new ScatterPointToWindowsPointConverter();

            var actualPoint = converter.Convert(scatterPoint, typeof(Point), null, System.Globalization.CultureInfo.CurrentCulture);

            actualPoint.Should().BeOfType<Point>();
        }

        [Test]
        [TestCase(12.9, 3.8, 12.9, 3.8)]
        public void Convert_ShouldReturnPointType(double x, double y, double expectedX, double expectedY)
        {
            var scatterPoint = new ScatterPoint(x, y);
            var converter = new ScatterPointToWindowsPointConverter();
            var expectedPoint = new Point(expectedX, expectedY);

            var actualPoint = converter.Convert(scatterPoint, typeof(Point), null, System.Globalization.CultureInfo.CurrentCulture);

            actualPoint.Should().Be(expectedPoint);
        }
    }
}
