namespace ReportTool.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using System.Linq;
    using TestStack.White;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.Finders;

    [TestFixture]
    class StartupTests
    {
        [Test]
        public void ShouldOpenMainWindowWithDefaultView()
        {
            using (var application = LaunchApplication())
            {
                var window = application.GetWindows().First();
                var textBox = window.Get<TextBox>(SearchCriteria.ByAutomationId("filePath"));
                textBox.Text.Should().BeEmpty();
            }
        }

        [Test]
        [TestCase(@"DataReadersTestCases\ExcelDataReaderTestCase1.xlsx", new[] { "one", "two", "three" })]
        [TestCase(@"DataReadersTestCases\CsvDataReaderTestCase1CommaDelimited.csv", new[] { "one", "two", "three" })]
        [TestCase(@"DataReadersTestCases\JsonDataReaderTestCase1.json", new[] { "weight", "speed", "tyre-consumption" })]
        public void ShouldDisplayCorrectColumnsAfterLoadingData(string fileName, string[] expectedColumnNames)
        {
            using (var application = LaunchApplication())
            {
                var window = application.GetWindows().First();
                var textBox = window.Get<TextBox>(SearchCriteria.ByAutomationId("filePath"));
                var loadButton = window.Get<Button>(SearchCriteria.ByAutomationId("loadData"));

                textBox.BulkText = TestUtils.BuildPathFor(fileName);
                loadButton.Click();

                var actualColumnNames = window.GetMultiple(SearchCriteria.ByAutomationId("columnName"))
                                              .OfType<WPFLabel>()
                                              .Select(label => label.Text);

                actualColumnNames.ShouldBeEquivalentTo(expectedColumnNames);
            }
        }

        private Application LaunchApplication()
        {
            var exePath = TestUtils.BuildPathFor(@"ReportTool.exe");
            return Application.Launch(exePath);
        }
    }
}
