using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
//Клиент
class Program_1
{
    static async Task Main(string[] args)
    {
        UdpClient udpClient = new UdpClient();
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Loopback, 11000);

        Console.WriteLine("Введите продукты через запятую (например, 'паста, курица') или 'exit' для выхода:");

        while (true)
        {
            string input = Console.ReadLine();
            if (input.ToLower() == "exit")
                break;

            byte[] requestBytes = Encoding.UTF8.GetBytes(input);
            await udpClient.SendAsync(requestBytes, requestBytes.Length, remoteEndPoint);

            var receivedResults = await udpClient.ReceiveAsync();
            string response = Encoding.UTF8.GetString(receivedResults.Buffer);
            Console.WriteLine($"Рецепты: {response}");
        }
    }
}
