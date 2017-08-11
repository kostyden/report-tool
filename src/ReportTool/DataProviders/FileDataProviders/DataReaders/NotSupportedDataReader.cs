namespace ReportTool.DataProviders.FileDataProviders.DataReaders
{
    using ReportTool.DataProviders;

    public class NotSupportedDataReader : IDataReader
    {
        public DataResult Read(string path)
        {
            return DataResult.CreateFailed($"Reader not found for file {path}");
        }
    }
}
