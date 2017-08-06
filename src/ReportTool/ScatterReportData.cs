namespace ReportTool
{
    using System;
    using System.Collections.Generic;

    public class ScatterReportData
    {
        public IEnumerable<ScatterPoint> PlotPoints { get; }

        public IEnumerable<ScatterPoint> TrendLinePoints { get; }

        public ScatterReportData(IEnumerable<ScatterPoint> plotPoints, IEnumerable<ScatterPoint> trendLinePoints)
        {
            PlotPoints = plotPoints;
            TrendLinePoints = trendLinePoints;
        }
    }
}
