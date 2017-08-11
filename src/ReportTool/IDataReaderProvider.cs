namespace ReportTool
{
    public interface IDataReaderProvider
    {
        IDataReader GetByExtension(string fileExtension);
    }
}
