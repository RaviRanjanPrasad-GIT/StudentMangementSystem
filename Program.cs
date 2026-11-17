using Microsoft.Extensions.DependencyInjection;
using StudentManagement.ConsoleApp.Interfaces;
using StudentManagement.ConsoleApp.Presentations;
using StudentManagement.ConsoleApp.Repositories;
using StudentManagement.ConsoleApp.Services;

var services = new ServiceCollection();
services.AddSingleton<IStudentRepository, StudentRepository>();
services.AddSingleton<IStudentService, StudentService>();
services.AddSingleton<ConsoleMenu>();

using var serviceProvider = services.BuildServiceProvider();
serviceProvider.GetRequiredService<ConsoleMenu>().Run();
