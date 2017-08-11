namespace ReportTool.Extensions
{
    using System.Collections.Generic;

    internal static class IEnumerableExtensions
    {
        public static HashSet<T> ToSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }
    }
}
