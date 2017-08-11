namespace ReportTool.DataProviders.FileDataProviders.DataReaders
{
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using ReportTool.DataProviders;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [SupportedFileExtension(".xlsx")]
    public class ExcelDataReader : IDataReader
    {
        public DataResult Read(string path)
        {
            try
            {
                var data = new List<Dictionary<string, double>>();
                using (var document = SpreadsheetDocument.Open(path, false))
                {
                    var sheet = document.WorkbookPart.WorksheetParts.First();
                    var sharedStrings = new List<string>();
                    if (document.WorkbookPart.SharedStringTablePart != null)
                    {
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
                    }

                    var columns = new Dictionary<string, string>();
                    using (var reader = OpenXmlReader.Create(sheet))
                    {
                        Dictionary<string, double> currentRow = null;
                        bool? isFirstRowSaved = null;
                        while (reader.Read())
                        {
                            if (reader.ElementType == typeof(Row) && reader.IsStartElement && isFirstRowSaved.HasValue == false)
                            {
                                isFirstRowSaved = false;
                            }

                            if (reader.ElementType == typeof(Row) && reader.IsEndElement && isFirstRowSaved.HasValue && isFirstRowSaved.Value == false)
                            {
                                isFirstRowSaved = true;
                            }

                            if (reader.ElementType == typeof(Cell))
                            {
                                var cell = (Cell)reader.LoadCurrentElement();
                                var columnHeader = new string(cell.CellReference.Value.TakeWhile(Char.IsLetter).ToArray());

                                if (cell.DataType != null && (cell.DataType == CellValues.SharedString || cell.DataType == CellValues.InlineString) && isFirstRowSaved.HasValue && isFirstRowSaved.Value == false)
                                {
                                    if (cell.DataType == CellValues.SharedString)
                                    {
                                        int.TryParse(cell.CellValue.Text, out int textId);
                                        columns.Add(columnHeader, sharedStrings.ElementAt(textId));
                                    }
                                    else
                                    {
                                        columns.Add(columnHeader, cell.InnerText);
                                    }
                                }
                                else if (isFirstRowSaved.HasValue && isFirstRowSaved.Value == true)
                                {
                                    if (columnHeader.Equals("A"))
                                    {
                                        currentRow = new Dictionary<string, double>();
                                        data.Add(currentRow);
                                    }

                                    if (cell.DataType == null || cell.DataType == CellValues.Number)
                                    {
                                        var value = double.Parse(cell.CellValue.Text);
                                        var columnName = columns[columnHeader];
                                        currentRow[columnName] = value;
                                    }
                                    else
                                    {
                                        throw new FormatException("String was not in a correct format");
                                    }
                                }
                            }
                        }
                    }

                }

                return DataResult.CreateSuccessful(data);
            }
            catch (Exception exception)
            {
                return DataResult.CreateFailed(exception.Message);
            }
        }
    }
}
