namespace ReportTool.UI.ViewModels
{
    public class DataColumnViewModel : ViewModel
    {
        public string Name { get; }

        private SelectionType _selectionType;

        public SelectionType SelectionType
        {
            get
            {
                return _selectionType;
            }

            set
            {
                SetValue(ref _selectionType, value);
            }
        }

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
