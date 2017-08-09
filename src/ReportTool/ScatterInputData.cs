namespace ReportTool
{
    using System;
    using System.Collections.Generic;

    public class ScatterInputData
    {
        public IEnumerable<Dictionary<string, double>> Data { get; set; }

        public string AbscissaColumnName { get; set; }

        internal object Select(Func<object, ScatterPoint> p)
        {
            throw new NotImplementedException();
        }

        public string OrdinateColumnName { get; set; }
    }
}
