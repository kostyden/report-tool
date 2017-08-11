namespace ReportTool.UI.ValueConverters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class ScatterPointToCoordinateValueConverter : IMultiValueConverter
    {
        public IMultiValueConverter ScatterPointToWindowsPointConverter { get; set; }

        public CoordinateType Output { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var point = (Point)ScatterPointToWindowsPointConverter.Convert(values, targetType, parameter, culture);

            if (Output == CoordinateType.X)
            {
                return point.X;
            }
            else
            {
                return point.Y;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public enum CoordinateType
    {
        X = 0,
        Y = 1
    }
}
