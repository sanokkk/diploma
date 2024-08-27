using Diploma.DTO.Reports;

namespace Diploma.Logic.Services.Interfaces;

public interface IReportService
{
    void GenerateReport(IReadOnlyCollection<UnitReport> units);
    Task GetLastReportAsync(Stream streamToCopy, CancellationToken cancellationToken);
}