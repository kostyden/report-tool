namespace ReportTool.Tests
{
    using System;
    using NUnit.Framework;
    using System.Collections.Generic;
    using FluentAssertions;

    [TestFixture]
    class DataResultTests
    {
        [Test]
        public void CreateSuccessful_ShouldReturnResultWithGivenData()
        {
            var expectedData = new[]
            {
                new Dictionary<string, double> {{ "test", 12.3 }, { "age", 34 }},
                new Dictionary<string, double> {{ "test", 12.87 }, { "age", 45 }}
            };

            var result = DataResult.CreateSuccessful(expectedData);

            result.Data.ShouldBeEquivalentTo(expectedData);
        }

        [Test]
        public void CreateSuccessful_ShouldReturnResultWithErrorMessageEqualsNull()
        {
            var data = new[]
            {
                new Dictionary<string, double> {{ "second", 497.3 }, { "third", 3.1415 }},
                new Dictionary<string, double> {{ "second", 6.87 }, { "third", 0.12 }}
            };

            var result = DataResult.CreateSuccessful(data);

            result.ErrorMessage.Should().BeNull();
        }

        [Test]
        public void CreateFailed_ShouldReturnResultWithEmptyData()
        {
            var errorMessage = "something gone wrong";

            var result = DataResult.CreateFailed(errorMessage);

            result.Data.Should().BeEmpty();
        }

        [Test]
        public void CreateFailed_ShouldReturnResultWithErrorMessageEqualsGivenMessage()
        {
            var errorMessage = "File not found";

            var result = DataResult.CreateFailed(errorMessage);

            result.ErrorMessage.Should().Be(errorMessage);
        }
    }
}
