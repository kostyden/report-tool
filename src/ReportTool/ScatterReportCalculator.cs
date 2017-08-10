namespace ReportTool
{
    using System.Collections.Generic;
    using System.Linq;

    public class ScatterReportCalculator : IScatterReportCalculator
    {
        public ScatterReportData Calculate(ScatterInputData input)
        {
            var plotPoints = input.Data.Select(row => new ScatterPoint(row[input.AbscissaColumnName], row[input.OrdinateColumnName])).ToList();
            var trendLine = CalculateTrendLine(plotPoints);

            return new ScatterReportData(plotPoints, trendLine);
        }

        private ScatterLine CalculateTrendLine(List<ScatterPoint> points)
        {
            if (points.Any() == false)
            {
                return ScatterLine.Zero;
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

            var minXValue = points.Min(point => point.X);
            var maxXValue = points.Max(point => point.X);

            var start = new ScatterPoint(minXValue, slope * minXValue + offset);
            var end = new ScatterPoint(maxXValue, slope * maxXValue + offset);
            return new ScatterLine(start, end);
        }
    }
}
