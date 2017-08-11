namespace ReportTool.Tests.UI.ViewModels
{
    using FluentAssertions;
    using NUnit.Framework;
    using ReportTool.UI;
    using ReportTool.UI.ViewModels;

    [TestFixture]
    class DataColumnViewModelTests
    {
        private DataColumnViewModel _viewmodel;

        [SetUp]
        public void SetUp()
        {
            _viewmodel = new DataColumnViewModel("dummyName");
        }

        [Test]
        [TestCase(SelectionType.NotSelected, SelectionType.Abscissa)]
        [TestCase(SelectionType.Ordinate, SelectionType.NotSelected)]
        public void SelectionType_ShouldRaisePpropertyChangedWhenNewValueGiven(SelectionType originalType, SelectionType newType)
        {
            ExecuteTest(originalType, newType);
            _viewmodel.ShouldRaisePropertyChangeFor(vm => vm.SelectionType);
        }

        [Test]
        [TestCase(SelectionType.NotSelected, SelectionType.NotSelected)]
        [TestCase(SelectionType.Ordinate, SelectionType.Ordinate)]
        public void SelectionType_ShouldNotRaisePpropertyChangedWhenSameValueGiven(SelectionType originalType, SelectionType newType)
        {
            ExecuteTest(originalType, newType);
            _viewmodel.ShouldNotRaisePropertyChangeFor(vm => vm.SelectionType);
        }

        private void ExecuteTest(SelectionType originalType, SelectionType newType)
        {
            _viewmodel.SelectionType = originalType;
            _viewmodel.MonitorEvents();
            _viewmodel.SelectionType = newType;
        }
    }
}
