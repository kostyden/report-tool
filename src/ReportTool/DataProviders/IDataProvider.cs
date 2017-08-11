namespace ReportTool.DataProviders
{
    using System.Collections.Generic;

    public interface IDataProvider
    {
        DataResult GetFrom(string path);
    }
}
