using Keysmith.PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Keysmith.Models
{
    public enum PinningType
    {
        Standard = 0,
        SFIC = 1
    }

    public class PinningModel
    {
        #region Constant Values
        private const string defaultDriverPinHeader = "Driver Pins:";
        private const string defaultControlPinHeader = "Control Pins:";
        private const string defaultMasterPinHeader = "Master Pins:";
        private const string defaultBottomPinHeader = "Bottom Pins:";
        #endregion
        #region Private Members
        private PinningType _cylinderType = PinningType.Standard;
        private Func<List<int>, ObservableCollection<string>> CalculatePinColumn;
        #endregion
        #region Constructors
        public PinningModel(PinningType newCylinderType = PinningType.Standard, string newBottomPinHeader = defaultBottomPinHeader, string newMasterPinHeader = defaultMasterPinHeader, string newControlPinHeader = defaultControlPinHeader, string newDriverPinHeader = defaultDriverPinHeader)
        {
            Keys = new ObservableCollection<KeyModel>();
            Initialize(newCylinderType, newBottomPinHeader, newMasterPinHeader, newControlPinHeader, newDriverPinHeader);
        }
        public PinningModel(ObservableCollection<KeyModel> inputKeys, PinningType newCylinderType = PinningType.Standard, string newBottomPinHeader = defaultBottomPinHeader, string newMasterPinHeader = defaultMasterPinHeader, string newControlPinHeader = defaultControlPinHeader, string newDriverPinHeader = defaultDriverPinHeader)
        {
            Keys = inputKeys;
            Initialize(newCylinderType, newBottomPinHeader, newMasterPinHeader, newControlPinHeader, newDriverPinHeader);
        }
        #endregion
        #region Properties
        public ObservableCollection<KeyModel> Keys { get; private set; }
        public ObservableCollection<ObservableCollection<string>> Columns { get; private set; }
        public string DriverPinHeader { get; private set; }
        public string ControlPinHeader { get; private set; }
        public string MasterPinHeader { get; private set; }
        public string BottomPinHeader { get; private set; }
        public int RowCount { get; private set; }
        public int ColumnCount { get; private set; }
        public PinningType CylinderType
        {
            get { return _cylinderType; }
            set
            {
                _cylinderType = value;
                RefreshPinColumnDelegate();
            }
        }
        #endregion
        #region Private Methods
        private void Initialize(PinningType newCylinderType = PinningType.Standard, string newBottomPinHeader = defaultBottomPinHeader, string newMasterPinHeader = defaultMasterPinHeader, string newControlPinHeader = defaultControlPinHeader, string newDriverPinHeader = defaultDriverPinHeader)
        {
            CylinderType = newCylinderType;
            BottomPinHeader = newBottomPinHeader;
            MasterPinHeader = newMasterPinHeader;
            ControlPinHeader = newControlPinHeader;
            DriverPinHeader = newDriverPinHeader;
            Recalculate();
        }
        private void RefreshPinColumnDelegate()
        {
            switch (this.CylinderType)
            {
                case PinningType.SFIC:
                    CalculatePinColumn = CalculateSFICPinColumn;
                    break;
                default:
                    CalculatePinColumn = CalculateStandardPinColumn;
                    break;
            }
        }
        private void Recalculate()
        {
            if (Keys.Count == 0)
            {
                KeyModel emptyKey = new KeyModel
                { Cuts = "000000" };
                GenerateColumns(emptyKey);
            }
            else
            {
                KeyModel[] outputArray = new KeyModel[Keys.Count];
                Keys.CopyTo(outputArray, 0);
                GenerateColumns(outputArray);
            }
        }
        private void GenerateColumns(params KeyModel[] inputKeys)
        {
            ObservableCollection<ObservableCollection<string>> outputColumns = GetPinColumns(GetCutColumns(inputKeys));
            Columns = outputColumns;
            UpdateCounts();
        }
        private static List<List<int>> GetCutColumns(params KeyModel[] inputKeys)
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
        private ObservableCollection<ObservableCollection<string>> GetPinColumns(List<List<int>> cutColumns)
        {
            ObservableCollection<ObservableCollection<string>> pinColumns = new ObservableCollection<ObservableCollection<string>>();

            foreach (List<int> currentCutColumn in cutColumns)
            { pinColumns.Add(CalculatePinColumn(currentCutColumn)); }

            return PadPinColumnns(pinColumns);
        }
        private static int GetMaxColumnHeight(ObservableCollection<ObservableCollection<string>> inputColumns)
        {
            int maxHeight = 0;

            foreach (ObservableCollection<string> currentColumn in inputColumns)
            {
                if (currentColumn.Count > maxHeight)
                { maxHeight = currentColumn.Count; }
            }

            return maxHeight;
        }
        private static ObservableCollection<string> CalculateStandardPinColumn(List<int> cutColumn)
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
        private ObservableCollection<string> CalculateSFICPinColumn(List<int> arg)
        {
            throw new NotImplementedException();
        }
        private static ObservableCollection<ObservableCollection<string>> PadPinColumnns(ObservableCollection<ObservableCollection<string>> pinColumns, string paddingString = "-")
        {
            int maxHeight = GetMaxColumnHeight(pinColumns);

            foreach (ObservableCollection<string> currentColumn in pinColumns)
            {
                while (currentColumn.Count < maxHeight)
                { currentColumn.Add(paddingString); }
            }

            return pinColumns;
        }
        private static int GetMaxLength(params KeyModel[] inputKeys)
        {
            int outputLength = 0;
            foreach (KeyModel currentKey in inputKeys)
            {
                if (currentKey.Cuts.Length > outputLength)
                { outputLength = currentKey.Cuts.Length; }
            }
            return outputLength;
        }
        private void UpdateCounts()
        {
            ColumnCount = Columns.Count;
            RowCount = GetMaxColumnHeight(Columns);
        }
        #endregion
    }
}