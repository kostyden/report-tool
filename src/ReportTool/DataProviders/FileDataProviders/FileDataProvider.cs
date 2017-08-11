namespace ReportTool.DataProviders.FileDataProviders
{
    using ReportTool.DataProviders;
    using System.IO;

    public class FileDataProvider : IDataProvider
    {
        private readonly IDataReaderProvider _readerProvider;

        public FileDataProvider(IDataReaderProvider readerProvider)
        {
            _readerProvider = readerProvider;
        }

        public DataResult GetFrom(string path)
        {
            var extension = Path.GetExtension(path);
            var reader = _readerProvider.GetByExtension(extension);
            return reader.Read(path);
        }
    }
}
