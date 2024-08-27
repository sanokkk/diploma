using System.Text.Json.Serialization;

namespace Diploma.DTO.Tcp.Responses;

public sealed record ConditionResponse(IReadOnlyCollection<UnitResponse> Units);
