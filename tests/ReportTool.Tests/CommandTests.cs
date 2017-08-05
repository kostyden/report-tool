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
            Func<object, bool> fakePredicate = param => true;
            var command = new Command(fakeAction, fakePredicate);

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
            Func<object, bool> fakePredicate = param => true;
            var command = new Command(fakeAction, fakePredicate);

            command.Execute(expectedParameter);

            actualPatameter.Should().Be(expectedParameter);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void CanExecute_ShouldReturnResultOfProvidedPredicate(bool expected)
        {
            var dummyParam = 42;
            Action<object> fakeAction = param => { };
            Func<object, bool> fakePredicate = param => expected;
            var command = new Command(fakeAction, fakePredicate);

            var actual = command.CanExecute(dummyParam);

            actual.Should().Be(expected);
        }

        [Test]
        [TestCase(true)]
        [TestCase(34)]
        public void CanExecute_ShouldPassGivenParameterToThePredicate(object expectedParameter)
        {
            object actualPatameter = null;
            Action<object> fakeAction = param => { };
            Func<object, bool> fakePredicate = param =>
            {
                actualPatameter = param;
                return true;
            };
            var command = new Command(fakeAction, fakePredicate);

            command.CanExecute(expectedParameter);

            actualPatameter.Should().Be(expectedParameter);
        }
    }
}
