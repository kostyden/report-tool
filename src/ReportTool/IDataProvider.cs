namespace ReportTool
{
    using System.Collections.Generic;

    public interface IDataProvider
    {
        DataResult GetFrom(string filePath);
    }
}
