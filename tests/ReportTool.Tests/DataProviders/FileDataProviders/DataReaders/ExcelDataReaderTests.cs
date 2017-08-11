namespace ReportTool.Tests.DataProviders.FileDataProviders.DataReaders
{
    using FluentAssertions;
    using NUnit.Framework;
    using ReportTool.DataProviders;
    using ReportTool.DataProviders.FileDataProviders;
    using ReportTool.DataProviders.FileDataProviders.DataReaders;
    using System.Collections.Generic;

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
            var path = TestUtils.BuildPathFor(@"DataReadersTestCases\ExcelDataReaderTestCase1.xlsx");
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
            var path = TestUtils.BuildPathFor(@"DataReadersTestCases\ExcelDataReaderTestCase3ProtectedView.xlsx");
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
            var path = TestUtils.BuildPathFor(@"DataReadersTestCases\NotExistedFile.xlsx");
            var reader = new ExcelDataReader();

            var result = reader.Read(path);

            result.Data.Should().BeEmpty();
            result.ErrorMessage.Should().ContainEquivalentOf("Could not find file");
        }

        [Test]
        public void Read_ShouldReturnFailedResultWithExpectedMessageWhenValuesInWrongFormat()
        {
            var path = TestUtils.BuildPathFor(@"DataReadersTestCases\ExcelDataReaderTestCase2.xlsx");
            var reader = new ExcelDataReader();

            var result = reader.Read(path);

            result.Data.Should().BeEmpty();
            result.ErrorMessage.Should().ContainEquivalentOf("string was not in a correct format");
        }
    }
}
