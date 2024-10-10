using ElevatorSimulator.Application;
using ElevatorSimulator.Application.ElevatorApplication;
using ElevatorSimulator.Application.ElevatorApplication.StateContext;
using ElevatorSimulator.Domain.Entities;
using ElevatorSimulator.Domain.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator.Tests
{
    internal class OrchestratorTests
    {
        private Orchestrator _orchestrator;
        private Mock<IApplicationFeedback> _mockApplicationFeedback;
        private Mock<IElevatorStateContext> _mockElevatorStateContext1;
        private Mock<IElevatorStateContext> _mockElevatorStateContext2;
        private IElevator _elevator1;
        private IElevator _elevator2;

        [SetUp]
        public void Setup()
        {

            _mockApplicationFeedback = new Mock<IApplicationFeedback>();
            _orchestrator = new Orchestrator(_mockApplicationFeedback.Object);

            _elevator1 = new Mock<IElevator>().Object;
            _elevator2 = new Mock<IElevator>().Object;

            _mockElevatorStateContext1 = new Mock<IElevatorStateContext>();
            _mockElevatorStateContext2 = new Mock<IElevatorStateContext>();

            _orchestrator._elevators.Add(_elevator1);
            _orchestrator._elevators.Add(_elevator2);

            _orchestrator._elevatorContext[_elevator1] = _mockElevatorStateContext1.Object;
            _orchestrator._elevatorContext[_elevator2] = _mockElevatorStateContext2.Object;
        }

        [Test]
        public async Task GivenRequestOnGroundFloor_Elevator2ReturnedAsBest()
        {

            // Arrange
            var request = new Request { CurrentFloor = 3, TargetFloor = 7 };

            _mockElevatorStateContext1.Setup(c => c.CanHandleRequest(request)).Returns(true);
            _mockElevatorStateContext2.Setup(c => c.CanHandleRequest(request)).Returns(true);

            var mockElevator1 = Mock.Get(_elevator1);
            var mockElevator2 = Mock.Get(_elevator2);

            mockElevator1.SetupGet(e => e.CurrentFloor).Returns(2);
            mockElevator1.SetupGet(e => e.Status).Returns(ElevatorStatus.Idle);

            mockElevator2.SetupGet(e => e.CurrentFloor).Returns(5);
            mockElevator2.SetupGet(e => e.Status).Returns(ElevatorStatus.Idle);

            // Act
            var bestElevator = _orchestrator.FindBestElevator(request);

            // Assert
            Assert.AreEqual(_elevator1, bestElevator);
        }
    }
}
