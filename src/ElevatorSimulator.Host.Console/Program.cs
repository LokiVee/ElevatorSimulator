using ElevatorSimulator.Application;
using ElevatorSimulator.Application.Features.Requests.Events;
using ElevatorSimulator.Domain;
using ElevatorSimulator.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ElevatorSimulator.Hosts.ConsoleApp;

internal class Program
{
    private static StatusUpdates _statusUpdates;
    private static IConfiguration _configuration;
    private static int _statusLineCount;
    private static int _menuLineCount;
    private static IMediator _mediator;
    private static IHost ApplicationHost;
    private static string _currentMenuText = "To request elevator enter the current floor you are on.";
    static async Task Main(string[] args)
    {
        _statusUpdates = new StatusUpdates();
        var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        _configuration = builder.Build();
        var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDomain().AddApplication(_statusUpdates, _configuration);

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
                    _currentMenuText = "To request elevator enter the current floor you are on.";
                    //Display the menu print
                    DisplayMenu();
                    //Display the handle input
                    await HandleInput();
                }
            }
            catch (OperationCanceledException cv)
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
        var floor = GetInput("floor");
        if (floor == null)
        {
            await Task.Delay(2000);
            return;
        }
        _currentMenuText = "Enter the floor you want to go to.";
        SetMenuText(_currentMenuText);

        var targetfloor = GetInput("floor");
        if (targetfloor == null)
        {
            await Task.Delay(2000); //Delay so person can see error message
            return;
        }
        _currentMenuText = "Enter the number of people waiting.";
        SetMenuText(_currentMenuText);

        var persons = GetInput("persons");
        if (persons == null)
        {
            await Task.Delay(2000); //Delay so person can see error message
            return;
        }

        var notification = new ElevatorRequestCreate(new Request
        {
            CurrentFloor = floor.Value,
            ObjectWaiting = persons.Value,
            TargetFloor = targetfloor.Value
        });

        await _mediator.Publish(notification);
    }

    private static void SetMenuText(string text)
    {
        Console.SetCursorPosition(0, _statusLineCount);
        Console.WriteLine(text.PadRight(Console.WindowWidth, ' '));
        Console.WriteLine("".PadRight(Console.WindowWidth, ' '));
        Console.SetCursorPosition(0, InputLineNumber());
    }

    private static int? GetInput(string inputDetail)
    {
        var input = Console.ReadLine();
        var appSettingMaxNumberOfFloor = _configuration["AppSettings:MaxNumberOfFloors"];
        var parseMaxNumberOfFloor = int.TryParse(appSettingMaxNumberOfFloor, out int maxFloors);
        var didParse = int.TryParse(input, out int currentFloor);
        if (!didParse)
        {
            Console.WriteLine($"{input} is not a valid {inputDetail} number");
            return null;
      
        }
        if (currentFloor >= maxFloors)
        {
            Console.WriteLine($"The {inputDetail} floor cannot be greater than {maxFloors}");
            return null;
        }

        return currentFloor;
    }

    

   
    private static void DisplayMenu()
    {
        SetMenuText(_currentMenuText);
        //Cleaning up all the other text in console
        Console.WriteLine("".PadRight(Console.WindowWidth, ' '));
        Console.WriteLine("".PadRight(Console.WindowWidth, ' '));
        Console.WriteLine("".PadRight(Console.WindowWidth, ' '));
        Console.WriteLine("".PadRight(Console.WindowWidth, ' '));
        Console.WriteLine("".PadRight(Console.WindowWidth, ' '));
        Console.WriteLine("".PadRight(Console.WindowWidth, ' '));
        Console.SetCursorPosition(0, InputLineNumber());
        _menuLineCount = 1;
    }


    private static SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);
    public class StatusUpdates : IApplicationFeedback
    {
        public Task StatusUpdated(IReadOnlyCollection<IElevator> elevators)
        {
            //We want to ensure that one thread at a time updates the UI or it will look like the application is glitching out
            _semaphoreSlim.Wait();
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

            _semaphoreSlim.Release();
            return Task.CompletedTask;
        }
    }
}
