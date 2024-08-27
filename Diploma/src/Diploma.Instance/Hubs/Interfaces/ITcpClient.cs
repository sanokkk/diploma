namespace Diploma.Instance.Hubs.Interfaces;

public interface ITcpClient
{
    Task GetConditionData(string data);
}
