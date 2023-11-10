//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using Microsoft.EntityFrameworkCore;
//using WebApplication4.Models;
//using Serilog;
//using Serilog.Events;

//public class Startup
//{
//    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//    {
//        // ...

//        // Добавляем Serilog в качестве провайдера логгирования
//        app.UseSerilogRequestLogging();

//        // Применяем Serilog в конвейер обработки запросов
//        app.UseSerilogRequestLogging();

//        // ...
//    }

//    public void ConfigureServices(IServiceCollection services)
//    {
//        // Другие настройки сервисов

//        // Добавить настройку логирования
//        Log.Logger = new LoggerConfiguration()
//     .WriteTo.File("logFile.txt", restrictedToMinimumLevel: LogEventLevel.Verbose)
//     .CreateLogger();


//        services.AddControllersWithViews();
//        services.AddLogging(loggingBuilder =>
//        {
//            loggingBuilder.ClearProviders();
//            loggingBuilder.AddSerilog(Log.Logger);
//        });
//    }
//}