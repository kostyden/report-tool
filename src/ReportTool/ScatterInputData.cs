namespace ReportTool
{
    using System;
    using System.Collections.Generic;

    public class ScatterInputData
    {
        public IEnumerable<Dictionary<string, double>> Data { get; set; }

        public string AbscissaColumnName { get; set; }

        public string OrdinateColumnName { get; set; }
    }
}
