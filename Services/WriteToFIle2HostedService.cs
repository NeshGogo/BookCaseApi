using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace bookcaseApi.Services
{
    public class WriteToFile2HostedService : IHostedService, IDisposable
    {
        private readonly IHostEnvironment _environment;
        private readonly string _fileName = "File 2.txt";
        private Timer timer;

        public WriteToFile2HostedService(IHostEnvironment environment)
        {
            
            _environment = environment;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            WriteToFile($"WriteToFile2HostedService 2 : Process Started {DateTime.Now}");
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(18));
  
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            WriteToFile($"WriteToFile2HostedService 2 : Process working {DateTime.Now}");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            WriteToFile($"WriteToFile2HostedService 2 : Process Stop {DateTime.Now}");
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
