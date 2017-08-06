namespace ReportTool
{
    using System;
    using System.Collections.Generic;

    public class ScatterInputData
    {
        public IEnumerable<Dictionary<string, double>> Data { get; set; }

        public string ColumnNameForX { get; set; }

        public string ColumnNameForY { get; set; }
    }
}
