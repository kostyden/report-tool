namespace ReportTool
{
    using System.Collections.Generic;

    public interface IDataReader
    {
        DataResult Read(string path);
    }
}
