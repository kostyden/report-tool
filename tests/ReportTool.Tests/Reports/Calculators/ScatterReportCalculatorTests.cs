namespace ReportTool.Tests.Reports.Calculators
{
    using FluentAssertions;
    using FluentAssertions.Equivalency;
    using NUnit.Framework;
    using ReportTool.Reports;
    using ReportTool.Reports.Calculators;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    class ScatterReportCalculatorTests
    {
        private const double APPROXIMATION_PRECISION = 0.01;

        private ScatterReportCalculator _calculator;

        [SetUp]
        public void SetUp()
        {
            _calculator = new ScatterReportCalculator();
        }

        [Test]
        public void Calculate_ShouldReturnEmptyReportWhenGivenDataIsEmpty()
        {
            var inputData = new ScatterInputData
            {
                Data = Enumerable.Empty<Dictionary<string, double>>(),
                AbscissaColumnName = "first",
                OrdinateColumnName = "second"
            };
            var expectedReport = new ScatterReportData(Enumerable.Empty<ScatterPoint>(), new ScatterLine(new ScatterPoint(), new ScatterPoint()));

            var actualReport = _calculator.Calculate(inputData);

            actualReport.ShouldBeEquivalentTo(expectedReport);
        }

        [Test]
        [TestCaseSource(nameof(GetTestCasesForPlotPointsCalculation))]
        public void Calculate_ShouldReturnExpectedPlotPoints(ScatterInputData inputData, IEnumerable<ScatterPoint> expectedPlotPoints)
        {

            var actualReport = _calculator.Calculate(inputData);

            actualReport.PlotPoints.ShouldBeEquivalentTo(expectedPlotPoints);
        }

        private static IEnumerable<TestCaseData> GetTestCasesForPlotPointsCalculation()
        {
            var data = new[]
            {
                new Dictionary<string, double> {{ "first", 12 }, { "second", 1.2 }, { "third", 22.2 }, { "fourth", 9.012 } },
                new Dictionary<string, double> {{ "first", 4 }, { "second", 0.2 }, { "third", 33.879401 }, { "fourth", 10 } },
                new Dictionary<string, double> {{ "first", 1.3 }, { "second", 0.598 }, { "third", 28.59 }, { "fourth", 12 } },
                new Dictionary<string, double> {{ "first", 3.2 }, { "second", 1.4 }, { "third", 29.2 }, { "fourth", 19.8 } },
                new Dictionary<string, double> {{ "first", 9.3 }, { "second", 1.221 }, { "third", 28 }, { "fourth", 11.05 } }
            };
            var expectedPlotPoints = new[]
            {
                new ScatterPoint(12, 22.2),
                new ScatterPoint(4, 33.879401),
                new ScatterPoint(1.3, 28.59),
                new ScatterPoint(3.2, 29.2),
                new ScatterPoint(9.3, 28)
            };
            yield return new TestCaseData(new ScatterInputData { Data = data, AbscissaColumnName = "first", OrdinateColumnName = "third" }, expectedPlotPoints);

            expectedPlotPoints = new[]
            {
                new ScatterPoint(1.2, 9.012),
                new ScatterPoint(0.2, 10),
                new ScatterPoint(0.598, 12),
                new ScatterPoint(1.4, 19.8),
                new ScatterPoint(1.221, 11.05)
            };
            yield return new TestCaseData(new ScatterInputData { Data = data, AbscissaColumnName = "second", OrdinateColumnName = "fourth" }, expectedPlotPoints);
        }

        [Test]
        public void Calculate_ShouldReturnExpectedTrendLinePointsWithPositiveCorrelation()
        {
            var data = new[]
            {
                new Dictionary<string, double> {{ "first", 10 }, { "second", 1.2 }, { "third", 22.2 }, { "fourth", 10 } },
                new Dictionary<string, double> {{ "first", 15 }, { "second", 0.2 }, { "third", 33.879401 }, { "fourth", 20 } },
                new Dictionary<string, double> {{ "first", 20 }, { "second", 0.598 }, { "third", 28.59 }, { "fourth", 30 } },
                new Dictionary<string, double> {{ "first", 35 }, { "second", 1.4 }, { "third", 29.2 }, { "fourth", 40 } },
                new Dictionary<string, double> {{ "first", 40 }, { "second", 1.221 }, { "third", 28 }, { "fourth", 50 } }
            };
            var expectedTrendLine = new ScatterLine(new ScatterPoint(10, 13.2833), new ScatterPoint(40, 49.1033));
            var inputData = new ScatterInputData { Data = data, AbscissaColumnName = "first", OrdinateColumnName = "fourth" };

            AssertThatTrendLineCalculatedCorrectly(inputData, expectedTrendLine);
        }

        [Test]
        public void Calculate_ShouldReturnExpectedTrendLinePointsWithNegativeCorrelation()
        {
            var data = new[]
            {
                new Dictionary<string, double> {{ "first", 10 }, { "second", 50 }},
                new Dictionary<string, double> {{ "first", 20 }, { "second", 40 }},
                new Dictionary<string, double> {{ "first", 30 }, { "second", 30 }},
                new Dictionary<string, double> {{ "first", 40 }, { "second", 20 }},
                new Dictionary<string, double> {{ "first", 50 }, { "second", 10 }}
            };
            var expectedTrendLine = new ScatterLine(new ScatterPoint(10, 50), new ScatterPoint(50, 10));
            var inputData = new ScatterInputData { Data = data, AbscissaColumnName = "first", OrdinateColumnName = "second" };

            AssertThatTrendLineCalculatedCorrectly(inputData, expectedTrendLine);
        }

        [Test]
        public void Calculate_ShouldReturnExpectedTrendLinePointsWithEvenCorrelation()
        {
            var data = new[]
            {
                new Dictionary<string, double> {{ "first", 61 }, { "second", 1.40714182621643 } },
                new Dictionary<string, double> {{ "first", 75 }, { "second", 1.67869994337439 } },
                new Dictionary<string, double> {{ "first", 26 }, { "second", 1.86521975662936 } },
                new Dictionary<string, double> {{ "first", 65 }, { "second", 1.96665785281196 } },
                new Dictionary<string, double> {{ "first", 28 }, { "second", 1.48216295762807 } }
            };
            var expectedTrendLine = new ScatterLine(new ScatterPoint(26, 1.6660), new ScatterPoint(75, 1.6954));
            var inputData = new ScatterInputData { Data = data, AbscissaColumnName = "first", OrdinateColumnName = "second" };

            AssertThatTrendLineCalculatedCorrectly(inputData, expectedTrendLine);
        }

        private void AssertThatTrendLineCalculatedCorrectly(ScatterInputData inputData, ScatterLine expectedLine)
        {
            var actualReport = _calculator.Calculate(inputData);

            actualReport.TrendLine.ShouldBeEquivalentTo(
                expectedLine, 
                config => PointShouldBeEqualApproximately(config));
        }

        private EquivalencyAssertionOptions<ScatterLine> PointShouldBeEqualApproximately(EquivalencyAssertionOptions<ScatterLine> config)
        {
            return config.Using<double>(context => context.Subject.Should().BeApproximately(context.Expectation, APPROXIMATION_PRECISION))
                         .WhenTypeIs<double>();
        }
    }
}
