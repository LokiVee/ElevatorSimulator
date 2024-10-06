using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ElevatorSimulator.Tests
{
    //public class ElevatorStateTests
    //{
    //    private Mock<ElevatorStateContext> _contextMock;

    //    [SetUp]
    //    public void Setup()
    //    {
    //        _contextMock = new Mock<ElevatorStateContext>(MockBehavior.Strict);
    //    }

    //    [Test]
    //    public async Task IdleState_TransitionsToPickupState_WhenRequestReceived()
    //    {
    //        Arrange
    //       var elevator = new NormalElevator { Name = "1" };
    //        var idleState = new IdleState();
    //        var request = new Request { CurrentFloor = 1, TargetFloor = 5, ObjectWaiting = 1 };

    //        _contextMock.Setup(ctx => ctx.ProcessRequest(It.IsAny<Request>())).Returns(Task.CompletedTask);
    //        _contextMock.Setup(ctx => ctx.TransitionToState(It.IsAny<IState>())).Callback<IState>(state =>
    //        {
    //            Assert.IsInstanceOf<PickupState>(state); // Assert that we transitioned to PickupState
    //        });

    //        Act
    //       await idleState.ProcessRequest(_contextMock.Object, request);

    //        Assert
    //        _contextMock.Verify(ctx => ctx.ProcessRequest(request), Times.Once);
    //    }
    //}
}
