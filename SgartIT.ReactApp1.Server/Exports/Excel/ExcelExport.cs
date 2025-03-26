using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore.Metadata;
using SgartIT.ReactApp1.Server.DTO;

namespace SgartIT.ReactApp1.Server.Exports.Excel;

public class ExcelExport(ILogger logger)
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/office/open-xml/spreadsheet/how-to-create-a-spreadsheet-document-by-providing-a-file-name?tabs=cs-0%2Ccs-100%2Ccs-1%2Ccs#sample-code
    /// https://learn.microsoft.com/en-us/office/open-xml/spreadsheet/how-to-insert-text-into-a-cell-in-a-spreadsheet?tabs=cs-1%2Ccs-2%2Ccs-3%2Ccs-4%2Ccs
    /// </summary>
    /// <param name="todos"></param>
    /// <returns></returns>
    public void Export(Stream strm, List<Todo> todos)
    {
        logger.LogDebug("Excel export ");
        try
        {

            using (SpreadsheetDocument spreadsheet = SpreadsheetDocument.Create(strm, SpreadsheetDocumentType.Workbook))
            {

                // Add a WorkbookPart to the document.
                WorkbookPart workbookPart = spreadsheet.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                // Get the SharedStringTablePart. If it does not exist, create a new one.
                SharedStringTablePart shareStringPart;
                if (workbookPart.GetPartsOfType<SharedStringTablePart>().Any())
                {
                    shareStringPart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
                }
                else
                {
                    shareStringPart = workbookPart.AddNewPart<SharedStringTablePart>();
                }

                // Insert a new worksheet.
                WorksheetPart worksheetPart = InsertWorksheet(workbookPart);

                uint row = 1;
                // header
                SetCellString(worksheetPart, 1, row, "Title", shareStringPart);
                SetCellString(worksheetPart, 2, row, "Completed", shareStringPart);
                SetCellString(worksheetPart, 3, row, "Category", shareStringPart);
                SetCellString(worksheetPart, 4, row, "Modified", shareStringPart);
                // rows
                foreach (var item in todos)
                {
                    row++;

                    SetCellString(worksheetPart, 1, row, item.Title, shareStringPart);
                    SetCellString(worksheetPart, 2, row, item.IsCompleted ? "Yes": "No", shareStringPart);
                    SetCellString(worksheetPart, 3, row, item.Category , shareStringPart);
                    SetCellString(worksheetPart, 4, row, item.Modified.ToString("dd/MM/yyyy"), shareStringPart);

                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Items count {count}", todos?.Count);
            throw;
        }
    }

    void SetCellString(WorksheetPart worksheetPart, int columnIndex, uint rowIndex, string? text, SharedStringTablePart shareStringPart)
    {
        string collNames = GetColumn(columnIndex);
        // Insert the text into the SharedStringTablePart.
        uint index = InsertSharedStringItem(text, shareStringPart);


        // Insert cell A1 into the new worksheet.
        Cell cell = InsertCellInWorksheet(collNames, rowIndex, worksheetPart);

        // Set the value of cell A1.
        cell.CellValue = new CellValue(index.ToString());
        cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
    }

    const string cellNames = "ABCDEFGHIGKLMNOPURSTUVWXYZ";
    readonly static int cellLen = cellNames.Length;

    static string GetColumn(int columnIndex)
    {
        int i = columnIndex - 1;
        if (i < cellLen)
        {
            return cellNames[i].ToString();
        }

        int i1 = columnIndex / cellLen;
        int i2 = columnIndex % cellLen;

        return cellNames[i1].ToString() + cellNames[i2].ToString();
    }

    // Given text and a SharedStringTablePart, creates a SharedStringItem with the specified text 
    // and inserts it into the SharedStringTablePart. If the item already exists, returns its index.
    static uint InsertSharedStringItem(string? text, SharedStringTablePart shareStringPart)
    {
        string searchText = text ?? string.Empty;

        // If the part does not contain a SharedStringTable, create one.
        shareStringPart.SharedStringTable ??= new SharedStringTable();

        uint i = 0;

        // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
        foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
        {
            if (item.InnerText == searchText)
            {
                return i;
            }

            i++;
        }

        // The text does not exist in the part. Create the SharedStringItem and return its index.
        shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new Text(searchText)));

        return i;
    }

    // Given a WorkbookPart, inserts a new worksheet.
    private static WorksheetPart InsertWorksheet(WorkbookPart workbookPart)
    {
        // Add a new worksheet part to the workbook.
        WorksheetPart newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        newWorksheetPart.Worksheet = new Worksheet(new SheetData());

        Sheets sheets = workbookPart.Workbook.GetFirstChild<Sheets>() ?? workbookPart.Workbook.AppendChild(new Sheets());
        string relationshipId = workbookPart.GetIdOfPart(newWorksheetPart);

        // Get a unique ID for the new sheet.
        uint sheetId = 1;
        if (sheets.Elements<Sheet>().Any())
        {
            sheetId = sheets.Elements<Sheet>().Select<Sheet, uint>(s =>
            {
                if (s.SheetId is not null && s.SheetId.HasValue)
                {
                    return s.SheetId.Value;
                }

                return 0;
            }).Max() + 1;
        }

        string sheetName = "Sheet" + sheetId;

        // Append the new worksheet and associate it with the workbook.
        Sheet sheet = new()
        {
            Id = relationshipId,
            SheetId = sheetId,
            Name = sheetName
        };
        sheets.Append(sheet);

        return newWorksheetPart;
    }


    // Given a column name, a row index, and a WorksheetPart, inserts a cell into the worksheet. 
    // If the cell already exists, returns it. 
    private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
    {
        Worksheet worksheet = worksheetPart.Worksheet;
        SheetData? sheetData = worksheet.GetFirstChild<SheetData>();
        string cellReference = columnName + rowIndex;

        // If the worksheet does not contain a row with the specified row index, insert one.
        Row row;

        if (sheetData?.Elements<Row>().Where(r => r.RowIndex is not null && r.RowIndex == rowIndex).Count() != 0)
        {
            row = sheetData!.Elements<Row>().Where(r => r.RowIndex is not null && r.RowIndex == rowIndex).First();
        }
        else
        {
            row = new Row() { RowIndex = rowIndex };
            sheetData.Append(row);
        }

        // If there is not a cell with the specified column name, insert one.  
        if (row.Elements<Cell>().Where(c => c.CellReference is not null && c.CellReference.Value == columnName + rowIndex).Any())
        {
            return row.Elements<Cell>().Where(c => c.CellReference is not null && c.CellReference.Value == cellReference).First();
        }
        else
        {
            // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
            Cell? refCell = null;

            foreach (Cell cell in row.Elements<Cell>())
            {
                if (string.Compare(cell.CellReference?.Value, cellReference, true) > 0)
                {
                    refCell = cell;
                    break;
                }
            }

            Cell newCell = new()
            {
                CellReference = cellReference
            };
            row.InsertBefore(newCell, refCell);

            return newCell;
        }
    }
}
