using ElevatorSimulator.Application.ElevatorApplication.StateContext;
using ElevatorSimulator.Application.ElevatorApplication.StateContext.States;
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
    public class MovingStateTests
    {
        private Mock<IElevatorStateContext> _mockContext;
        private Mock<IElevator> _mockElevator;
        private int _currentFloor;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<IElevatorStateContext>();
            _mockElevator = new Mock<IElevator>();
            _currentFloor = 1; // Initial floor
            _mockContext.SetupGet(c => c.Elevator).Returns(_mockElevator.Object);
        }


        [TestCase(1, 5)]
        [TestCase(2, 6)]
        [TestCase(3, 7)]
        public async Task ProcessRequest_MovesElevatorUpToTargetFloor(int initialFloor, int targetFloor)
        {
            _mockElevator.SetupGet(e => e.CurrentFloor).Returns(() => _currentFloor);
            _mockElevator.SetupSet(e => e.CurrentFloor = It.IsAny<int>()).Callback<int>(value => _currentFloor = value);

            // Arrange
            _currentFloor = initialFloor;
            var state = new MovingState(ElevatorStatus.MovingUp, targetFloor, false);
            var request = new Request { CurrentFloor = targetFloor };

            // Act
            await state.ProcessRequest(_mockContext.Object, request);

            // Assert
            Assert.AreEqual(targetFloor, _currentFloor);
        }


        [TestCase(5, 1)]
        [TestCase(6, 2)]
        [TestCase(7, 3)]
        public async Task ProcessRequest_MovesElevatorDownToTargetFloor(int initialFloor, int targetFloor)
        {
            _mockElevator.SetupGet(e => e.CurrentFloor).Returns(() => _currentFloor);
            _mockElevator.SetupSet(e => e.CurrentFloor = It.IsAny<int>()).Callback<int>(value => _currentFloor = value);
            // Arrange
            _currentFloor = initialFloor;
            var state = new MovingState(ElevatorStatus.MovingDown, targetFloor, false);
            var request = new Request { CurrentFloor = targetFloor };

            // Act
            await state.ProcessRequest(_mockContext.Object, request);

            // Assert
            Assert.AreEqual(targetFloor, _currentFloor);
        }


        [Test]
        public async Task ProcessRequest_TransitionsToPickUpState()
        {
            // Arrange
            _mockElevator.SetupGet(e => e.CurrentFloor).Returns(() => _currentFloor);
            _mockElevator.SetupSet(e => e.CurrentFloor = It.IsAny<int>()).Callback<int>(value => _currentFloor = value);
            _currentFloor = 1;
            var targetFloor = 5;
            var state = new MovingState(ElevatorStatus.MovingUp, targetFloor, true);
            var request = new Request { CurrentFloor = targetFloor };
            var mockPickUpState = new Mock<IState>();

            _mockContext.Setup(c => c.TransitionToState(It.IsAny<IState>()))
                        .Callback<IState>(newState =>
                        {
                            if (newState is PickupState)
                            {
                                mockPickUpState.Object.EnterState(_mockContext.Object);
                            }
                        });

            // Act
            await state.ProcessRequest(_mockContext.Object, request);

            // Assert
            Assert.AreEqual(targetFloor, _currentFloor);
            mockPickUpState.Verify(s => s.EnterState(_mockContext.Object), Times.Once);
        }
    }
    
}
