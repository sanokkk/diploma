namespace Diploma.DTO.Reports;

public sealed record UnitReport(
    string UnitName,
    IReadOnlyCollection<ParameterReport> Parameters,
    string Formula,
    int Index);
