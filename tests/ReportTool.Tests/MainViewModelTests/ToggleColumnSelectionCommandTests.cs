namespace ReportTool.Tests.MainViewModelTests
{
    using FluentAssertions;
    using NSubstitute;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;

    class ToggleColumnSelectionCommandTests : MainViewModelTestsBase
    {
        [SetUp]
        public void SetUp()
        {
            var data = new[]
            {
                new Dictionary<string, double>
                {
                    { "one", 0.00052 },
                    { "two", 2.17 },
                    { "three", 3.14 },
                    { "four", 4.42 },
                    { "five", 1.5 },
                    { "six", 16 },
                    { "seven", 71.51 },
                    { "eight", 64.12 },
                    { "nine", 29.00900 },
                    { "ten", 1.00010010001 },
                }
            };

            var dataResult = DataResult.CreateSuccessful(data);
            FakeProvider.GetFrom(null).ReturnsForAnyArgs(dataResult);

            ViewModel.LoadDataCommand.Execute("dummy.path");
        }

        [Test]
        public void WhenNoColumnsSelectedShouldSetGivenColumnSelectionTypeToAbscissa()
        {
            var columnToSelect = ViewModel.Columns.First(column => column.Name.Equals("nine"));
            AssertThatSelectionTypeIsSetCorrectlyFor(columnToSelect, SelectionType.Abscissa);
        }

        [Test]
        public void WhenColumnsContainsOnlyColumnForAbcsissaShouldSetSelectionToOrdinateForGivenColumn()
        {
            ViewModel.Columns.First(column => column.Name.Equals("one")).SelectionType = SelectionType.Abscissa;
            var columnToSelect = ViewModel.Columns.First(column => column.Name.Equals("two"));

            AssertThatSelectionTypeIsSetCorrectlyFor(columnToSelect, SelectionType.Ordinate);
        }

        [Test]
        public void WhenColumnsContainsOnlyColumnForOrdinateShouldSetSelectionToAscissaForGivenColumn()
        {
            ViewModel.Columns.First(column => column.Name.Equals("five")).SelectionType = SelectionType.Ordinate;
            var columnToSelect = ViewModel.Columns.First(column => column.Name.Equals("seven"));

            AssertThatSelectionTypeIsSetCorrectlyFor(columnToSelect, SelectionType.Abscissa);
        }

        [Test]
        public void WhenColumnsContainsColumnsForAbscissaAndOrdinateGivenColumnSelectionTypeShouldRemainNotSelected()
        {
            ViewModel.Columns.First(column => column.Name.Equals("seven")).SelectionType = SelectionType.Abscissa;
            ViewModel.Columns.First(column => column.Name.Equals("eight")).SelectionType = SelectionType.Ordinate;
            var columnToSelect = ViewModel.Columns.First(column => column.Name.Equals("four"));

            AssertThatSelectionTypeIsSetCorrectlyFor(columnToSelect, SelectionType.NotSelected);
        }
        [Test]
        [TestCase(SelectionType.Abscissa)]
        [TestCase(SelectionType.Ordinate)]
        public void ShouldRemoveSelectionWhenGivenColumnHaveSelectedType(SelectionType selectedType)
        {
            var columnToSelect = ViewModel.Columns.First(column => column.Name.Equals("ten"));
            columnToSelect.SelectionType = selectedType;

            AssertThatSelectionTypeIsSetCorrectlyFor(columnToSelect, SelectionType.NotSelected);
        }

        private void AssertThatSelectionTypeIsSetCorrectlyFor(DataColumnViewModel column, SelectionType expectedType)
        {
            ViewModel.ToggleColumnSelectionCommand.Execute(column);
            column.SelectionType.Should().Be(expectedType);
        }
    }
}
