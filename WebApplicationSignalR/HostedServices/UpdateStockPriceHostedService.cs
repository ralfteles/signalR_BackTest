using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApplicationSignalR.Hubs;
using WebApplicationSignalR.Model;

namespace WebApplicationSignalR.HostedServices
{
    public class UpdateStockPriceHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly ILogger _logger;
        public IServiceProvider Services { get; }
        private readonly List<string> _stocks;
        public UpdateStockPriceHostedService(IServiceProvider services, ILogger<UpdateStockPriceHostedService> logger)
        {
            _logger = logger;

            Services = services;
            _stocks = new List<string>
            {
                "ITSA4",
                "TAEE11",
                "PETR4"
            };
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdatePrices, null, 0, 3000);

            return Task.CompletedTask;
        }

        private void UpdatePrices(object state)
        {
            _logger.LogInformation($"Executando tarefa - {DateTime.Now}----------------");

            using (var scope = Services.CreateScope())
            {
                // Obtenho a instância do IHubContext, para permitir interagir com os Hubs e as conexões dos grupos.
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<BrokerHub>>();

                // Para cada ação da lista eu gero um número aleatório entre 5 e 30, e então notifico o grupo do Hub dessa ação sobre o novo objeto que contém o valor.
                foreach (var stock in _stocks)
                {
                    var stockPrice = GetRandomNumber(5, 30);

                    _logger.LogInformation($"Valores -{stockPrice}");


                    hubContext.Clients.Group(stock).SendAsync("UpdatePrice", new StockPrice(stock, stockPrice));
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private double GetRandomNumber(double minimum, double maximum)
        {
            var random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
