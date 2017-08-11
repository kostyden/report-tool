namespace ReportTool.Tests.Commands
{
    using FluentAssertions;
    using NUnit.Framework;
    using ReportTool.Commands;
    using System;

    [TestFixture]
    public class CommandTests
    {
        private CommandBuilder _commandBuilder;

        [SetUp]
        public void SetUp()
        {
            _commandBuilder = new CommandBuilder();
        }
        
        [Test]
        public void Execute_ShouldExecuteActionProvidedDuringConstruction()
        {
            var isExecuted = false;
           var dummyParam = "dummy";
            Action<object> action = param => isExecuted = true;
            var command = _commandBuilder.With(action).Build();

            command.Execute(dummyParam);

            isExecuted.Should().BeTrue();
        }

        [Test]
        [TestCase("parameter")]
        [TestCase(123.45)]
        public void Execute_ShouldPassGivenParameterToTheAction(object expectedParameter)
        {
            object actualPatameter = null;
            Action<object> action = param => actualPatameter = param;
            var command = _commandBuilder.With(action).Build();

            command.Execute(expectedParameter);

            actualPatameter.Should().Be(expectedParameter);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void CanExecute_ShouldReturnResultOfProvidedPredicate(bool expected)
        {
            var dummyParam = 42;
            Func<object, bool> predicate = param => expected;
            var command = _commandBuilder.With(predicate).Build();

            var actual = command.CanExecute(dummyParam);

            actual.Should().Be(expected);
        }

        [Test]
        [TestCase(true)]
        [TestCase(34)]
        public void CanExecute_ShouldPassGivenParameterToThePredicate(object expectedParameter)
        {
            object actualPatameter = null;
            Func<object, bool> predicate = param =>
            {
                actualPatameter = param;
                return true;
            };
            var command = _commandBuilder.With(predicate).Build();

            command.CanExecute(expectedParameter);

            actualPatameter.Should().Be(expectedParameter);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void CanExecute_ShouldReturnTrueWhenCommandConstructedWithoneParameter(object dummyParameter)
        {
            Action<object> action = param => { };
            var command = new Command(action);

            var actual = command.CanExecute(dummyParameter);

            actual.Should().BeTrue();
        }

        [Test]
        public void RaiseCanExecuteChanged_ShouldRaiseCanExecuteEvent()
        {
            var command = _commandBuilder.Build();
            command.MonitorEvents();

            command.RaiseCanExecuteChanged();

            command.ShouldRaise(nameof(command.CanExecuteChanged));
        }

        [Test]
        public void RaiseCanExecuteChanged_ShouldRaiseCanExecuteEventWithItselfAsSender()
        {
            var command = _commandBuilder.Build();
            command.MonitorEvents();

            command.RaiseCanExecuteChanged();

            command.ShouldRaise(nameof(command.CanExecuteChanged)).WithSender(command);
        }

        [Test]
        public void RaiseCanExecuteChanged_ShouldRaiseCanExecuteEventWithEmptyArgs()
        {
            var command = _commandBuilder.Build();
            command.MonitorEvents();

            command.RaiseCanExecuteChanged();

            command.ShouldRaise(nameof(command.CanExecuteChanged)).WithArgs<EventArgs>();
        }

        [Test]
        public void RaiseCanExecuteChanged_ShouldNotThrowWhenEventIsNull()
        {
            var command = _commandBuilder.Build();

            Action raise = command.RaiseCanExecuteChanged;

            raise.ShouldNotThrow<NullReferenceException>();
        }

        private class CommandBuilder
        {
            private Action<object> _action;

            private Func<object, bool> _predicate;

            public CommandBuilder()
            {
                _action = param => { };
                _predicate = param => true;
            }

            public CommandBuilder With(Action<object> action)
            {
                _action = action;
                return this;
            }

            public CommandBuilder With(Func<object, bool> predicate)
            {
                _predicate = predicate;
                return this;
            }

            public Command Build()
            {
                return new Command(_action, _predicate);
            }
        }
    }
}
