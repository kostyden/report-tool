namespace ReportTool.Reports.Calculators
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

            var (slope, offset) = CalculateSlopeAndOffsetFrom(points);            
            return CreateLineFrom(points, slope, offset);
        }

        private (double slope, double offset) CalculateSlopeAndOffsetFrom(IEnumerable<ScatterPoint> points)
        {
            var pointsAmount = points.Count();
            var sumOfX = points.Sum(point => point.X);
            var sumOfY = points.Sum(point => point.Y);
            var sumOfXYProduct = points.Sum(point => point.X * point.Y);
            var sumOfXSquare = points.Sum(point => point.X * point.X);
            var squareOfsumOfX = sumOfX * sumOfX;

            var slopeDividend = pointsAmount * sumOfXYProduct - sumOfX * sumOfY;
            var slopeDivisor = pointsAmount * sumOfXSquare - squareOfsumOfX;

            var slope = slopeDividend / slopeDivisor;

            var offsetDividend = sumOfY - slope * sumOfX;
            var offset = offsetDividend / pointsAmount;

            return (slope, offset);
        }
        
        private ScatterLine CreateLineFrom(IEnumerable<ScatterPoint> points, double slope, double offset)
        {
            var minXValue = points.Min(point => point.X);
            var maxXValue = points.Max(point => point.X);

            var start = CalculateTrendLinePointFor(minXValue, slope, offset);
            var end = CalculateTrendLinePointFor(maxXValue, slope, offset);

            return new ScatterLine(start, end);
        }

        private ScatterPoint CalculateTrendLinePointFor(double xValue, double slope, double offset)
        {
            return new ScatterPoint(xValue, slope * xValue + offset);
        }
    }
}
