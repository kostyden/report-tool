namespace ReportTool
{
    using System;

    public class DataColumnViewModel
    {
        public string Name { get; }

        public SelectionType SelectionType { get; set; }

        public bool IsSelected
        {
            get
            {
                return SelectionType != SelectionType.NotSelected;
            }
        }

        public DataColumnViewModel(string columnName)
        {
            Name = columnName;
            SelectionType = SelectionType.NotSelected;
        }
    }
}
