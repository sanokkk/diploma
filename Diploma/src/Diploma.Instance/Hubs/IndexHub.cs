using Microsoft.AspNetCore.SignalR;

namespace Diploma.Instance.Hubs;

public class IndexHub : Hub
{
    public async Task SendIndex()
    {
        Console.WriteLine("Начинаю выполнение");
        await Clients.All.SendAsync("35");
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("ReceiveMessage", $"Пiдключився клиент {Context.ConnectionId}");
        Console.WriteLine("Пiдключився client");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"{Context.ConnectionId} вышел");
        await Clients.All.SendAsync("ReceiveMessage", "disconnected");
    }

    public async Task Send()
    {
        Console.WriteLine("finally sended");
        await Clients.All.SendAsync("ReceiveMessage", $"some message from ");
    }
}
