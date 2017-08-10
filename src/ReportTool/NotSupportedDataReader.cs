namespace ReportTool
{
    using System;

    public class NotSupportedDataReader : IDataReader
    {
        public DataResult Read(string path)
        {
            return DataResult.CreateFailed($"Reader not found for file {path}");
        }
    }
}
