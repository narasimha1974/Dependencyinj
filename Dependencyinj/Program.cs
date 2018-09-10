using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System;

public class Program
{
    public static void Main(string[] args)
    {
        //setup our DI
        ServiceProvider serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddSingleton<IBarService, BarService>()
            .AddSingleton<IFooService, FooService>()              
            .BuildServiceProvider();

        //configure console logging
        serviceProvider
            .GetService<ILoggerFactory>()      
            .AddConsole();//LogLevel.Debug    
        
        ILogger logger = serviceProvider.GetService<ILoggerFactory>()
            .CreateLogger<Program>();

        logger.LogInformation("Starting application");

        //do the actual work here
        IBarService bar = serviceProvider.GetService<IBarService>();
        bar.DoSomeRealWork();

        logger.LogInformation("All done!");

        Console.ReadLine();

    }
}


public interface IFooService
{
    void DoThing(int number);
}

public interface IBarService
{
    void DoSomeRealWork();
}

public class BarService : IBarService
{

    private readonly IFooService _fooService;
    public BarService(IFooService fooService)
    {
        _fooService = fooService;
    }

    public void DoSomeRealWork()
    {
        for (int i = 0; i < 10; i++)
        {
            _fooService.DoThing(i);            
        }
    }
}

public class FooService : IFooService
{
    private readonly ILogger<FooService> _logger;

    public FooService(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<FooService>();
    }

    public void DoThing(int number)
    {
        _logger.LogInformation($"Doing the thing {number}");
    }
}