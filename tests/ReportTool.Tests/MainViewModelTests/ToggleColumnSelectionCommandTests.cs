namespace ReportTool.Tests.MainViewModelTests
{
    using FluentAssertions;
    using NSubstitute;
    using NUnit.Framework;
    using ReportTool.DataProviders;
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

        [Test]
        public void ShouldRaisePropertyChangedForAbscissaColumnName()
        {
            var columnToSelect = ViewModel.Columns.First(column => column.Name.Equals("one"));
            ViewModel.MonitorEvents();

            ViewModel.ToggleColumnSelectionCommand.Execute(columnToSelect);

            ViewModel.ShouldRaisePropertyChangeFor(viewmodel => viewmodel.AbscissaColumnName);
        }

        [Test]
        public void ShouldRaisePropertyChangedForOrdinateColumnName()
        {
            var columnToSelect = ViewModel.Columns.First(column => column.Name.Equals("six"));
            ViewModel.MonitorEvents();

            ViewModel.ToggleColumnSelectionCommand.Execute(columnToSelect);

            ViewModel.ShouldRaisePropertyChangeFor(viewmodel => viewmodel.OrdinateColumnName);
        }

        [Test]
        public void ShouldRaiseCanExecuteChangedEventForGenerateReportDataCommand()
        {
            var columnToSelect = ViewModel.Columns.First(column => column.Name.Equals("six"));
            ViewModel.GenerateReportDataCommand.MonitorEvents();

            ViewModel.ToggleColumnSelectionCommand.Execute(columnToSelect);

            ViewModel.GenerateReportDataCommand.ShouldRaise(nameof(ViewModel.GenerateReportDataCommand.CanExecuteChanged));
        }

        [Test]
        public void ShouldResetReportDataAfterRequiredColumnWasUnselected()
        {
            var inputData = new ScatterInputData
            {
                Data = new[]
                {
                    new Dictionary<string, double> { { "seven", 0.00052 }, { "eight", 1.000012}, { "nine", 1.1} },
                    new Dictionary<string, double> { { "seven", 0.0000010101 }, { "eight", 0.12456 }, { "nine", 1.2 } }
                },
                AbscissaColumnName = "seven",
                OrdinateColumnName = "eight"
            };

            var dataResult = DataResult.CreateSuccessful(inputData.Data);
            FakeProvider.GetFrom(null).ReturnsForAnyArgs(dataResult);

            var dummyReportData = GenerateDummyReportData();
            FakeScatterReportCalculator.Calculate(null).ReturnsForAnyArgs(dummyReportData);

            ViewModel.LoadDataCommand.Execute("dummy.path");
            var abscissaColumn = ViewModel.Columns.First(column => column.Name.Equals("seven"));
            var ordinateColumn = ViewModel.Columns.First(column => column.Name.Equals("nine"));
            abscissaColumn.SelectionType = SelectionType.Abscissa;
            ordinateColumn.SelectionType = SelectionType.Ordinate;

            ViewModel.GenerateReportDataCommand.Execute(null);
            ViewModel.ToggleColumnSelectionCommand.Execute(abscissaColumn);

            ViewModel.Report.ShouldBeEquivalentTo(ScatterReportData.Empty);
        }

        private void AssertThatSelectionTypeIsSetCorrectlyFor(DataColumnViewModel column, SelectionType expectedType)
        {
            ViewModel.ToggleColumnSelectionCommand.Execute(column);
            column.SelectionType.Should().Be(expectedType);
        }

        private ScatterReportData GenerateDummyReportData()
        {
            var points = new[]
            {
                new ScatterPoint(1.2, 4.3),
                new ScatterPoint(1.3, 4.2),
                new ScatterPoint(1.4, 4.1),
                new ScatterPoint(1.5, 4.0)
            };
            var trendLine = new ScatterLine(new ScatterPoint(1.0, 4.15), new ScatterPoint(1.5, 4.15));

            return new ScatterReportData(points, trendLine);
        }
    }
}
