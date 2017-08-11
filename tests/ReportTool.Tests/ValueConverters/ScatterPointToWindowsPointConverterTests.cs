namespace ReportTool.Tests.ValueConverters
{
    using FluentAssertions;
    using NUnit.Framework;
    using ReportTool.Reports;
    using ReportTool.ValueConverters;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;

    [TestFixture]
    class ScatterPointToWindowsPointConverterTests
    {
        private ScatterPointToWindowsPointConverter _converter;

        [SetUp]
        public void SetUp()
        {
            _converter = new ScatterPointToWindowsPointConverter();
        }

        [Test]
        [TestCaseSource(nameof(GetTestCasesForConvertion))]
        public void ConvertShouldReturnExpectedPoint(object[] inputValues, Point expectedPoint)
        {
            var actualPoint = _converter.Convert(inputValues, typeof(Point), null, CultureInfo.CurrentCulture);

            actualPoint.ShouldBeEquivalentTo(
                expectedPoint, 
                config => config.Using<double>(context => context.Subject.Should().BeApproximately(context.Expectation, 0.1))
                                .WhenTypeIs<double>());
        }

        private static IEnumerable<TestCaseData> GetTestCasesForConvertion()
        {
            var originalPoint = new ScatterPoint(20, 10);
            var minDataPoint = new ScatterPoint(10, 5);
            var maxDataPoint = new ScatterPoint(50, 20);
            var elementActualWdth = 800.0;
            var elementActualHeight = 600.0;
            var expectedPoint = new Point(200, 200);
            yield return new TestCaseData(new object[] { originalPoint, minDataPoint, maxDataPoint, elementActualWdth, elementActualHeight }, expectedPoint);

            originalPoint = new ScatterPoint(1, 7);
            minDataPoint = new ScatterPoint(0, 0);
            maxDataPoint = new ScatterPoint(100, 10);
            elementActualWdth = 560;
            elementActualHeight = 210;
            expectedPoint = new Point(5.6, 147.0);
            yield return new TestCaseData(new object[] { originalPoint, minDataPoint, maxDataPoint, elementActualWdth, elementActualHeight }, expectedPoint);

            originalPoint = new ScatterPoint(0, 0);
            minDataPoint = new ScatterPoint(-20, -10);
            maxDataPoint = new ScatterPoint(100, 20);
            elementActualWdth = 400;
            elementActualHeight = 200;
            expectedPoint = new Point(66.66, 66.66);
            yield return new TestCaseData(new object[] { originalPoint, minDataPoint, maxDataPoint, elementActualWdth, elementActualHeight }, expectedPoint);

            originalPoint = new ScatterPoint(0, 0);
            minDataPoint = new ScatterPoint(0, 0);
            maxDataPoint = new ScatterPoint(0, 0);
            elementActualWdth = 400;
            elementActualHeight = 200;
            expectedPoint = new Point(0, 0);
            yield return new TestCaseData(new object[] { originalPoint, minDataPoint, maxDataPoint, elementActualWdth, elementActualHeight }, expectedPoint);
        }


        [Test]
        public void ConvertBack_ShouldThrowNotImplementedException()
        {
            Action convertBack = () => _converter.ConvertBack(null, Enumerable.Empty<Type>().ToArray(), null, CultureInfo.CurrentCulture);

            convertBack.ShouldThrow<NotImplementedException>();
        }
    }
}
