﻿namespace ReportTool.DataProviders
{
    using System.Collections.Generic;
    using System.Linq;

    public class DataResult
    {
        public IEnumerable<Dictionary<string, double>> Data { get; }

        public string ErrorMessage { get; }

        public static DataResult CreateSuccessful(IEnumerable<Dictionary<string, double>> data)
        {
            return new DataResult(data, null);
        }

        public static DataResult CreateFailed(string message)
        {
            var data = Enumerable.Empty<Dictionary<string, double>>();
            return new DataResult(data, message);
        }

        private DataResult(IEnumerable<Dictionary<string, double>> data, string errorMessage)
        {
            Data = data;
            ErrorMessage = errorMessage;
        }
    }
}
