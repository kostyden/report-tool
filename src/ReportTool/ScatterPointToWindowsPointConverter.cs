namespace ReportTool
{
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
            var xCoefficent = xRange == 0 ? 0 : actualWidth / (maxPoint.X - minPoint.X);
            var yCoefficent = yRange == 0 ? 0 : actualHeight / (maxPoint.Y - minPoint.Y);

            var scaledX = (plotPoint.X - minPoint.X) * xCoefficent;
            var scaledY = (plotPoint.Y - minPoint.Y) * yCoefficent;

            return new Point(scaledX, scaledY);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
