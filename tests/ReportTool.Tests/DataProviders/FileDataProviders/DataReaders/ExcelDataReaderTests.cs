namespace ReportTool.Tests.DataProviders.FileDataProviders.DataReaders
{
    using FluentAssertions;
    using NUnit.Framework;
    using ReportTool.DataProviders;
    using ReportTool.DataProviders.FileDataProviders.DataReaders;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    [TestFixture]
    class ExcelDataReaderTests
    {
        [Test]
        public void ShouldBeDecoratedWithExpectedAttribute()
        {
            typeof(ExcelDataReader).Should().BeDecoratedWith<SupportedFileExtensionAttribute>(attribute => attribute.Extension.Equals(".xlsx"));
        }

        [Test]
        public void Read_ShouldReturnSuccessfulResultWithExpectedData()
        {
            var directoryName = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            var fileName = @"DataReadersTestCases\ExcelDataReaderTestCase1.xlsx";
            var path = Path.Combine(directoryName, fileName);
            var data = new[]
            {
                new Dictionary<string, double> {{ "one", 1.1 }, { "two", 1.2 }, { "three", 1.3 } },
                new Dictionary<string, double> {{ "one", 2.1 }, { "two", 2.2 }, { "three", 2.3 } },
                new Dictionary<string, double> {{ "one", 3.1 }, { "two", 3.2 }, { "three", 3.3 } }
            };
            var expectedResult = DataResult.CreateSuccessful(data);
            var reader = new ExcelDataReader();

            var result = reader.Read(path);

            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Test]
        public void Read_ShouldReturnSuccessfulResultWithExpectedDataWhenFileInProtectedView()
        {
            var directoryName = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            var fileName = @"DataReadersTestCases\ExcelDataReaderTestCase3ProtectedView.xlsx";
            var path = Path.Combine(directoryName, fileName);
            var data = new[]
            {
                new Dictionary<string, double> {{ "one", 30.24 }, { "two", 256 }, { "three", 0.4099 }},
                new Dictionary<string, double> {{ "one", 30.84 }, { "two", 338 }, { "three", 0.0312 }},
                new Dictionary<string, double> {{ "one", 16.92 }, { "two", 357 }, { "three", 0.4993 }}
            };
            var expectedResult = DataResult.CreateSuccessful(data);
            var reader = new ExcelDataReader();

            var result = reader.Read(path);

            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Test]
        public void Read_ShouldReturnFailedResultWithExpectedMessageWhenFileNotFound()
        {
            var directoryName = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            var fileName = @"DataReadersTestCases\NotExistedFile.xlsx";
            var path = Path.Combine(directoryName, fileName);

            var reader = new ExcelDataReader();

            var result = reader.Read(path);

            result.Data.Should().BeEmpty();
            result.ErrorMessage.Should().ContainEquivalentOf("Could not find file");
        }

        [Test]
        public void Read_ShouldReturnFailedResultWithExpectedMessageWhenValuesInWrongFormat()
        {
            var directoryName = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            var fileName = @"DataReadersTestCases\ExcelDataReaderTestCase2.xlsx";
            var path = Path.Combine(directoryName, fileName);

            var reader = new ExcelDataReader();

            var result = reader.Read(path);

            result.Data.Should().BeEmpty();
            result.ErrorMessage.Should().ContainEquivalentOf("string was not in a correct format");
        }
    }
}
