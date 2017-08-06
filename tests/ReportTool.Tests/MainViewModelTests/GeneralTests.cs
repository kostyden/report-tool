namespace ReportTool.Tests.MainViewModelTests
{
    using FluentAssertions;
    using NUnit.Framework;
    using System;

    class GeneralTests : MainViewModelTestsBase
    {
        [Test]
        public void Columns_ShouldBeEmptyAfterInitialization()
        {
            ViewModel.Columns.Should().BeEmpty();
        }
    }
}
