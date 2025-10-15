using DACS.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DACS.Services
{
    public class SocketServer
    {
        private readonly IServiceProvider _serviceProvider;

        public SocketServer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 8888);
            listener.Start();
            Console.WriteLine("🟢 Server đang lắng nghe tại cổng 8888...");

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client);
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            Console.WriteLine("🔗 Client đã kết nối.");
            using var stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int byteCount = await stream.ReadAsync(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, byteCount);

            Console.WriteLine($"📩 Nhận từ client: {message}");

            using (var scope = _serviceProvider.CreateScope())
            {
                var homeController = scope.ServiceProvider.GetRequiredService<HomeController>();
                var result = await homeController.Ask(message) as JsonResult;
    

                string response = (result?.Value as dynamic)?.response ?? "Lỗi khi xử lý chatbot.";
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
            }

            client.Close();
        }
    }
}
