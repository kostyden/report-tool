namespace ReportTool.DataProviders.FileDataProviders.DataReaders
{
    using System;

    public abstract class FileDataReader : IDataReader
    {
        protected abstract DataResult ReadImpl(string path);

        public DataResult Read(string path)
        {
            try
            {
                return ReadImpl(path);
            }
            catch (Exception exception)
            {
                return DataResult.CreateFailed(exception.Message);
            }
        }
    }
}
