namespace ReportTool.Reports
{
    using System;

    public struct ScatterPoint
    {
        public double X { get; }

        public double Y { get; }

        public ScatterPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
