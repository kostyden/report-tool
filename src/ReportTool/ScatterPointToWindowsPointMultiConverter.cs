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

            var xCoefficent = actualWidth / (maxPoint.X - minPoint.X) / 2;
            var yCoefficent = actualHeight / (maxPoint.Y - minPoint.Y) / 2;

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
