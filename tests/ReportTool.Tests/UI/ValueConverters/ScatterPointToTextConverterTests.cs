namespace ReportTool.Tests.UI.ValueConverters
{
    using FluentAssertions;
    using NUnit.Framework;
    using ReportTool.Reports;
    using ReportTool.UI.ValueConverters;
    using System;
    using System.Globalization;

    [TestFixture]
    class ScatterPointToTextConverterTests
    {
        private ScatterPointToTextConverter _converter;

        [SetUp]
        public void SetUp()
        {
            _converter = new ScatterPointToTextConverter();
        }

        [Test]
        [TestCase(2.3, 120, "(2.3, 120)")]
        [TestCase(-10, 0.0021598, "(-10, 0.0021598)")]
        public void Convert_ShouldReturnExpectedText(double pointX, double pointY, string expectedText)
        {
            var point = new ScatterPoint(pointX, pointY);

            var actualText = _converter.Convert(point, typeof(string), null, CultureInfo.CurrentCulture);

            actualText.Should().Be(expectedText);
        }

        [Test]
        public void ConvertBack_ShouldThrowNotImplementedException()
        {
            Action convertBack = () => _converter.ConvertBack(null, typeof(ScatterPoint), null, CultureInfo.CurrentCulture);

            convertBack.ShouldThrow<NotImplementedException>();
        }
    }
}
