using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
//Сервер
class Program
{
    private static Dictionary<string, List<string>> _recipes = new Dictionary<string, List<string>>()
    {
        { "паста", new List<string> { "Паста с томатным соусом", "Паста с грибами" } },
        { "курица", new List<string> { "Курица с картошкой", "Курица в сливочном соусе" } },
        { "рыба", new List<string> { "Запеченная рыба", "Рыба на гриле" } }
    };

    static async Task Main(string[] args)
    {
        UdpClient udpServer = new UdpClient(11000);
        Console.WriteLine("Сервер запущен...");

        while (true)
        {
            var receivedResults = await udpServer.ReceiveAsync();
            string request = Encoding.UTF8.GetString(receivedResults.Buffer);
            Console.WriteLine($"Получен запрос: {request}");

            string response = GetRecipes(request);
            byte[] responseBytes = Encoding.UTF8.GetBytes(response);

            await udpServer.SendAsync(responseBytes, responseBytes.Length, receivedResults.RemoteEndPoint);
        }
    }

    private static string GetRecipes(string ingredients)
    {
        var ingredientList = ingredients.ToLower().Split(',');
        var foundRecipes = new List<string>();

        foreach (var ingredient in ingredientList)
        {
            if (_recipes.ContainsKey(ingredient.Trim()))
            {
                foundRecipes.AddRange(_recipes[ingredient.Trim()]);
            }
        }

        return foundRecipes.Count > 0 ? string.Join(", ", foundRecipes) : "Рецепты не найдены.";
    }
}
