namespace ReportTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ScatterReportData
    {
        public IEnumerable<ScatterPoint> PlotPoints { get; }

        public IEnumerable<ScatterPoint> TrendLinePoints { get; }

        public ScatterPoint MinPoint
        {
            get
            {
                return new ScatterPoint(PlotPoints.Select(point => point.X).DefaultIfEmpty(0).Min(), PlotPoints.Select(plot => plot.Y).DefaultIfEmpty(0).Min());
            }
        }

        public ScatterPoint MaxPoint
        {
            get
            {
                return new ScatterPoint(PlotPoints.Select(plot => plot.X).DefaultIfEmpty(0).Max(), PlotPoints.Select(plot => plot.Y).DefaultIfEmpty(0).Max());
            }
        }

        public ScatterReportData(IEnumerable<ScatterPoint> plotPoints, IEnumerable<ScatterPoint> trendLinePoints)
        {
            PlotPoints = plotPoints;
            TrendLinePoints = trendLinePoints;
        }
    }
}
