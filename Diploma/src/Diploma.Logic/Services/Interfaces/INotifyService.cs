namespace Diploma.Logic.Services.Interfaces;

public interface INotifyService
{
    Task NotifyAsync(IReadOnlyCollection<string> messages, CancellationToken cancellationToken);
}