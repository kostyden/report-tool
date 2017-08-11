namespace ReportTool.Reports
{
    using ReportTool.Reports;
    using System;

    public interface IScatterReportCalculator
    {
        ScatterReportData Calculate(ScatterInputData data);
    }
}
