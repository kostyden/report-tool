namespace ReportTool.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    class DataReaderProviderTests
    {
        [Test]
        public void GetByExtension_ShouldReturnReaderWhichTypeDecoratedWithGivenExtension()
        {
            var expectedReader = new OneReader();
            var readers = new IDataReader[]
            {
                new TwoReader(),
                expectedReader,
                new ThreeReader()
            };
            var provider = new DataReaderProvider(readers);

            var actualReader = provider.GetByExtension("csv");

            actualReader.Should().BeSameAs(expectedReader);
        }

        [Test]
        public void GetByExtension_ShouldReturnSameReaderForTwoExtensionsWhenReaderSupportsGivenExtensions()
        {
            var expectedReader = new TwoReader();
            var readers = new IDataReader[]
            {
                new OneReader(),
                new ThreeReader(),
                expectedReader
            };
            var provider = new DataReaderProvider(readers);

            var xlsReader = provider.GetByExtension("xls");
            var xlsxReader = provider.GetByExtension("xlsx");

            xlsReader.Should().BeSameAs(expectedReader);
            xlsReader.Should().BeSameAs(xlsxReader);
        }

        [Test]
        public void GetByExtension_ShouldReturnNotSupportedReaderWhenNoReaderFoundForGivenExtension()
        {
            var readers = new IDataReader[]
            {
                new OneReader(),
                new TwoReader(),
                new ThreeReader(),
            };
            var provider = new DataReaderProvider(readers);

            var actualReader = provider.GetByExtension("test");

            actualReader.Should().BeOfType<NotSupportedDataReader>();
        }

        [SupportedFileExtension("csv")]
        private class OneReader : ZeroReader { }

        [SupportedFileExtension("xlsx")]
        [SupportedFileExtension("xls")]
        private class TwoReader : ZeroReader { }

        [SupportedFileExtension("json")]
        private class ThreeReader : ZeroReader { }

        private abstract class ZeroReader : IDataReader
        {
            public DataResult Read(string path)
            {
                throw new NotImplementedException();
            }
        }
    }
}
