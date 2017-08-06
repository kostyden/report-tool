namespace ReportTool
{
    using System;

    public interface IScatterReportCalculator
    {
        ScatterReportData Calculate(ScatterInputData data);
    }
}
