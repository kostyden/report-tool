namespace ReportTool.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using TestStack.White;
    using TestStack.White.Configuration;
    using TestStack.White.Factory;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.Finders;
    using TestStack.White.UIItems.ListBoxItems;

    [TestFixture]
    [Category("Acceptence tests")]
    public class AcceptenceTests
    {
        [Test]
        public void OnStartupFilePathInputControlShouldBeEmpty()
        {
            using (var application = LaunchApplication())
            {
                var window = application.GetWindows().First();
                var textBox = window.Get<TextBox>(SearchCriteria.ByAutomationId("filePath"));
                textBox.Text.Should().BeEmpty();
            }
        }

        [Test]
        public void WithValidPathShouldLoadExpectedColumnsAfterClickingLoadButton()
        {
            using (var application = LaunchApplication())
            {
                var window = application.GetWindows().First();
                var textBox = window.Get<TextBox>(SearchCriteria.ByAutomationId("filePath"));
                var button = window.Get<Button>(SearchCriteria.ByAutomationId("loadData"));
                var listBox = window.Get<ListBox>(SearchCriteria.ByAutomationId("columns"));

                textBox.BulkText = "";
                button.Click();

                var actualColumns = listBox.Items.Select(item => item.Text);
                var expectedColumns = new[]
                {
                    "Age",
                    "Weight",
                    "Height"
                };

                actualColumns.ShouldBeEquivalentTo(expectedColumns);
            }
        }

        private Application LaunchApplication()
        {
            var directoryName = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
            var exePath = Path.Combine(directoryName, @"ReportTool.exe");
            return Application.Launch(exePath);
        }
    }
}
