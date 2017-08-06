namespace ReportTool
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class DataColumnViewModel : INotifyPropertyChanged
    {
        public string Name { get; }

        private SelectionType _selectionType;

        public event PropertyChangedEventHandler PropertyChanged;

        public SelectionType SelectionType
        {
            get
            {
                return _selectionType;
            }

            set
            {
                if (_selectionType == value)
                {
                    return;
                }

                _selectionType = value;
                RaisePropertyChanged();
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

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
