namespace ReportTool.Tests.ValueConverters
{
    using FluentAssertions;
    using NSubstitute;
    using NUnit.Framework;
    using ReportTool.ValueConverters;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;

    [TestFixture]
    class ScatterPointToCoordinateValueConverterTests
    {
        private static readonly Type[] _dummyTypes = Enumerable.Empty<Type>().ToArray();

        private IMultiValueConverter _fakeScatterPointToWindowsPointConverter;

        private ScatterPointToCoordinateValueConverter _converter;

        [SetUp]
        public void SetUp()
        {
            _fakeScatterPointToWindowsPointConverter = Substitute.For<IMultiValueConverter>();

            _converter = new ScatterPointToCoordinateValueConverter
            {
                ScatterPointToWindowsPointConverter = _fakeScatterPointToWindowsPointConverter
            };
        }

        [Test]
        [TestCaseSource(nameof(GetTestCasesForConvertion))]
        public void Convert_ShouldReturnExpectedValue(object[] inputValues, Point convertedPoint, CoordinateType outputType, double expectedValue)
        {
            _fakeScatterPointToWindowsPointConverter.Convert(null, null, null, null).ReturnsForAnyArgs(convertedPoint);
            _converter.Output = CoordinateType.X;

            var actualValue = (double)_converter.Convert(inputValues, typeof(double), null, CultureInfo.CurrentCulture);

            actualValue.Should().BeApproximately(convertedPoint.X, 0.01);
        }

        private static IEnumerable<TestCaseData> GetTestCasesForConvertion()
        {
            var originalPoint = new ScatterPoint(20, 10);
            var minDataPoint = new ScatterPoint(10, 5);
            var maxDataPoint = new ScatterPoint(50, 20);
            var elementActualWdth = 800.0;
            var elementActualHeight = 600.0;
            var convertedPoint = new Point(100, 90);
            var outputType = CoordinateType.X;
            var expectedValue = 100.0;
            yield return new TestCaseData(new object[] { originalPoint, minDataPoint, maxDataPoint, elementActualWdth, elementActualHeight }, convertedPoint, outputType, expectedValue);

            originalPoint = new ScatterPoint(1, 7);
            minDataPoint = new ScatterPoint(0, 0);
            maxDataPoint = new ScatterPoint(100, 10);
            elementActualWdth = 560;
            elementActualHeight = 210;
            convertedPoint = new Point(2.8, 73.5);
            outputType = CoordinateType.Y;
            expectedValue = 73.5;
            yield return new TestCaseData(new object[] { originalPoint, minDataPoint, maxDataPoint, elementActualWdth, elementActualHeight }, convertedPoint, outputType, expectedValue);


            originalPoint = new ScatterPoint(0, 0);
            minDataPoint = new ScatterPoint(-20, -10);
            maxDataPoint = new ScatterPoint(100, 20);
            elementActualWdth = 400;
            elementActualHeight = 200;
            convertedPoint = new Point(33.33, 36.66);
            outputType = CoordinateType.X;
            expectedValue = 33.33;
            yield return new TestCaseData(new object[] { originalPoint, minDataPoint, maxDataPoint, elementActualWdth, elementActualHeight }, convertedPoint, outputType, expectedValue);
        }

        [Test]
        public void Convert_ShouldPassCorrectValuesToTheScatterPointToPointConverterInCorrectOrder()
        {            
            var inputValues = new List<object>
            {
                new ScatterPoint(12.9, 0.347),
                new ScatterPoint(2.9, 0.1),
                new ScatterPoint(23.1, 1.1),
                300.0,
                200.0
            };
            object[] actualValues = null;
            _fakeScatterPointToWindowsPointConverter.Convert(Arg.Do<object[]>(values => actualValues = values), Arg.Any<Type>(), Arg.Any<object>(), Arg.Any<CultureInfo>()).Returns(new Point());
            _converter.Output = CoordinateType.X;

            _converter.Convert(inputValues.ToArray(), typeof(double), null, CultureInfo.CurrentCulture);

            actualValues.ShouldBeEquivalentTo(inputValues, config => config.WithStrictOrdering());
        }

        [Test]
        public void ConvertBack_ShouldThrowNotImplementedException()
        {
            Action convertBack = () => _converter.ConvertBack(null, _dummyTypes, null, CultureInfo.CurrentCulture);

            convertBack.ShouldThrow<NotImplementedException>();
        }
    }
}
