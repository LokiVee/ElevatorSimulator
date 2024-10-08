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
    //internal class IdleStateFlowTests
    //{
    //    private Mock<IElevatorStateContext> _mockContext;
    //    private Mock<IElevator> _mockElevator;
    //    private IdleState _idleState;
        
    //    [SetUp]
    //    public void Setup()
    //    {
    //        _mockContext = new Mock<IElevatorStateContext>();
    //        _mockElevator = new Mock<IElevator>();

    //        _mockContext.SetupGet(c => c.Elevator).Returns(_mockElevator.Object);
    //        _idleState = new IdleState();
    //    }

    //    [Test]
    //    public async Task When_EnterState_ShouldBeStatus_Idle()
    //    {
    //        _mockElevator.Object.Status = Domain.Enums.ElevatorStatus.MovingUp;
    //        await _idleState.EnterState(_mockContext.Object);

    //        Assert.That(_mockElevator.Object.Status == Domain.Enums.ElevatorStatus.Idle, "Enter state for Idle state did not update the elevator object");
    //    }
    //}
}
