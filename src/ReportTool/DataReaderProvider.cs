namespace ReportTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class DataReaderProvider : IDataReaderProvider
    {
        private List<IDataReader> _readers;

        public DataReaderProvider(IEnumerable<IDataReader> readers)
        {
            _readers = readers.ToList();
        }

        public IDataReader GetByExtension(string fileExtension)
        {
            return _readers.Where(reader => ((MemberInfo)reader.GetType())
                                                               .GetCustomAttributes<SupportedFileExtensionAttribute>()
                                                               .Any(attr => attr.Extension.Equals(fileExtension)))
                           .DefaultIfEmpty(new NotSupportedDataReader())
                           .First();
        }
    }
}
