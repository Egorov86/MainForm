using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class NetworkManager
{
    private TcpListener _server;
    private TcpClient _client;
    private NetworkStream _stream;

    public async Task StartServer(int port)
    {
        _server = new TcpListener(IPAddress.Any, port);
        _server.Start();
        Console.WriteLine("Сервер запущен...");
        _client = await _server.AcceptTcpClientAsync();
        Console.WriteLine("Клиент подключен.");
        _stream = _client.GetStream();
    }

    public async Task ConnectToServer(string ipAddress, int port)
    {
        _client = new TcpClient();
        await _client.ConnectAsync(ipAddress, port);
        Console.WriteLine("Подключено к серверу.");
        _stream = _client.GetStream();
    }

    public async Task<string> ReadMessage()
    {
        byte[] buffer = new byte[1024];
        int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
        return Encoding.UTF8.GetString(buffer, 0, bytesRead);
    }

    public async Task SendMessage(string message)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        await _stream.WriteAsync(buffer, 0, buffer.Length);
    }

    public void Stop()
    {
        _stream?.Close();
        _client?.Close();
        _server?.Stop();
    }

}
