namespace ReportTool
{
    using System.Collections.Generic;

    internal static class Extensions
    {
        public static HashSet<T> ToSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }
    }
}
