namespace Diploma.DTO.Tcp.Models;

public sealed record TcpMessage(
    string Name,
    MessageParameter?[] Parameters);