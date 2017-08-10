namespace ReportTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ScatterReportData
    {
        public readonly static ScatterReportData Empty = new ScatterReportData(Enumerable.Empty<ScatterPoint>(), ScatterLine.Zero);

        public IEnumerable<ScatterPoint> PlotPoints { get; }

        public ScatterLine TrendLine { get; }

        public ScatterPoint MinPoint { get; }

        public ScatterPoint MaxPoint { get; }

        public ScatterReportData(IEnumerable<ScatterPoint> plotPoints, ScatterLine trendLine)
        {
            PlotPoints = plotPoints;
            TrendLine = trendLine;
            MinPoint = new ScatterPoint(PlotPoints.Select(point => point.X).DefaultIfEmpty(0).Min(), PlotPoints.Select(plot => plot.Y).DefaultIfEmpty(0).Min());
            MaxPoint = new ScatterPoint(PlotPoints.Select(plot => plot.X).DefaultIfEmpty(0).Max(), PlotPoints.Select(plot => plot.Y).DefaultIfEmpty(0).Max());
        }
    }
}
