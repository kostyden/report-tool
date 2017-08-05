namespace ReportTool.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using System;

    [TestFixture]
    class MainViewModelTests
    {
        [Test]
        public void Columns_ShouldBeEmptyAfterInitialization()
        {
            var viewmodel = new MainViewModel();
            viewmodel.Columns.Should().BeEmpty();
        }
    }
}
