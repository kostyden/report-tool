﻿namespace ReportTool
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    [SupportedFileExtension(".json")]
    public class JsonDataReader : IDataReader
    {
        public DataResult Read(string path)
        {
            var rawData = File.ReadAllText(path);
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, double>>>(rawData);
            return DataResult.CreateSuccessful(data);
        }
    }
}
