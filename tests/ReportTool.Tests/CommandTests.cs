namespace ReportTool.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class CommandTests
    {
        [Test]
        public void Execute_ShouldExecuteActionProvidedDuringConstruction()
        {
            var isExecuted = false;
           var dummyParam = "dummy";
            Action<object> fakeAction = param => isExecuted = true;
            var command = new Command(fakeAction);

            command.Execute(dummyParam);

            isExecuted.Should().BeTrue();
        }

        [Test]
        [TestCase("parameter")]
        [TestCase(123.45)]
        public void Execute_ShouldPassGivenParameterToTheAction(object expectedParameter)
        {
            object actualPatameter = null;
            Action<object> fakeAction = param => actualPatameter = param;
            var command = new Command(fakeAction);

            command.Execute(expectedParameter);

            actualPatameter.Should().Be(expectedParameter);
        }
    }
}
