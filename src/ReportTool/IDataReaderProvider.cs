namespace ReportTool
{
    using System;

    public interface IDataReaderProvider
    {
        IDataReader GetByExtension(string fileExtension);
    }
}
