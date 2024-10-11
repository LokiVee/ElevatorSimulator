using ElevatorSimulator.Application.ElevatorApplication.StateContext.States;
using ElevatorSimulator.Application.ElevatorApplication.StateContext;
using ElevatorSimulator.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Domain.Enums;

namespace ElevatorSimulator.Tests.StateFlow
{
    [TestFixture]
    public class DropOffStateFlowTests
    {
        private Mock<IElevatorStateContext> _mockContext;
        private DropOffState _dropOffState;
        private Request _request;
        private Mock<IElevator> _mockElevator;

        [SetUp]
        public void SetUp()
        {
            _mockContext = new Mock<IElevatorStateContext>();
            _dropOffState = new DropOffState();
            _request = new Request { CurrentFloor = 1, TargetFloor = 5, ObjectWaiting = 3 };
            _mockElevator = new Mock<IElevator>();

            _mockContext.SetupGet(c => c.Elevator).Returns(_mockElevator.Object);
        }

        [Test]
        public async Task EnterState_ShouldSetElevatorStatusToDropOff()
        {
            await _dropOffState.EnterState(_mockContext.Object);

            _mockElevator.VerifySet(e => e.Status = ElevatorStatus.DropOff);
        }

        [Test]
        public async Task ExitState_ShouldCompleteSuccessfully()
        {
            Assert.DoesNotThrowAsync(() => _dropOffState.ExitState(_mockContext.Object));
        }

        [Test]
        public async Task ProcessRequest_ShouldDecreaseCurrentCapacity()
        {
            _mockElevator.SetupGet(e => e.CurrentCapacity).Returns(10);

            await _dropOffState.ProcessRequest(_mockContext.Object, _request);

            _mockElevator.VerifySet(e => e.CurrentCapacity = 7);
        }

        [Test]
        public async Task ProcessRequest_ShouldRemoveRequest()
        {
            await _dropOffState.ProcessRequest(_mockContext.Object, _request);

            _mockContext.Verify(c => c.RemoveRequest(_request));
        }

        [Test]
        public async Task ProcessRequest_ShouldTransitionToIdleState()
        {
            await _dropOffState.ProcessRequest(_mockContext.Object, _request);

            _mockContext.Verify(c => c.TransitionToState(It.IsAny<IdleState>()));
        }
    }
}
