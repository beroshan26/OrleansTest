using Interfaces;
using Orleans;
using Grains;
using Orleans.Runtime.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Stocks.Interfaces;

namespace Silo
{
    class Program
    {
        static SiloHost siloHost;

        static void Main(string[] args)
        {
            Console.Title = "Silo";

            // Orleans should run in its own AppDomain, we set it up like this
            AppDomain hostDomain = AppDomain.CreateDomain("OrleansHost", null,
                new AppDomainSetup()
                {
                    AppDomainInitializer = InitSilo
                });

            Console.WriteLine("Orleans Silo is running.\nPress Enter to terminate...");
            Console.ReadLine();

            // We do a clean shutdown in the other AppDomain
            hostDomain.DoCallBack(ShutdownSilo);
        }

        static void InitSilo(string[] args)
        {
            siloHost = new SiloHost(Dns.GetHostName());
            siloHost.LoadOrleansConfig();

            siloHost.InitializeOrleansSilo();
            var startedok = siloHost.StartOrleansSilo();
            if (!startedok)
                throw new SystemException(String.Format("Failed to start Orleans silo '{0}' as a {1} node", siloHost.Name, siloHost.Type));
        }

        static void ShutdownSilo()
        {
            if (siloHost != null)
            {
                siloHost.Dispose();
                GC.SuppressFinalize(siloHost);
                siloHost = null;
            }
        }

        //private static async Task<int> RunMainAsync()
        //{
        //    try
        //    {
        //        var host = await StartSilo();
        //        Console.WriteLine("Press Enter to terminate...");
        //        var client = host.Services.GetRequiredService<IClusterClient>();

        //        var stockGrain = client.GetGrain<IStockGrain>("MSFT");

        //        var price = await stockGrain.GetPrice();

        //        Console.WriteLine("Price is \n{0}", price);

        //        Console.ReadLine();

        //        await host.StopAsync();

        //        return 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        return 1;
        //    }
        //}

        //private static async Task<ISiloHost> StartSilo()
        //{
        //    // define the cluster configuration
        //    var builder = new SiloHostBuilder()
        //        .UseLocalhostClustering()
        //        .Configure<ClusterOptions>(options =>
        //        {
        //            options.ClusterId = "dev";
        //            options.ServiceId = "StocksSampleApp";
        //        })
        //        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(StockGrain).Assembly).WithReferences())
        //        .ConfigureLogging(logging => logging.AddConsole())
        //        .EnableDirectClient();

        //    var host = builder.Build();
        //    await host.StartAsync();
        //    return host;
        //}
    }
}
