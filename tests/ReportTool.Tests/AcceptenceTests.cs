namespace ReportTool.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Reflection;
    using TestStack.White;
    using TestStack.White.Factory;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.Finders;

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
    }
}
