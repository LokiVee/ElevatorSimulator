using ElevatorSimulator.Application;
using ElevatorSimulator.Application.Features.Requests.Commands;
using ElevatorSimulator.Application.Features.Requests.Events;
using ElevatorSimulator.Domain;
using ElevatorSimulator.Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ElevatorSimulator.Hosts.ConsoleApp;

internal class Program
{
    private static StatusUpdates _statusUpdates;
    private static int _statusLineCount;
    private static int _menuLineCount;
    private static IMediator _mediator;
    private static IHost ApplicationHost;
    static async Task Main(string[] args)
    {
        _statusUpdates = new StatusUpdates();
        var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDomain().AddApplication(_statusUpdates); 
                    
                });
        using (var cts = new CancellationTokenSource())
        {
            ApplicationHost = hostBuilder.Build();
            _mediator = ApplicationHost.Services.GetRequiredService<IMediator>();

            await ApplicationHost.StartAsync();
            try
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    //Do the menu print
                    DisplayMenu();
                    //Do the handle input
                    _ = HandleInput();
                    //Do the clear menu section 
                }
            }
            catch (OperationCanceledException)
            {
                // This will be thrown when the cancellation token is set and the loop is broken  
            }
            finally
            {
                await ApplicationHost.StopAsync();
            }
            
        }
        ApplicationHost.Dispose();
        Console.WriteLine("Application has been stopped.");
    }

    private static int InputLineNumber() => _statusLineCount + _menuLineCount;

    private static async Task HandleInput()
    {
        var input = Console.ReadLine();
        switch (input)
        {
            case "1":
                //Did this with some menu thing
                var notificaiton = new ElevatorRequestCreate(new Request
                {
                    CurrentFloor = -1,
                    TargetFloor = 6,
                    ObjectWaiting = 5
                });
                await _mediator.Publish(notificaiton);
                break;
            case "2":
                //Did this with some menu thing
                var notificaiton1 = new ElevatorRequestCreate(new Request
                {
                    CurrentFloor = 4,
                    TargetFloor = 6,
                    ObjectWaiting = 2
                });
                await _mediator.Publish(notificaiton1);
                break;
        }
    }

    private static void DisplayMenu()
    {
        Console.SetCursorPosition(0, _statusLineCount);
        Console.WriteLine("Your menu options here now.".PadRight(Console.WindowWidth, ' '));
        Console.WriteLine("1. Add Request".PadRight(Console.WindowWidth, ' '));
        Console.WriteLine("".PadRight(Console.WindowWidth, ' '));
        _menuLineCount = 2;
    }

    public class StatusUpdates : IApplicationFeedback
    {
        public Task StatusUpdated(IReadOnlyCollection<Elevator> elevators)
        {
            Console.SetCursorPosition(0, 0);
            foreach (var elevator in elevators)
            {
                var message = elevator.ToString().PadRight(Console.WindowWidth, ' ');
                Console.WriteLine(message);
            }
            Console.WriteLine("".PadRight(Console.WindowWidth, '-'));
            _statusLineCount = elevators.Count() + 1; //Give one empty line after status for input
            //Do my status updated
            //Can do cursor lines and everything in here
            DisplayMenu();
            Console.SetCursorPosition(0, InputLineNumber()); //Make sure the input line number is back in the correct line after update
            return Task.CompletedTask;
        }
    }
}
