using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Keysmith.Models
{
    public class PinningModelBase : ModelBase, IPinningModel
    {
        #region Constant Values
        protected const string defaultDriverPinHeader = "Driver Pins:";
        protected const string defaultControlPinHeader = "Control Pins:";
        protected const string defaultMasterPinHeader = "Master Pins:";
        protected const string defaultBottomPinHeader = "Bottom Pins:";
        protected const string defaultEmptyCellSpacer = "-";
        #endregion
        #region Constructors
        public PinningModelBase()
        {
            SetValues(new ObservableCollection<KeyModel>());
            Initialize();
        }
        public PinningModelBase(IEnumerable<KeyModel> inputKeys, bool inputIsEndStoppedLeft = true, String inputBottomPinHeader = defaultBottomPinHeader,
            String inputMasterPinHeader = defaultMasterPinHeader, String inputControlPinHeader = defaultControlPinHeader,
            String inputDriverPinHeader = defaultDriverPinHeader, String inputEmptyCellSpacer = defaultEmptyCellSpacer)
        {
            SetValues(inputKeys, inputIsEndStoppedLeft, inputBottomPinHeader, inputMasterPinHeader, inputControlPinHeader, inputDriverPinHeader, inputEmptyCellSpacer);

            Initialize();
        }
        #endregion
        #region Properties
        public ObservableCollection<KeyModel> Keys { get; protected set; } = new ObservableCollection<KeyModel>();
        public ObservableCollection<String> RowHeaders { get; protected set; } = new ObservableCollection<string>();
        public ObservableCollection<ObservableCollection<string>> Rows { get; protected set; } = new ObservableCollection<ObservableCollection<string>>();
        public int ColumnCount { get; protected set; }

        public string DriverPinHeader { get; protected set; } = defaultDriverPinHeader;
        public string ControlPinHeader { get; protected set; } = defaultControlPinHeader;
        public string MasterPinHeader { get; protected set; } = defaultMasterPinHeader;
        public string BottomPinHeader { get; protected set; } = defaultBottomPinHeader;
        public string EmptyCellSpacer { get; protected set; } = defaultEmptyCellSpacer;
        public bool IsEndStoppedLeft { get; protected set; } = true;
        #endregion
        #region Instance Methods
        protected void SetValues(IEnumerable<KeyModel> inputKeys, bool inputIsEndStoppedLeft = true, String inputBottomPinHeader = defaultBottomPinHeader,
            String inputMasterPinHeader = defaultMasterPinHeader, String inputControlPinHeader = defaultControlPinHeader,
            String inputDriverPinHeader = defaultDriverPinHeader, String inputEmptyCellSpacer = defaultEmptyCellSpacer)
        {
            Keys = inputKeys as ObservableCollection<KeyModel>;
            IsEndStoppedLeft = inputIsEndStoppedLeft;
            BottomPinHeader = inputBottomPinHeader;
            MasterPinHeader = inputMasterPinHeader;
            ControlPinHeader = inputControlPinHeader;
            DriverPinHeader = inputDriverPinHeader;
            EmptyCellSpacer = inputEmptyCellSpacer;
        }
        protected virtual void Initialize() { }
        #endregion
        #region Static Methods
        public static int GetMaxKeyLength(IEnumerable<KeyModel> inputKeys)
        {
            int outputLength = 0;
            foreach (KeyModel currentKey in inputKeys)
            {
                if (currentKey.Cuts.Length > outputLength)
                { outputLength = currentKey.Cuts.Length; }
            }
            return outputLength;
        }
        public static int GetMinKeyLength(IEnumerable<KeyModel> inputKeys)
        {
            int outputLength = 0;
            foreach (KeyModel currentKey in inputKeys)
            {
                if (outputLength <= 0 && currentKey.Cuts.Length > 0)
                { outputLength = currentKey.Cuts.Length; }
                else if (currentKey.Cuts.Length < outputLength && currentKey.Cuts.Length != 0)
                { outputLength = currentKey.Cuts.Length; }
            }
            return outputLength;
        }
        public static List<int?> PadKey(KeyModel inputKey, int paddingLength, bool isEndStoppedLeft = true)
        {
            List<int?> output = new List<int?>();
            int currentKeyLength = inputKey.CutsList.Count;
            int paddingDelta = paddingLength - currentKeyLength;

            if (paddingDelta >= 0) // Pad
            {
                if (isEndStoppedLeft == false)
                {
                    for (int paddingIndex = 0; paddingIndex < paddingDelta; paddingIndex++)
                    { output.Add(null); }
                }

                foreach (int currentCut in inputKey.CutsList)
                { output.Add(currentCut); }

                if (isEndStoppedLeft == true)
                {
                    for (int paddingIndex = 0; paddingIndex < paddingDelta; paddingIndex++)
                    { output.Add(null); }
                }
            }
            else if (isEndStoppedLeft) // Trim right
            {
                for (int columnIndex = 0; columnIndex < paddingLength; columnIndex++)
                { output.Add(inputKey.CutsList[columnIndex]); }
            }
            else // Trim left
            {
                for (int columnIndex = Math.Abs(paddingDelta); columnIndex < currentKeyLength; columnIndex++)
                { output.Add(inputKey.CutsList[columnIndex]); }
            }

            return output;
        }
        public static List<List<int?>> GetPaddedKeys(IEnumerable<KeyModel> inputOperatingKeys, int paddingLength, bool isEndStoppedLeft = true)
        {
            List<List<int?>> output = new List<List<int?>>();

            foreach (KeyModel currentKey in inputOperatingKeys)
            { output.Add(PadKey(currentKey, paddingLength, isEndStoppedLeft)); }

            return output;
        }
        public static List<List<int?>> GenerateEmptyPaddedKeys(int paddingLength)
        {
            List<List<int?>> output = new List<List<int?>>();
            List<int?> currentRow = new List<int?>();

            for (int columnIndex = 0; columnIndex < paddingLength; columnIndex++)
            { currentRow.Add(null); }

            output.Add(currentRow);

            return output;
        }
        public static List<int?> GetSortedCutsAtIndex(List<List<int?>> inputKeys, int index)
        {
            List<int?> output = new List<int?>();

            foreach (List<int?> currentKey in inputKeys)
            {
                int? currentCut = currentKey[index];

                if (currentCut != null && output.Contains(currentCut) == false)
                {
                    output.Add(currentCut);
                }
            }

            output.Sort();

            return output;
        }
        public static int GetMaxColumnHeight(List<List<int?>> inputColumns)
        {
            int outputHeight = 0;

            foreach (List<int?> currentColumn in inputColumns)
            {
                if (currentColumn.Count > outputHeight)
                { outputHeight = currentColumn.Count; }
            }

            return outputHeight;
        }
        public static List<List<int?>> GetSortedCuts(List<List<int?>> inputKeys)
        {
            List<List<int?>> output = new List<List<int?>>();
            if (inputKeys.Count == 0)
            { return output; }

            int keyLength = inputKeys[0].Count;

            foreach (List<int?> currentKey in inputKeys)
            {
                if (currentKey.Count != keyLength)
                { throw new ArgumentException(); }
            }

            for (int columnIndex = 0; columnIndex < keyLength; columnIndex++)
            { output.Add(GetSortedCutsAtIndex(inputKeys, columnIndex)); }

            return output;
        }
        public static bool ValidateCuts(List<int?> inputCutColumn)
        { // Returns false only on empty input, throws exception for invalid input. (Empty input is normal use case, invalid input means bad code.)
            if (inputCutColumn.Count <= 0)
            { return false; }
            else if(inputCutColumn[0] == null)
            { throw new ArgumentNullException("inputCutColumn cannot contain null values."); }

            for (int index = 1; index < inputCutColumn.Count; index++)
            {
                int? previousCut = inputCutColumn[index - 1];
                int? currentCut = inputCutColumn[index];

                if (currentCut == null || previousCut == null)
                { throw new ArgumentNullException("inputCutColumn cannot contain null values."); }
                else if (currentCut <= previousCut)
                { throw new ArgumentException("inputCutColumn must be sorted."); }
            }

            return true;
        }
        public static bool ValidateCuts(List<List<int?>> inputCutsColumn)
        {
            if(inputCutsColumn.Count < 1)
            { return false; }

            bool result = false;

            foreach(List<int?> currentColumn in inputCutsColumn)
            {
                if(ValidateCuts(currentColumn) == true)
                { result = true; }
            }

            return result;
        }
        public static int? GetDeepestCut(List<int?> inputCutColumn)
        {
            if (ValidateCuts(inputCutColumn) == false)
            { return null; }

            return inputCutColumn[inputCutColumn.Count - 1];
        }
        public static List<int?> GetDeepestCuts(List<List<int?>> inputCutColumns)
        {
            List<int?> output = new List<int?>();

            foreach (List<int?> currentColumn in inputCutColumns)
            { output.Add(GetDeepestCut(currentColumn)); }

            return output;
        }
        public static List<int?> CalculatePinColumn(List<int?> inputCutColumn)
        {
            List<int?> output = new List<int?>();
            int? previousCut = null;

            if (ValidateCuts(inputCutColumn) == false)
            { return output; }

            foreach (int? currentCut in inputCutColumn)
            {
                if (previousCut == null)
                {
                    output.Add(currentCut);
                    previousCut = currentCut;
                }
                else
                {
                    output.Add(currentCut - previousCut);
                    previousCut = currentCut;
                }
            }

            return output;
        }
        public static List<int?> PadColumn(List<int?> inputColumn, int paddingHeight)
        {
            if (inputColumn.Count > paddingHeight)
            { throw new ArgumentOutOfRangeException(); }
            else if (inputColumn.Count == paddingHeight)
            { return inputColumn; }
            else
            {
                List<int?> output = new List<int?>();
                for (int rowIndex = 0; rowIndex < paddingHeight; rowIndex++)
                {
                    if (rowIndex >= inputColumn.Count)
                    { output.Add(null); }
                    else
                    { output.Add(inputColumn[rowIndex]); }
                }
                return output;
            }
        }
        public static List<List<int?>> PadColumns(List<List<int?>> inputColumns)
        {
            List<List<int?>> output = new List<List<int?>>();
            int maxColumnHeight = GetMaxColumnHeight(inputColumns);

            foreach (List<int?> currentColumn in inputColumns)
            { output.Add(PadColumn(currentColumn, maxColumnHeight)); }

            return output;
        }
        public static List<List<int?>> GetOperatingPins(List<List<int?>> inputOperatingCuts)
        {
            List<List<int?>> output = new List<List<int?>>();

            foreach (List<int?> currentCutColumn in inputOperatingCuts)
            { output.Add(CalculatePinColumn(currentCutColumn)); }

            return PadColumns(output);
        }
        public static ObservableCollection<String> GetRowAtIndex(List<List<int?>> inputColumns, int index, String inputEmptyCellSpacer = defaultEmptyCellSpacer)
        {
            ObservableCollection<String> output = new ObservableCollection<string>();

            int maxColumnHeight = GetMaxColumnHeight(inputColumns);

            if (index < 0 || index >= maxColumnHeight || inputColumns.Count <= 0)
            { throw new IndexOutOfRangeException(); }

            foreach (List<int?> currentColumn in inputColumns)
            {
                int? currentPin;
                if (index < currentColumn.Count)
                { currentPin = currentColumn[index]; }
                else
                { currentPin = null; }

                if (currentPin == null)
                { output.Add(inputEmptyCellSpacer); }
                else
                { output.Add(currentPin.ToString()); }
            }

            return output;
        }
        public static int CountRowsFromColumns(List<List<int?>> inputColumns)
        {
            int output = 0;

            foreach (List<int?> currentColumn in inputColumns)
            {
                if (currentColumn.Count > output)
                { output = currentColumn.Count; }
            }

            return output;
        }
        public static ObservableCollection<String> GenerateStandardRowHeaders(int operatingRows, string inputBottomPinHeader = defaultBottomPinHeader,
            string inputMasterPinHeader = defaultMasterPinHeader)
        {
            ObservableCollection<String> output = new ObservableCollection<string>();
            if (operatingRows < 1)
            { return output; }

            output.Add(inputBottomPinHeader);

            for (int rowIndex = 1; rowIndex < operatingRows; rowIndex++)
            { output.Add(inputMasterPinHeader); }

            return output;
        }
        public static ObservableCollection<ObservableCollection<String>> GenerateStandardRows(List<List<int?>> inputOperatingPins,
            String inputEmptyCellSpacer = defaultEmptyCellSpacer)
        {
            ObservableCollection<ObservableCollection<String>> output = new ObservableCollection<ObservableCollection<string>>();
            int rowCount = CountRowsFromColumns(inputOperatingPins);

            if (inputOperatingPins.Count < 1)
            { return output; }

            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            { output.Add(GetRowAtIndex(inputOperatingPins, rowIndex, inputEmptyCellSpacer)); }

            return output;
        }
        public static int GetMaxRowLength(ObservableCollection<ObservableCollection<String>> inputRows)
        {
            int output = 0;

            foreach (ObservableCollection<String> currentRow in inputRows)
            {
                if (currentRow.Count > output)
                { output = currentRow.Count; }
            }

            return output;
        }
        #endregion
    }
}
