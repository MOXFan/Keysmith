using Keysmith.PropertyChanged;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Keysmith.Models
{
    public class PinningModel : PropertyChangedBase
    {
        #region Private Members
        private ObservableCollection<ObservableCollection<string>> _columns = new ObservableCollection<ObservableCollection<string>>();
        private const string defaultTopPinHeader = "Top Pins:";
        private const string defaultBottomPinHeader = "Bottom Pins:";
        private string _topPinHeader = defaultTopPinHeader;
        private string _bottomPinHeader = defaultBottomPinHeader;
        private int _rowCount = 0;
        private int _columnCount = 0;
        #endregion
        #region Constructors
        public PinningModel(string newBottomPinHeader = defaultBottomPinHeader, string newTopPinHeader = defaultTopPinHeader)
        {
            TopPinHeader = newTopPinHeader;
            BottomPinHeader = newBottomPinHeader;

            GenerateColumns(new ObservableCollection<KeyModel>());
        }
        public PinningModel(ObservableCollection<KeyModel> inputKeys, string newBottomPinHeader = defaultBottomPinHeader, string newTopPinHeader = defaultTopPinHeader)
        {
            TopPinHeader = newTopPinHeader;
            BottomPinHeader = newBottomPinHeader;
            GenerateColumns(inputKeys);
        }
        #endregion
        #region Properties
        public ObservableCollection<ObservableCollection<string>> Columns
        {
            get { return _columns; }
            set
            {
                _columns = value;
                OnPropertyChanged();
                UpdateCounts();
            }
        }
        public string TopPinHeader
        {
            get { return _topPinHeader; }
            set
            {
                _topPinHeader = value;
                OnPropertyChanged();
            }
        }
        public string BottomPinHeader
        {
            get { return _bottomPinHeader; }
            set
            {
                _bottomPinHeader = value;
                OnPropertyChanged();
            }
        }
        public int RowCount
        {
            get { return _rowCount; }
            set
            {
                _rowCount = value;
                OnPropertyChanged();
            }
        }
        public int ColumnCount
        {
            get { return _columnCount; }
            set
            {
                _columnCount = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Methods
        public void GenerateColumns(ObservableCollection<KeyModel> inputKeysCollection)
        {
            if (inputKeysCollection.Count == 0)
            {
                KeyModel emptyKey = new KeyModel
                { Cuts = "000000" };
                GenerateColumns(emptyKey);
            }
            else
            {
                KeyModel[] outputArray = new KeyModel[inputKeysCollection.Count];
                inputKeysCollection.CopyTo(outputArray, 0);
                GenerateColumns(outputArray);
            }
        }
        public void GenerateColumns(params KeyModel[] inputKeys)
        { Columns = GetPinColumns(GetCutColumns(inputKeys)); }
        public ObservableCollection<ObservableCollection<string>> GetPinColumns(List<List<int>> cutColumns)
        {
            ObservableCollection<ObservableCollection<string>> pinColumns = new ObservableCollection<ObservableCollection<string>>();

            foreach (List<int> currentCutColumn in cutColumns)
            { pinColumns.Add(CalculatePinColumn(currentCutColumn)); }

            return PadPinColumnns(pinColumns);
        }
        public int GetMaxColumnHeight(ObservableCollection<ObservableCollection<string>> inputColumns)
        {
            int maxHeight = 0;

            foreach (ObservableCollection<string> currentColumn in inputColumns)
            {
                if (currentColumn.Count > maxHeight)
                { maxHeight = currentColumn.Count; }
            }

            return maxHeight;
        }
        public ObservableCollection<ObservableCollection<string>> PadPinColumnns(ObservableCollection<ObservableCollection<string>> pinColumns, string paddingString = "-")
        {
            int maxHeight = GetMaxColumnHeight(pinColumns);

            foreach (ObservableCollection<string> currentColumn in pinColumns)
            {
                while (currentColumn.Count < maxHeight)
                { currentColumn.Add(paddingString); }
            }

            return pinColumns;
        }
        public ObservableCollection<string> CalculatePinColumn(List<int> cutColumn)
        {
            ObservableCollection<string> pinColumn = new ObservableCollection<string>();

            cutColumn.Sort();
            int stackHeight = 0;
            for (int index = 0; index < cutColumn.Count; index++)
            {
                int currentCut = cutColumn[index];
                int currentPin = currentCut - stackHeight;
                stackHeight = currentCut;

                if (currentPin > 0 || index == 0)
                { pinColumn.Add(currentPin.ToString()); }
            }

            return pinColumn;
        }
        public List<List<int>> GetCutColumns(params KeyModel[] inputKeys)
        {
            int numberOfKeys = inputKeys.Length;
            int maxLength = GetMaxLength(inputKeys);

            List<List<int>> cutColumns = new List<List<int>>();

            for (int columnIndex = 0; columnIndex < maxLength; columnIndex++)
            {
                List<int> currentColumnCuts = new List<int>();

                for (int keyIndex = 0; keyIndex < numberOfKeys; keyIndex++)
                {
                    KeyModel currentKey = inputKeys[keyIndex];

                    if (columnIndex < currentKey.Cuts.Length)
                    { currentColumnCuts.Add(currentKey.CutsList[columnIndex]); }
                }

                cutColumns.Add(currentColumnCuts);
            }

            return cutColumns;
        }
        public static int GetMaxLength(params KeyModel[] inputKeys)
        {
            int outputLength = 0;
            foreach (KeyModel currentKey in inputKeys)
            {
                if (currentKey.Cuts.Length > outputLength)
                { outputLength = currentKey.Cuts.Length; }
            }
            return outputLength;
        }
        public void UpdateCounts()
        {
            ColumnCount = Columns.Count;
            RowCount = GetMaxColumnHeight(Columns);
        }
        #endregion
    }
}
