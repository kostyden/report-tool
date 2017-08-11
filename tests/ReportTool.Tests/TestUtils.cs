namespace ReportTool.Tests
{
    using FluentAssertions;
    using FluentAssertions.Equivalency;
    using ReportTool.Reports;
    using System;
    using System.IO;
    using System.Reflection;

    public static class TestUtils
    {
        public const double APPROXIMATION_PRECISION = 0.01;

        public static string BuildPathFor(string fileName)
        {
            var directoryName = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            return Path.Combine(directoryName, fileName);
        }


        public static EquivalencyAssertionOptions<T> DoubleShouldBeEqualApproximately<T>(EquivalencyAssertionOptions<T> config)
        {
            return config.Using<double>(context => context.Subject.Should().BeApproximately(context.Expectation, APPROXIMATION_PRECISION))
                         .WhenTypeIs<double>();
        }
    }
}
