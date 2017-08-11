namespace ReportTool.Tests.DataProviders.FileDataProviders
{
    using FluentAssertions;
    using NSubstitute;
    using NUnit.Framework;
    using ReportTool.DataProviders;
    using ReportTool.DataProviders.FileDataProviders;
    using System.Collections.Generic;

    [TestFixture]
    class FileDataProviderTests
    {
        private IDataReaderProvider _fakeReaderProvider;

        private IDataReader _fakeReader;

        private FileDataProvider _provider;

        [SetUp]
        public void SetUp()
        {
            _fakeReaderProvider = Substitute.For<IDataReaderProvider>();
            _fakeReader = Substitute.For<IDataReader>();

            _provider = new FileDataProvider(_fakeReaderProvider);
        }

        [Test]
        [TestCase("fileName", ".xlsx")]
        [TestCase("anotherName", ".csv")]
        public void GetFrom_ShouldReturnSuccessfulResultReturnedByProvidedReader(string fileName, string fileExtension)
        {
            var path = $"{fileName}.{fileExtension}";
            var data = new[]
            {
                new Dictionary<string, double> {{ "one", 1.1 }, { "two", 2.1 }},
                new Dictionary<string, double> {{ "one", 1.2 }, { "two", 2.2 }}
            };
            var expectedResult = DataResult.CreateSuccessful(data);

            _fakeReaderProvider.GetByExtension(fileExtension).Returns(_fakeReader);
            _fakeReader.Read(path).Returns(expectedResult);
            
            var result = _provider.GetFrom(path);

            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Test]
        [TestCase("testFile", ".test", "File not found")]
        [TestCase("anotherFile", ".com", "Reader not found for this extension")]
        public void GetFrom_ShouldReturnFailedResultWithMessageProvidedByDataReader(string fileName, string fileExtension, string errorMessage)
        {
            var path = $"{fileName}.{fileExtension}";
            var expectedResult = DataResult.CreateFailed(errorMessage);

            _fakeReaderProvider.GetByExtension(fileExtension).Returns(_fakeReader);
            _fakeReader.Read(path).Returns(expectedResult);

            var result = _provider.GetFrom(path);

            result.ShouldBeEquivalentTo(expectedResult);
        }
    }
}
