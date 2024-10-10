using ElevatorSimulator.Application.ElevatorApplication.StateContext.States;
using ElevatorSimulator.Application.ElevatorApplication.StateContext;
using ElevatorSimulator.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator.Tests.StateFlow
{
    internal class IdleStateFlowTests
    {
        private Mock<IElevatorStateContext> _mockContext;
        private Mock<IElevator> _mockElevator;
        private IdleState _idleState;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<IElevatorStateContext>();
            _mockElevator = new Mock<IElevator>();

            _mockContext.SetupGet(c => c.Elevator).Returns(_mockElevator.Object);
            _idleState = new IdleState();
        }

        [Test]
        public async Task When_EnterState_ShouldBeStatus_Idle()
        {
            _mockElevator.Object.Status = Domain.Enums.ElevatorStatus.MovingUp;
            await _idleState.EnterState(_mockContext.Object);

            Assert.That(_mockElevator.Object.Status == Domain.Enums.ElevatorStatus.Idle, "Enter state for Idle state did not update the elevator object");
        }

        [Test]
        public async Task Given_RequestHigherFloor_MustMoveUp()
        {
            _mockElevator.Setup(e => e.CurrentFloor).Returns(2);
            _mockContext.Setup(c => c.TransitionToState(It.IsAny<IState>())).Verifiable();
            var request = new Request { CurrentFloor = 6 };
            await _idleState.ProcessRequest(_mockContext.Object, request);

            _mockContext.Verify(i => i.TransitionToState(It.Is<MovingState>(p => p._direction == Domain.Enums.ElevatorStatus.MovingUp)), Times.Once);
        }

        [Test]
        public async Task Given_RequestLowerFloor_MustMoveDown()
        {
            _mockElevator.Setup(e => e.CurrentFloor).Returns(6);
            _mockContext.Setup(c => c.TransitionToState(It.IsAny<IState>())).Verifiable();
            var request = new Request { CurrentFloor = 2 };
            await _idleState.ProcessRequest(_mockContext.Object, request);

            _mockContext.Verify(i => i.TransitionToState(It.Is<MovingState>(p => p._direction == Domain.Enums.ElevatorStatus.MovingDown)), Times.Once);
        }
    }
}
