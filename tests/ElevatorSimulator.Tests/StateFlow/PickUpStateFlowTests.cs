using ElevatorSimulator.Application.ElevatorApplication.StateContext.States;
using ElevatorSimulator.Application.ElevatorApplication.StateContext;
using ElevatorSimulator.Domain.Entities;
using ElevatorSimulator.Domain.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator.Tests.StateFlow
{
    internal class PickUpStateFlowTests
    {
        private Mock<IElevatorStateContext> _mockContext;
        private Mock<IElevator> _mockElevator;
        private PickupState _pickupState;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<IElevatorStateContext>();
            _mockElevator = new Mock<IElevator>();
            _mockContext.SetupGet(c => c.Elevator).Returns(_mockElevator.Object);
            _pickupState = new PickupState(ElevatorStatus.MovingUp, 5);
        }

        [Test]
        public async Task EnterState_SetsElevatorStatusToPickingUp()
        {
            // Act
            await _pickupState.EnterState(_mockContext.Object);

            // Assert
            _mockElevator.VerifySet(e => e.Status = ElevatorStatus.PickingUp);
        }

   
        [Test]
        public async Task ProcessRequest_PicksUpPassengersAndTransitionsToMovingState()
        {
            // Arrange
            var request = new Request { CurrentFloor = 2, TargetFloor = 5, ObjectWaiting = 3 };
            var onboardRequests = new List<Request>();
            _mockContext.Setup(c => c._onboardRequests).Returns(onboardRequests);
            _mockElevator.SetupGet(e => e.CurrentCapacity).Returns(0);
            _mockElevator.SetupSet(e => e.CurrentCapacity = It.IsAny<double>());

            // Act
            await _pickupState.ProcessRequest(_mockContext.Object, request);

            // Assert
            _mockContext.Verify(c => c.TransitionToState(It.IsAny<MovingState>()), Times.Once);
            _mockContext.Verify(c => c.ProcessRequest(request), Times.Once);
            Assert.Contains(request, onboardRequests);
            _mockElevator.VerifySet(e => e.CurrentCapacity = 3);
        }


        [Test]
        public async Task ExitState_CompletesTask()
        {
            // Act
            var task = _pickupState.ExitState(_mockContext.Object);

            // Assert
            Assert.IsTrue(task.IsCompletedSuccessfully);
        }
    }
}
