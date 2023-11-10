using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApplication4.Models;    // пространство имен класса ApplicationContext


var builder = WebApplication.CreateBuilder(args);

// получаем строку подключения из файла конфигурации
string connection = builder.Configuration.GetConnectionString("DefaultConnection");

// добавляем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

builder.Services.AddControllersWithViews();


var app = builder.Build();
Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Debug()
           .WriteTo.Console()
           .WriteTo.File("logFile.txt", rollingInterval: RollingInterval.Day)
           .CreateLogger();



Log.CloseAndFlush();
app.MapDefaultControllerRoute();


app.Run();
