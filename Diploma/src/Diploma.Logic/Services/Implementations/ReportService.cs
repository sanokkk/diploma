using System.Text;
using ClosedXML.Excel;
using Diploma.DTO.Reports;
using Diploma.Logic.Services.Interfaces;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Diploma.Logic.Services.Implementations;

public sealed class ReportService : IReportService
{
    private const string ReportName = "Report.xlsx";
    private const string SheetName = "Вычисления";
    private readonly Dictionary<string, string> _columnNames = new()
    {
        ["A1"] = "Узел",
        ["B1"] = "Параметр",
        ["C1"] = "Значение",
        ["D1"] = "Формула",
        ["E1"] = "Индекс",
        
    };
    
    private const string UnitRangeTemplate = "A{0}:A{1}";
    private const string ParameterTemplate = "B{0}";
    private const string ValueTemplate = "C{0}";
    private const string FormulaRangeTemplate = "D{0}:D{1}";
    private const string IndexRangeTemplate = "E{0}:E{1}";

    private const string AColumn = "A";
    private const string BColumn = "B";
    private const string DColumn = "D";

    public void GenerateReport(IReadOnlyCollection<UnitReport> units)
    {
        var workBook = new XLWorkbook();
        var workSheet = workBook.Worksheets.Add(SheetName);
        
        SetColumnNames(workSheet, _columnNames);
        SetUnitsData(units, workSheet);
        
        workBook.SaveAs(ReportName);
    }

    public async Task GetLastReportAsync(Stream streamToCopy, CancellationToken cancellationToken)
    {
        await using var fs = new FileStream(ReportName, FileMode.Open, FileAccess.ReadWrite);

        await fs.CopyToAsync(streamToCopy, cancellationToken);
    }

    private void SetColumnNames(IXLWorksheet worksheet, Dictionary<string, string> columnNames)
    {
        foreach (var key in columnNames.Keys)
        {
            worksheet.Column(key[0]).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Column(key[0]).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            
            worksheet.Cell(key).Value = columnNames[key];
        }
    }

    private void SetUnitsData(IReadOnlyCollection<UnitReport> units, IXLWorksheet worksheet)
    {
        var aIndex = 2;
        var bIndex = 2;
        var cIndex = 2;
        var dIndex = 2;
        var eIndex = 2;

        var aLength = 0;
        var bLength = 0;
        var dLength = 0;
        

        foreach (var unit in units)
        {
            var endRow = aIndex + unit.Parameters.Count - 1;
            
            ProcessUnitInfo(worksheet, ref aIndex, endRow, unit, ref aLength);

            foreach (var parameter in unit.Parameters)
            {
                ProcessParameterName(worksheet, ref bIndex, parameter, ref bLength);

                ProcessParameterValue(worksheet, ref cIndex, parameter);
            }

            ProcessUnitFormula(worksheet, ref dIndex, endRow, unit, ref dLength);

            ProcessUnitIndex(worksheet, ref eIndex, endRow, unit);
        }

        SetColumnWidth(worksheet, aLength, bLength, dLength);
    }

    private static void SetColumnWidth(IXLWorksheet worksheet, int aLength, int bLength, int dLength)
    {
        worksheet.Column(AColumn).Width = aLength * 1.1;
        worksheet.Column(BColumn).Width = bLength * 1.1;
        worksheet.Column(DColumn).Width = dLength * 1.1;
        worksheet.Columns().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Columns().Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
    }

    private static void ProcessUnitInfo(IXLWorksheet worksheet, ref int aIndex, int endRow, UnitReport unit, ref int aLength)
    {
        var unitRange = worksheet.Range(string.Format(UnitRangeTemplate, aIndex, endRow));
        unitRange.Merge();
        unitRange.Value = unit.UnitName;

        aLength = Math.Max(aLength, unit.UnitName.Length);
        aIndex = endRow + 1;
    }

    private static void ProcessUnitIndex(IXLWorksheet worksheet, ref int eIndex, int endRow, UnitReport unit)
    {
        var indexRange = worksheet.Range(string.Format(IndexRangeTemplate, eIndex, endRow));
        indexRange.Merge();
        indexRange.Value = unit.Index;
        eIndex = endRow + 1;
    }

    private static void ProcessUnitFormula(IXLWorksheet worksheet, ref int dIndex, int endRow, UnitReport unit, ref int dLength)
    {
        var formulaRange = worksheet.Range(string.Format(FormulaRangeTemplate, dIndex, endRow));
        formulaRange.Merge();
        formulaRange.Value = unit.Formula;

        dLength = Math.Max(dLength, unit.Formula.Length);
        dIndex = endRow + 1;
    }

    private static void ProcessParameterValue(IXLWorksheet worksheet, ref int cIndex, ParameterReport parameter)
    {
        worksheet.Cell(string.Format(ValueTemplate, cIndex)).Value = parameter.Value;
        cIndex++;
    }

    private static void ProcessParameterName(IXLWorksheet worksheet, ref int bIndex, ParameterReport parameter, ref int bLength)
    {
        worksheet.Cell(string.Format(ParameterTemplate, bIndex)).Value = parameter.ParameterName;
        bLength = Math.Max(bLength, parameter.ParameterName.Length);
        bIndex++;
    }
}
