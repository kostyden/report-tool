namespace ReportTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ScatterReportCalculator : IScatterReportCalculator
    {
        public ScatterReportData Calculate(ScatterInputData input)
        {
            var plotPoints = input.Data.Select(row => new ScatterPoint(row[input.AbscissaColumnName], row[input.OrdinateColumnName])).ToList();
            var trendLinePoints = CalculateTrendLine(plotPoints).ToList();

            return new ScatterReportData(plotPoints, trendLinePoints);
        }

        private IEnumerable<ScatterPoint> CalculateTrendLine(List<ScatterPoint> points)
        {
            if (points.Any() == false)
            {
                return Enumerable.Empty<ScatterPoint>();
            }

            var pointsAmount = points.Count();
            var sumOfXYProduct = points.Sum(point => point.X * point.Y);
            var sumOfX = points.Sum(point => point.X);
            var sumOfY = points.Sum(point => point.Y);
            var sumOfXSquare = points.Sum(point => point.X * point.X);
            var squareOfsumOfX = points.Sum(point => point.X) * points.Sum(point => point.X);

            var slopeDividend = pointsAmount * sumOfXYProduct - sumOfX * sumOfY;
            var slopeDivisor = pointsAmount * sumOfXSquare - squareOfsumOfX;

            var slope = slopeDividend / slopeDivisor;

            var offsetDividend = sumOfY - slope * sumOfX;
            var offset = offsetDividend / pointsAmount;

            return points.Select(point => new ScatterPoint(point.X, slope * point.X + offset));
        }
    }
}
