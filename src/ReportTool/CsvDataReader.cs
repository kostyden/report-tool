namespace ReportTool
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    [SupportedFileExtension(".csv")]
    public class CsvDataReader : IDataReader
    {
        private readonly char _delimeter;

        public CsvDataReader(char columnDelimeter)
        {
            _delimeter = columnDelimeter;
        }

        public DataResult Read(string path)
        {
            var data = new List<Dictionary<string, double>>();
            using (var reader = new StreamReader(path))
            {
                List<string> columnsNames = null;
                while(reader.EndOfStream == false)
                {
                    if (columnsNames == null)
                    {
                        columnsNames = reader.ReadLine().Split(_delimeter).ToList();
                        
                    }
                    else
                    {
                        var values = Array.ConvertAll(reader.ReadLine().Split(_delimeter), value => double.Parse(value))
                                          .Select((value, index) => new { Value = value, Index = index })
                                          .ToDictionary(info => columnsNames[info.Index], info => info.Value);
                        data.Add(values);
                    }
                }
            }

            return DataResult.CreateSuccessful(data);
        }
    }
}
