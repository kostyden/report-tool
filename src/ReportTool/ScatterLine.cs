namespace ReportTool
{
    using System;

    public class ScatterLine
    {
        public readonly static ScatterLine Zero = new ScatterLine(new ScatterPoint(0, 0), new ScatterPoint(0, 0));

        public ScatterPoint Start { get; }

        public ScatterPoint End { get; }

        public ScatterLine(ScatterPoint start, ScatterPoint end)
        {
            Start = start;
            End = end;
        }
    }
}
