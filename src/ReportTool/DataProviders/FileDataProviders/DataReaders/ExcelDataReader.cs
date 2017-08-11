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
    public class ExcelDataReader : FileDataReader
    {
        private const string FIRST_COLUMN_KEY = "A";

        protected override DataResult ReadImpl(string path)
        {
            using (var document = SpreadsheetDocument.Open(path, false))
            {
                var sheet = document.WorkbookPart.WorksheetParts.First();
                var sharedStrings = TryLoadSharedStrings(document);
                var cellReader = new CellReader(sharedStrings);

                var data = ReadValues(sheet, cellReader);

                return DataResult.CreateSuccessful(data);
            }
        }

        private List<Dictionary<string, double>> ReadValues(WorksheetPart sheet, CellReader cellReader)
        {
            var data = new List<Dictionary<string, double>>();
            using (var reader = OpenXmlReader.Create(sheet))
            {
                var columns = new Dictionary<string, string>();
                Action<Dictionary<string, double>, string, double> saveValueTo = (row, columnKey, value) => row[columns[columnKey]] = value;
                var firstRowState = ReadingState.NotStarted;
                Dictionary<string, double> currentRow = null;

                while (reader.Read())
                {
                    if (isStartingReadFirstRow(reader, firstRowState))
                    {
                        firstRowState = ReadingState.Reading;
                    }

                    if (isEndingReadFirstRow(reader, firstRowState))
                    {
                        firstRowState = ReadingState.Complete;
                    }

                    if (reader.ElementType == typeof(Cell))
                    {
                        var cell = (Cell)reader.LoadCurrentElement();
                        var columnKey = cellReader.GetColumnKeyOf(cell);

                        if (firstRowState == ReadingState.Reading)
                        {
                            var columnName = cellReader.GetValueFrom(cell);
                            columns.Add(columnKey, columnName);
                            continue;
                        }

                        if (firstRowState == ReadingState.Complete)
                        {
                            if (IsFirstColumn(columnKey))
                            {
                                currentRow = new Dictionary<string, double>();
                                data.Add(currentRow);
                            }

                            var value = cellReader.GetValueOfDouble(cell);
                            saveValueTo(currentRow, columnKey, value);
                        }
                    }
                }
            }

            return data;
        }

        private bool IsFirstColumn(string columnKey)
        {
            return columnKey.Equals(FIRST_COLUMN_KEY);
        }

        private bool isStartingReadFirstRow(OpenXmlReader reader, ReadingState firstRowState)
        {
            return reader.ElementType == typeof(Row) && reader.IsStartElement && firstRowState == ReadingState.NotStarted;
        }

        private bool isEndingReadFirstRow(OpenXmlReader reader, ReadingState firstRowState)
        {
            return reader.ElementType == typeof(Row) && reader.IsEndElement && firstRowState == ReadingState.Reading;
        }

        private List<string> TryLoadSharedStrings(SpreadsheetDocument document)
        {
            var strings = new List<string>();
            if (document.WorkbookPart.SharedStringTablePart == null)
            {
                return strings;
            }

            using (var reader = OpenXmlReader.Create(document.WorkbookPart.SharedStringTablePart))
            {
                while (reader.Read())
                {
                    if (reader.ElementType == typeof(SharedStringItem))
                    {
                        var item = (SharedStringItem)reader.LoadCurrentElement();
                        strings.Add(item.Text.Text);
                    }
                }
            }

            return strings;
        }

        private enum ReadingState
        {
            NotStarted = 0,
            Reading = 1,
            Complete = 2
        }

        private class CellReader
        {
            private List<string> _sharedStrings;

            public CellReader(List<string> sharedStrings)
            {
                _sharedStrings = sharedStrings;
            }

            public string GetValueFrom(Cell cell)
            {
                if (cell.DataType == null)
                {
                    return cell.CellValue.Text;
                }

                if (cell.DataType == CellValues.SharedString)
                {
                    int.TryParse(cell.CellValue.Text, out int textId);
                    return _sharedStrings.ElementAt(textId);
                }

                if (cell.DataType == CellValues.InlineString)
                {
                    return cell.InnerText;
                }

                if (cell.DataType == CellValues.Number)
                {
                    return cell.CellValue.Text;
                }

                throw new FormatException("String was not in a correct format");
            }

            public double GetValueOfDouble(Cell cell)
            {
                var rawValue = GetValueFrom(cell);
                return double.Parse(rawValue);
            }

            public string GetColumnKeyOf(Cell cell)
            {
                var onlyColumnLetters = cell.CellReference.Value.TakeWhile(Char.IsLetter).ToArray();
                return new string(onlyColumnLetters);
            }
        }
    }
}
