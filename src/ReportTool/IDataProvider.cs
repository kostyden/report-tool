namespace ReportTool
{
    using System.Collections.Generic;

    public interface IDataProvider
    {
        IEnumerable<Dictionary<string, double>> GetFrom(string filePath);
    }
}
