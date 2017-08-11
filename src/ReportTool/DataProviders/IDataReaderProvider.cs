namespace ReportTool.DataProviders
{
    public interface IDataReaderProvider
    {
        IDataReader GetByExtension(string fileExtension);
    }
}
