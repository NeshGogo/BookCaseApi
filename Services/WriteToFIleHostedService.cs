using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace bookcaseApi.Services
{
    public class WriteToFIleHostedService : IHostedService
    {
        private readonly IHostEnvironment _environment;
        private readonly string _fileName = "file 1.txt";

        public WriteToFIleHostedService(IHostEnvironment environment)
        {
            _environment = environment;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            WriteToFile("WriteToFIleHostedService : Process Started");
            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            WriteToFile("WriteToFIleHostedService : Process Started");
            return Task.CompletedTask;
        }

        private void WriteToFile(string message)
        {
            throw new NotImplementedException();
        }
    }
}
