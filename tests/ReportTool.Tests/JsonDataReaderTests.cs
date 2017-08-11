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
                new Dictionary<string, double> {{ "weight", 142.00 }, { "speed", 229.98 }, { "tyre-consumption", 0.7000 } }
            };
            var reader = new JsonDataReader();

            var result = reader.Read(path);

            result.Data.ShouldBeEquivalentTo(expectedData);
        }

        [Test]
        public void Read_ShouldReturnFailedResultWithExpectedMessageWhenFileNotFound()
        {
            var directoryName = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            var fileName = @"DataReadersTestCases\NotExistedFile.json";
            var path = Path.Combine(directoryName, fileName);

            var reader = new JsonDataReader();

            var result = reader.Read(path);

            result.Data.Should().BeEmpty();
            result.ErrorMessage.Should().ContainEquivalentOf("Could not find file");
        }

        [Test]
        public void Read_ShouldReturnFailedResultWithExpectedMessageWhenValuesInWrongFormat()
        {
            var directoryName = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            var fileName = @"DataReadersTestCases\JsonDataReaderTestCase2.json";
            var path = Path.Combine(directoryName, fileName);

            var reader = new JsonDataReader();

            var result = reader.Read(path);

            result.Data.Should().BeEmpty();
            result.ErrorMessage.Should().ContainEquivalentOf("could not convert string to double");
        }
    }
}
