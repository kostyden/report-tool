namespace ReportTool.Tests.MainViewModelTests
{
    using FluentAssertions;
    using NSubstitute;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;

    class SelectColumnCommandTests : MainViewModelTestsBase
    {
        [Test]
        public void SelectColumnCommand_WhenNoColumnsSelectedShouldSetGivenColumnSlectionTypeToAbscissa()
        {
            var data = new[]
            {
                new Dictionary<string, double> { { "seven", 0.00052 }, { "eight", 1.000012}, { "nine", 1.1} }
            };
            var dataResult = DataResult.CreateSuccessful(data);
            FakeProvider.GetFrom(null).ReturnsForAnyArgs(dataResult);

            ViewModel.LoadDataCommand.Execute("dummy.path");
            var columnToSelect = ViewModel.Columns.First(column => column.Name.Equals("nine"));

            ViewModel.SelectColumnCommand.Execute(columnToSelect);

            columnToSelect.SelectionType.Should().Be(SelectionType.Abscissa);
        }
    }
}
