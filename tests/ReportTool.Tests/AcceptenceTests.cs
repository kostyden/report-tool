namespace ReportTool.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using TestStack.White;
    using TestStack.White.Factory;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.Finders;
    using TestStack.White.UIItems.ListBoxItems;
    using TestStack.White.UIItems.WPFUIItems;

    [TestFixture]
    public class AcceptenceTests
    {
        [Test]
        public void OnStartupFilePathInputControlShouldBeEmpty()
        {
            var directoryName = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            var exePath = Path.Combine(directoryName, @"ReportTool.exe");
            using (var application = Application.Launch(exePath))
            {
                var window = application.GetWindow("Report tool", InitializeOption.NoCache);
                var textBox = window.Get<TextBox>(SearchCriteria.ByAutomationId("filePath"));
                textBox.Text.Should().BeEmpty();
            }
        }

        [Test]
        public void WithValidPathShouldLoadExpectedColumnsAfterClickingLoadButton()
        {
            var directoryName = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            var exePath = Path.Combine(directoryName, @"ReportTool.exe");
            using (var application = Application.Launch(exePath))
            {
                var window = application.GetWindow("Report tool", InitializeOption.NoCache);
                var textBox = window.Get<TextBox>(SearchCriteria.ByAutomationId("filePath"));
                var button = window.Get<Button>(SearchCriteria.ByAutomationId("loadData"));
                var listBox = window.Get<ListBox>(SearchCriteria.ByAutomationId("columns"));

                textBox.BulkText = "";
                button.Click();

                var actualColumns = listBox.Items.Select(item => item.Text);
                var expectedColumns = new[]
                {
                    "id",
                    "age",
                    "weight"
                };

                actualColumns.ShouldBeEquivalentTo(expectedColumns);
            }
        }
    }
}
