namespace ReportTool.UI.ValueConverters
{
    using ReportTool.Reports;
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class ScatterPointToWindowsPointConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var plotPoint = (ScatterPoint)values[0];
            var minPoint = (ScatterPoint)values[1];
            var maxPoint = (ScatterPoint)values[2];
            var actualWidth = (double)values[3];
            var actualHeight = (double)values[4];

            var xRange = (maxPoint.X - minPoint.X);
            var yRange = (maxPoint.Y - minPoint.Y);
            var xCoefficent = CalculateCoefficentFor(actualWidth, xRange);
            var yCoefficent = CalculateCoefficentFor(actualHeight, yRange);

            var convertedX = (plotPoint.X - minPoint.X) * xCoefficent;
            var convertedY = (plotPoint.Y - minPoint.Y) * yCoefficent;

            return new Point(convertedX, convertedY);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private double CalculateCoefficentFor(double actualRange, double givenRange)
        {
            if (givenRange == 0)
            {
                return 0;
            }

            return actualRange / givenRange;
        }
    }
}
