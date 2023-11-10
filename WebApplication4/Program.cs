using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApplication4.Models;    // ������������ ���� ������ ApplicationContext


var builder = WebApplication.CreateBuilder(args);

// �������� ������ ����������� �� ����� ������������
string connection = builder.Configuration.GetConnectionString("DefaultConnection");

// ��������� �������� ApplicationContext � �������� ������� � ����������
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
