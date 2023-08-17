using CryptoProvider.Contracts.Clients;
using CryptoProvider.KuCoin.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SwingTrader9000.Cmd
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
               .Build();

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.ConfigureCryptoProviderServices(configuration);
                })
                .Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;
                var cryptoClient = serviceProvider.GetRequiredService<ICryptoClient>();
                var result = await cryptoClient.GetInitialWebSocketDataAsync(CancellationToken.None);
                Console.WriteLine(result);
            }

            //using var client = new ClientWebSocket();
            //await client.ConnectAsync(new Uri("ws://your-websocket-server-url"), CancellationToken.None);

            //Console.WriteLine("Connected!");

            //var sendTask = SendAsync(client);
            //var receiveTask = ReceiveAsync(client);

            //await Task.WhenAll(sendTask, receiveTask);
        }

        //private static async Task SendAsync(ClientWebSocket client)
        //{
        //    while (client.State == WebSocketState.Open)
        //    {
        //        Console.WriteLine("Enter a message to send to the server:");
        //        var messageToSend = Console.ReadLine();

        //        if (string.IsNullOrEmpty(messageToSend))
        //        {
        //            Console.WriteLine("Empty input. Type a message or close the app.");
        //            continue;
        //        }

        //        var buffer = Encoding.UTF8.GetBytes(messageToSend);
        //        await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        //    }
        //}

        //private static async Task ReceiveAsync(ClientWebSocket client)
        //{
        //    var buffer = new byte[1024 * 4];

        //    while (client.State == WebSocketState.Open)
        //    {
        //        var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        //        if (result.MessageType == WebSocketMessageType.Text)
        //        {
        //            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
        //            Console.WriteLine($"Received: {message}");
        //        }
        //        else if (result.MessageType == WebSocketMessageType.Close)
        //        {
        //            Console.WriteLine("Connection closed.");
        //            break;
        //        }
        //    }
        //}
    }
}
