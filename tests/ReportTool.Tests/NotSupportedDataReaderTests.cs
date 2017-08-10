namespace ReportTool.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using System;

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
