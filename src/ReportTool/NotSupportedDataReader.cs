namespace ReportTool
{
    using System;

    public class NotSupportedDataReader : IDataReader
    {
        public DataResult Read(string path)
        {
            throw new NotImplementedException();
        }
    }
}
