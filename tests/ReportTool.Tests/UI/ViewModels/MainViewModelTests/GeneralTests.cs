namespace ReportTool.Tests.UI.ViewModels.MainViewModelTests
{
    using FluentAssertions;
    using NSubstitute;
    using NUnit.Framework;
    using ReportTool.DataProviders;
    using ReportTool.Reports;
    using ReportTool.UI;
    using ReportTool.UI.ViewModels;
    using System.Collections.Generic;
    using System.Linq;

    class GeneralTests : MainViewModelTestsBase
    {
        [Test]
        public void Columns_ShouldBeEmptyAfterInitialization()
        {
            ViewModel.Columns.Should().BeEmpty();
        }

        [Test]
        public void Report_ShouldBeEmptyAfterInitialization()
        {
            ViewModel.Report.ShouldBeEquivalentTo(ScatterReportData.Empty);
        }

        [Test]
        public void AbscissaColumnName_ShouldReturnNameOfColumnSelectedForAbscissaWhenSelected()
        {
            LoadColumns();
            var selectedColumn = ViewModel.Columns.First(column => column.Name.Equals("two"));
            selectedColumn.SelectionType = SelectionType.Abscissa;

            ViewModel.AbscissaColumnName.Should().Be(selectedColumn.Name);
        }

        [Test]
        public void AbscissaColumnName_ShouldReturnDefaultTextWhenAbscissaColumnNotSelected()
        {
            LoadColumns();
            var selectedColumn = ViewModel.Columns.First(column => column.Name.Equals("one"));
            selectedColumn.SelectionType = SelectionType.Ordinate;

            ViewModel.AbscissaColumnName.Should().Be(MainViewModel.NOT_SELECTED);
        }

        [Test]
        public void OrdinateColumnName_ShouldReturnNameOfColumnSelectedForAbscissaWhenSelected()
        {
            LoadColumns();
            var selectedColumn = ViewModel.Columns.First(column => column.Name.Equals("two"));
            selectedColumn.SelectionType = SelectionType.Ordinate;


            ViewModel.OrdinateColumnName.Should().Be(selectedColumn.Name);
        }

        [Test]
        public void OrdinateColumnName_ShouldReturnDefaultTextWhenAbscissaColumnNotSelected()
        {
            LoadColumns();
            var selectedColumn = ViewModel.Columns.First(column => column.Name.Equals("one"));
            selectedColumn.SelectionType = SelectionType.Abscissa;

            ViewModel.OrdinateColumnName.Should().Be(MainViewModel.NOT_SELECTED);
        }

        private void LoadColumns()
        {
            var data = new[]
{
                new Dictionary<string, double>
                {
                    { "one", 0.00052 },
                    { "two", 2.17 },
                    { "three", 3.14 }
                }
            };

            var dataResult = DataResult.CreateSuccessful(data);
            FakeProvider.GetFrom(null).ReturnsForAnyArgs(dataResult);

            ViewModel.LoadDataCommand.Execute("dummy.path");
        }
    }
}
