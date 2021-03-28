using bookcaseApi.Contexts;
using bookcaseApi.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace bookcaseApi.Services
{
    public class WriteToFileHostedService : IHostedService, IDisposable
    {
        public IServiceProvider Services { get; }
        private readonly IHostEnvironment _environment;
        private readonly string _fileName = "File 1.txt";
        private Timer timer;

        public WriteToFileHostedService(IHostEnvironment environment, IServiceProvider services)
        {
            Services = services;   
            _environment = environment;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            WriteToFile($"WriteToFIleHostedService 1 : Process Started {DateTime.Now}");
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            WriteToFile($"WriteToFIleHostedService 1 : Process working {DateTime.Now}");
            
            // Asi es como creamos una tarea recurrente con entitiframework
            /*using (var scope = Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BookCaseDbContext>();
                var book = new Book
                {
                    Title = "Harry Potter",
                    AuthorId = 1
                };
                context.Books.Add(book);
                context.SaveChanges();
            }*/
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            WriteToFile($"WriteToFIleHostedService 1 : Process Stop {DateTime.Now}");
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private void WriteToFile(string message)
        {
            var path = $@"{_environment.ContentRootPath}\Statics\{_fileName}";
            using (StreamWriter writer = new StreamWriter(path, append: true))
            {
                writer.WriteLine(message);
            }
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
