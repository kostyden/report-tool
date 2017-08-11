namespace ReportTool.DataProviders.FileDataProviders.DataReaders
{
    using ReportTool.DataProviders;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    [SupportedFileExtension(".csv")]
    public class CsvDataReader : FileDataReader
    {
        private readonly char _delimeter;

        public CsvDataReader(char columnDelimeter)
        {
            _delimeter = columnDelimeter;
        }

        protected override DataResult ReadImpl(string path)
        {
            var data = new List<Dictionary<string, double>>();
            using (var reader = new StreamReader(path))
            {
                List<string> columnNames = null;
                while (reader.EndOfStream == false)
                {
                    if (columnNames == null)
                    {
                        columnNames = ReadValues(reader).ToList();
                    }

                    var values = ReadLineData(reader, columnNames);
                    data.Add(values);
                }
            }

            return DataResult.CreateSuccessful(data);
        }

        private Dictionary<string, double> ReadLineData(StreamReader reader, List<string> columnNames)
        {
            return ReadValues(reader).Select(value => double.Parse(value))
                                     .Select((value, index) => new { Value = value, Index = index })
                                     .ToDictionary(info => columnNames[info.Index], info => info.Value);
        }

        private IEnumerable<string> ReadValues(StreamReader reader)
        {
            return reader.ReadLine().Split(_delimeter);
        }
    }
}
