namespace ReportTool.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    [TestFixture]
    class JsonDataReaderTests
    {
        [Test]
        public void ShouldBeDecoratedWithExpectedAttribute()
        {
            typeof(JsonDataReader).Should().BeDecoratedWith<SupportedFileExtensionAttribute>(attribute => attribute.Extension.Equals(".json"));
        }

        [Test]
        public void Read_ShouldReturnSuccessfulResultWithExpectedData()
        {
            var directoryName = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            var fileName = @"DataReadersTestCases\JsonDataReaderTestCase1.json";
            var path = Path.Combine(directoryName, fileName);
            var expectedData = new[]
            {
                new Dictionary<string, double> {{ "weight", 150.05 }, { "speed", 220.15 }, { "tyre-consumption", 0.9845 } },
                new Dictionary<string, double> {{ "weight", 148.57 }, { "speed", 221.99 }, { "tyre-consumption", 0.9212 } },
                new Dictionary<string, double> {{ "weight", 146.98 }, { "speed", 225.05 }, { "tyre-consumption", 0.8601 } },
                new Dictionary<string, double> {{ "weight", 144.00 }, { "speed", 228.45 }, { "tyre-consumption", 0.7915 } },
                new Dictionary<string, double> {{ "weight", 143.05 }, { "speed", 120.98 }, { "tyre-consumption", 0.7801 } }
            };
            var reader = new JsonDataReader();

            var result = reader.Read(path);

            result.Data.ShouldBeEquivalentTo(expectedData);
        }
    }
}
