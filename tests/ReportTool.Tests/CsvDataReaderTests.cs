namespace ReportTool.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    [TestFixture]
    class CsvDataReaderTests
    {
        [Test]
        public void ShouldBeDecoratedWithExpectedAttribute()
        {
            typeof(CsvDataReader).Should().BeDecoratedWith<SupportedFileExtensionAttribute>(attribute => attribute.Extension.Equals(".csv"));
        }

        [Test]
        [TestCase(@"DataReadersTestCases\CsvDataReaderTestCase1TabDelimited.csv", '\t')]
        [TestCase(@"DataReadersTestCases\CsvDataReaderTestCase1CommaDelimited.csv", ',')]
        public void Read_ShouldReturnSuccessfulResultWithExpectedData(string fileName, char delimeter)
        {
            var directoryName = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            var path = Path.Combine(directoryName, fileName);
            var expectedData = new[]
            {
                new Dictionary<string, double> {{ "one", 0.4 }, { "two", 11.03 }, { "three", 13.333 } },
                new Dictionary<string, double> {{ "one", 0.5 }, { "two", 12.05 }, { "three", 22.33 } },
                new Dictionary<string, double> {{ "one", 0.6 }, { "two", 0.06 }, { "three", -40.23 } }
            };
            var reader = new CsvDataReader(delimeter);

            var result = reader.Read(path);

            result.Data.ShouldBeEquivalentTo(expectedData);
        }

        [Test]
        public void Read_ShouldReturnFailedResultWithExpectedMessageWhenFileNotFound()
        {
            var directoryName = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            var fileName = @"DataReadersTestCases\NotExistedFile.csv";
            var path = Path.Combine(directoryName, fileName);

            var reader = new CsvDataReader(';');

            var result = reader.Read(path);

            result.Data.Should().BeEmpty();
            result.ErrorMessage.Should().ContainEquivalentOf("Could not find file");
        }

        [Test]
        public void Read_ShouldReturnFailedResultWithExpectedMessageWhenValuesInWrongFormat()
        {
            var directoryName = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            var fileName = @"DataReadersTestCases\CsvDataReaderTestCase2CommaDelimited.csv";
            var path = Path.Combine(directoryName, fileName);

            var reader = new CsvDataReader(',');

            var result = reader.Read(path);

            result.Data.Should().BeEmpty();
            result.ErrorMessage.Should().ContainEquivalentOf("string was not in a correct format");
        }
    }
}
