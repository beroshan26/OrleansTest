using Interfaces;
using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using Stocks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Client";

            var config = ClientConfiguration.LoadFromFile("ClientConfiguration.xml");

            while (true)
            {
                try
                {
                    GrainClient.Initialize(config);
                    Console.WriteLine("Connected to silo!");

                    var friend = GrainClient.GrainFactory.GetGrain<IPerson>("Joe");
                    var result = friend.SayHelloAsync("Hello!");

                    var stock = GrainClient.GrainFactory.GetGrain<IStockGrain>("MSFT");
                    var stockResult = stock.GetPrice();
                    Console.ReadLine();
                    //break;
                }
                catch (SiloUnavailableException ex)
                {
                    Console.WriteLine("Silo not available! Retrying in 3 seconds.");
                    Thread.Sleep(3000);
                }
            }
        }
    }
}
