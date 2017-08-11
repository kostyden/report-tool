namespace ReportTool
{
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [SupportedFileExtension(".xlsx")]
    public class ExcelDataReader : IDataReader
    {
        public DataResult Read(string path)
        {
            var data = new List<Dictionary<string, double>>();
            using (var document = SpreadsheetDocument.Open(path, false))
            {
                var sheet = document.WorkbookPart.WorksheetParts.First();
                var sharedStrings = new List<string>();
                using (var reader = OpenXmlReader.Create(document.WorkbookPart.SharedStringTablePart))
                {
                    while (reader.Read())
                    {
                        if (reader.ElementType == typeof(SharedStringItem))
                        {
                            var item = (SharedStringItem)reader.LoadCurrentElement();
                            sharedStrings.Add(item.Text.Text);
                        }
                    }
                }

                var columns = new Dictionary<string, string>();
                using (var reader = OpenXmlReader.Create(sheet))
                {
                    Dictionary<string, double> currentRow = null;
                    while (reader.Read())
                    {
                        if (reader.ElementType == typeof(Cell))
                        {
                            var cell = (Cell)reader.LoadCurrentElement();
                            var columnHeader = new string(cell.CellReference.Value.TakeWhile(Char.IsLetter).ToArray());

                            if (cell.DataType != null && cell.DataType == CellValues.SharedString)
                            {
                                int.TryParse(cell.CellValue.Text, out int textId);

                                columns.Add(columnHeader, sharedStrings.ElementAt(textId));
                            }
                            else
                            {
                                if (columnHeader.Equals("A"))
                                {
                                    currentRow = new Dictionary<string, double>();
                                    data.Add(currentRow);
                                }

                                double.TryParse(cell.CellValue.Text, out double value);
                                var columnName = columns[columnHeader];
                                currentRow[columnName] = value;
                            }
                        }
                    }
                }

            }

            return DataResult.CreateSuccessful(data);
        }
    }
}
