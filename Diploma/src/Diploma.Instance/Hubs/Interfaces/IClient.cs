namespace Diploma.Instance.Hubs.Interfaces;

public interface IClient
{
    Task ReceiveMessage(string message);
}