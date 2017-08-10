namespace ReportTool
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class DataReaderProvider : IDataReaderProvider
    {
        private Dictionary<string, IDataReader> _readers;

        public DataReaderProvider(IEnumerable<IDataReader> readers)
        {
            _readers = readers.SelectMany(GetSupportedExtensions).ToDictionary(info => info.Extension, info => info.Reader);
        }

        public IDataReader GetByExtension(string fileExtension)
        {
            if (_readers.TryGetValue(fileExtension, out IDataReader reader))
            {
                return reader;
            }

            return new NotSupportedDataReader();
        }

        private IEnumerable<(string Extension, IDataReader Reader)> GetSupportedExtensions(IDataReader reader)
        {
            MemberInfo info = reader.GetType();
            return info.GetCustomAttributes<SupportedFileExtensionAttribute>().Select(attribute => (attribute.Extension, reader));
        }
    }
}
