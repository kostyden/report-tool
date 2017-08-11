namespace ReportTool.Tests.DataProviders.FileDataProviders.DataReaders
{
    using FluentAssertions;
    using NUnit.Framework;
    using ReportTool.DataProviders.FileDataProviders.DataReaders;

    [TestFixture]
    public class NotSupportedDataReaderTests 
    {
        private NotSupportedDataReader _reader;

        [SetUp]
        public void SetUp()
        {
            _reader = new NotSupportedDataReader();
        }

        [Test]
        public void Read_ShouldReturnFailedResult()
        {
            var result = _reader.Read("notsupported.extension");        
            result.Data.Should().BeEmpty();
        }

        [Test]
        public void Read_ShouldReturnFailedResultWithExpectedMessage()
        {
            var result = _reader.Read("file.notsupported");
            result.ErrorMessage.Should().ContainEquivalentOf("reader not found");
        }

        [Test]
        public void Read_ShouldReturnFailedResultWithMessageContainsGivenPath()
        {
            var path = "file.com";

            var result = _reader.Read(path);

            result.ErrorMessage.Should().ContainEquivalentOf(path);
        }
    }
}
