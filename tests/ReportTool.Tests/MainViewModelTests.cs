namespace ReportTool.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    class MainViewModelTests
    {
        [Test]
        public void Columns_ShouldBeEmptyAfterInitialization()
        {
            var viewmodel = new MainViewModel();
            viewmodel.Columns.Should().BeEmpty();
        }

        [Test]
        public void Columns_ShouldContainLoadedColumnsAfterSuccessfulExecutingLoadDataCommand()
        {
            var viewmodel = new MainViewModel();
            var validPath = @"somefile.xls";
            var loadedData = new[]
            {
                new Dictionary<string, double> { { "one", 1.23 }, { "two", 59.4 } },
                new Dictionary<string, double> { { "one", 1.23 }, { "two", 59.4 } },
                new Dictionary<string, double> { { "one", 1.23 }, { "two", 59.4 } }
            };
            var expectedColumns = loadedData.SelectMany(data => data.Keys).Distinct();

            viewmodel.LoadDataCommand.Execute(validPath);

            viewmodel.Columns.ShouldBeEquivalentTo(expectedColumns);
        }
    }
}
