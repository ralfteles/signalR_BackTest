

using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalRTeste.Servidor
{
    public class RealTimeBroker : Hub
    {
        public Task ConnectToSock(string symbol)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, symbol);

            return Task.CompletedTask;
        }
    }
}
