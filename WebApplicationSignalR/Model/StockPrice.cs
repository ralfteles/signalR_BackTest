using System;

namespace WebApplicationSignalR.Model
{
    public class StockPrice
    {
        public string Symbol { get; private set; }
        public double Price { get; private set; }
        public string DataConexao { get; set; }

        public StockPrice(string symbol, double price)
        {
            Symbol = symbol;
            Price = price;
            DataConexao = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
}
