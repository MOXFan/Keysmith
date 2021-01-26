using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Keysmith.Models
{
    public class SFICPinningModel : ModelBase
    {
        #region Constant Values
        private const string defaultDriverPinHeader = "Driver Pins:";
        private const string defaultControlPinHeader = "Control Pins:";
        private const string defaultMasterPinHeader = "Master Pins:";
        private const string defaultBottomPinHeader = "Bottom Pins:";
        private const string defaultEmptyCellSpacer = "-";
        #endregion
        #region Properties
        public ObservableCollection<KeyModel> Keys { get; private set; } = new ObservableCollection<KeyModel>();
        public ObservableCollection<KeyModel> ControlKeys { get; private set; } = new ObservableCollection<KeyModel>();
        public ObservableCollection<KeyModel> OperatingKeys { get; private set; } = new ObservableCollection<KeyModel>();
        public ObservableCollection<ObservableCollection<string>> Rows { get; private set; } = new ObservableCollection<ObservableCollection<string>>();
        public ObservableCollection<String> RowHeaders { get; private set; } = new ObservableCollection<string>();
        public string DriverPinHeader { get; private set; } = defaultDriverPinHeader;
        public string ControlPinHeader { get; private set; } = defaultControlPinHeader;
        public string MasterPinHeader { get; private set; } = defaultMasterPinHeader;
        public string BottomPinHeader { get; private set; } = defaultBottomPinHeader;
        public string EmptyCellSpacer { get; private set; } = defaultEmptyCellSpacer;
        public int ColumnCount { get; private set; }
        public bool IsEndStoppedLeft { get; private set; } = true;
        #endregion
        #region Constructors
        public SFICPinningModel() 
        {
            SetValues(new ObservableCollection<KeyModel>());
            Calculate();
        }
        public SFICPinningModel(IEnumerable<KeyModel> inputKeys, bool inputIsEndStoppedLeft = true, String inputBottomPinHeader = defaultBottomPinHeader,
            String inputMasterPinHeader = defaultMasterPinHeader, String inputControlPinHeader = defaultControlPinHeader,
            String inputDriverPinHeader = defaultDriverPinHeader, String inputEmptyCellSpacer = defaultEmptyCellSpacer)
        {
            SetValues(inputKeys, inputIsEndStoppedLeft, inputBottomPinHeader, inputMasterPinHeader, inputControlPinHeader, inputDriverPinHeader, inputEmptyCellSpacer);

            Calculate();
        }
        #endregion
        #region Instance Methods
        private void SetValues(IEnumerable<KeyModel> inputKeys, bool inputIsEndStoppedLeft = true, String inputBottomPinHeader = defaultBottomPinHeader,
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

            ControlKeys = GetControlKeys(Keys);
            OperatingKeys = GetOperatingKeys(Keys);
        }
        private void Calculate()
        {
            int maxKeyLength = GetMaxKeyLength(OperatingKeys);

            List<List<int?>> paddedOperatingKeys = GetPaddedKeys(OperatingKeys, maxKeyLength, IsEndStoppedLeft);
            List<List<int?>> paddedControlKeys = GetPaddedKeys(ControlKeys, maxKeyLength, IsEndStoppedLeft);

            if (maxKeyLength < 1)
            { paddedOperatingKeys = GenerateEmptyPaddedKeys(7); }

            List<List<int?>> operatingCuts = GetSortedCuts(paddedOperatingKeys);
            List<List<int?>> controlCuts = GetSortedCuts(paddedControlKeys);

            List<int?> deepestOperatingCuts = GetDeepestCuts(operatingCuts);
            List<int?> deepestControlCuts = GetDeepestCuts(controlCuts);

            List<List<int?>> operatingPins = GetOperatingPins(operatingCuts);
            List<List<int?>> controlPins = GetControlPins(controlCuts, deepestOperatingCuts);
            ObservableCollection<String> driverPins = GetDriverPins(deepestControlCuts, deepestOperatingCuts, EmptyCellSpacer);

            int operatingRowCount = CountRowsFromColumns(operatingPins);
            int controlRowCount = CountRowsFromColumns(controlPins);

            RowHeaders = GenerateRowHeaders(operatingRowCount, controlRowCount, BottomPinHeader, MasterPinHeader, ControlPinHeader, DriverPinHeader);
            Rows = GenerateRows(operatingPins, controlPins, driverPins, EmptyCellSpacer);
            ColumnCount = GetMaxRowLength(Rows);
        }
        #endregion
        #region Static Methods
        private static ObservableCollection<KeyModel> GetControlKeys(IEnumerable<KeyModel> inputKeys)
        {
            ObservableCollection<KeyModel> outputKeys = new ObservableCollection<KeyModel>();

            foreach (KeyModel currentKey in inputKeys)
            {
                if (currentKey.IsControl)
                { outputKeys.Add(currentKey); }
            }

            return outputKeys;
        }
        private static ObservableCollection<KeyModel> GetOperatingKeys(IEnumerable<KeyModel> inputKeys)
        {
            ObservableCollection<KeyModel> outputKeys = new ObservableCollection<KeyModel>();

            foreach (KeyModel currentKey in inputKeys)
            {
                if (currentKey.IsControl == false)
                { outputKeys.Add(currentKey); }
            }

            return outputKeys;
        }
        private static int GetMaxKeyLength(IEnumerable<KeyModel> inputKeys)
        {
            int outputLength = 0;
            foreach (KeyModel currentKey in inputKeys)
            {
                if (currentKey.Cuts.Length > outputLength)
                { outputLength = currentKey.Cuts.Length; }
            }
            return outputLength;
        }
        private static List<int?> PadKey(KeyModel inputKey, int paddingLength, bool isEndStoppedLeft = true)
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
                for (int columnIndex = Math.Abs(paddingDelta); columnIndex < paddingLength; columnIndex++)
                { output.Add(inputKey.CutsList[columnIndex]); }
            }

            return output;
        }
        private static List<List<int?>> GetPaddedKeys(IEnumerable<KeyModel> inputOperatingKeys, int paddingLength, bool isEndStoppedLeft = true)
        {
            List<List<int?>> output = new List<List<int?>>();

            foreach (KeyModel currentKey in inputOperatingKeys)
            { output.Add(PadKey(currentKey, paddingLength, isEndStoppedLeft)); }

            return output;
        }
        private static List<List<int?>> GenerateEmptyPaddedKeys(int paddingLength)
        {
            List<List<int?>> output = new List<List<int?>>();
            List<int?> currentRow = new List<int?>();

            for(int columnIndex = 0; columnIndex < paddingLength;columnIndex++)
            { currentRow.Add(null); }

            output.Add(currentRow);

            return output;
        }
        private static List<int?> GetSortedCutsAtIndex(List<List<int?>> inputKeys, int index)
        {
            List<int?> output = new List<int?>();

            foreach(List<int?> currentKey in inputKeys)
            {
                output.Add(currentKey[index]);
            }

            output.Sort();

            return output;
        }
        private static int GetMaxColumnHeight(List<List<int?>> inputColumns)
        {
            int outputHeight = 0;

            foreach (List<int?> currentColumn in inputColumns)
            {
                if (currentColumn.Count > outputHeight)
                { outputHeight = currentColumn.Count; }
            }

            return outputHeight;
        }
        private static List<int?> PadColumn(List<int?> inputColumn, int paddingHeight)
        {
            if (inputColumn.Count >= paddingHeight)
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
        private static List<List<int?>> PadCutColumns(List<List<int?>> inputCutColumns)
        {
            List<List<int?>> output = new List<List<int?>>();
            int maxColumnHeight = GetMaxColumnHeight(inputCutColumns);

            foreach (List<int?> currentColumn in inputCutColumns)
            { output.Add(PadColumn(currentColumn, maxColumnHeight)); }

            return output;
        }
        private static List<List<int?>> GetSortedCuts(List<List<int?>> inputKeys)
        {
            List<List<int?>> output = new List<List<int?>>();
            if(inputKeys.Count == 0)
            { return output; }

            int keyLength = inputKeys[0].Count;

            for (int columnIndex = 0; columnIndex < keyLength; columnIndex++)
            { output.Add(GetSortedCutsAtIndex(inputKeys, columnIndex)); }

            return PadCutColumns(output);
        }
        private static int? GetDeepestCut(List<int?> inputCutColumn)
        {
            if (inputCutColumn.Count <= 0)
            { return null; }
            else
            { return inputCutColumn[inputCutColumn.Count - 1]; }
        }
        private static List<int?> GetDeepestCuts(List<List<int?>> inputCutColumns)
        {
            List<int?> output = new List<int?>();

            foreach (List<int?> currentColumn in inputCutColumns)
            { output.Add(GetDeepestCut(currentColumn)); }

            return output;
        }
        private static List<List<int?>> GetOperatingPins(List<List<int?>> inputOperatingCuts)
        {
            List<List<int?>> output = new List<List<int?>>();

            foreach (List<int?> currentCutColumn in inputOperatingCuts)
            {
                List<int?> currentPinColumn = new List<int?>();
                int? previousCut = null;

                foreach (int? currentCut in currentCutColumn)
                {
                    if (currentCut == null)
                    { currentPinColumn.Add(null); }
                    else if(previousCut == null)
                    {
                        currentPinColumn.Add(currentCut);
                        previousCut = currentCut;
                    }
                    else 
                    {
                        currentPinColumn.Add(currentCut - previousCut);
                        previousCut = currentCut;
                    }
                }

                output.Add(currentPinColumn);
            }

            return output;
        }
        private static List<List<int?>> GetControlPins(List<List<int?>> inputControlCuts, List<int?> inputDeepestOperatingCuts)
        {
            List<List<int?>> output = new List<List<int?>>();
            if (inputControlCuts.Count < 1)
            { return output; }

            for (int columnIndex = 0; columnIndex < inputDeepestOperatingCuts.Count; columnIndex++)
            {
                List<int?> currentPinColumn = new List<int?>();
                List<int?> currentCutColumn = inputControlCuts[columnIndex];
                int? currentDeepestOperatingCut = inputDeepestOperatingCuts[columnIndex];
                int? previousCut = null;

                foreach (int? currentCut in currentCutColumn)
                {
                    if (currentCut == null || currentDeepestOperatingCut == null)
                    { currentPinColumn.Add(null); }
                    else if (previousCut == null)
                    {
                        currentPinColumn.Add(currentCut + 10 - currentDeepestOperatingCut);
                        previousCut = currentCut; 
                    }
                    else
                    {
                        currentPinColumn.Add(currentCut - previousCut);
                        previousCut = currentCut;
                    }
                }

                output.Add(currentPinColumn);
            }

            return output;
        }
        private ObservableCollection<String> GetDriverPins(List<int?> inputDeepestControlCuts, List<int?> inputDeepestOperatingCuts, String inputEmptyCellSpacer)
        {
            ObservableCollection<String> output = new ObservableCollection<string>();

            for (int columnIndex = 0; columnIndex < inputDeepestOperatingCuts.Count; columnIndex++)
            {
                int? currentControlCut = null;
                if (columnIndex < inputDeepestControlCuts.Count)
                { currentControlCut = inputDeepestControlCuts[columnIndex]; }
                int? currentOperatingCut = inputDeepestOperatingCuts[columnIndex];

                if (currentOperatingCut == null)
                { output.Add(inputEmptyCellSpacer); }
                else if (currentControlCut == null)
                { output.Add((23 - currentOperatingCut).ToString()); }
                else
                { output.Add((13 - currentControlCut).ToString()); } // 23 - (control cut + 10)
            }

            return output;
        }
        private int CountRowsFromColumns(List<List<int?>> inputColumns)
        {
            int output = 0;

            foreach (List<int?> currentColumn in inputColumns)
            {
                if (currentColumn.Count > output)
                { output = currentColumn.Count; }
            }

            return output;
        }
        private static ObservableCollection<String> GenerateRowHeaders(int operatingRows, int controlRows, string inputBottomPinHeader = defaultBottomPinHeader, 
            string inputMasterPinHeader = defaultMasterPinHeader, string inputControlPinHeader = defaultControlPinHeader, 
            string inputDriverPinHeader = defaultDriverPinHeader)
        {
            ObservableCollection<String> output = new ObservableCollection<string>();
            if (operatingRows < 1)
            { return output; }

            output.Add(inputBottomPinHeader);

            for (int rowIndex = 1; rowIndex < operatingRows; rowIndex++)
            { output.Add(inputMasterPinHeader); }

            for (int rowIndex = 0; rowIndex < controlRows; rowIndex++)
            { output.Add(inputControlPinHeader); }

            output.Add(inputDriverPinHeader);

            return output;
        }
        private static ObservableCollection<String> GetRowAtIndex(List<List<int?>> inputColumns, int index, String inputEmptyCellSpacer = defaultEmptyCellSpacer)
        {
            ObservableCollection<String> output = new ObservableCollection<string>();

            foreach (List<int?> currentColumn in inputColumns)
            {
                int? currentPin = currentColumn[index];
                if (currentPin == null)
                { output.Add(inputEmptyCellSpacer); }
                else
                { output.Add(currentPin.ToString()); }
            }

            return output;
        }
        private static ObservableCollection<ObservableCollection<String>> GenerateRows(List<List<int?>> inputOperatingPins, List<List<int?>> inputControlPins, 
            ObservableCollection<String> inputDriverPins, String inputEmptyCellSpacer = defaultEmptyCellSpacer)
        {
            ObservableCollection<ObservableCollection<String>> output = new ObservableCollection<ObservableCollection<string>>();

            if (inputOperatingPins.Count < 1)
            { return output; }

            for (int rowIndex = 0; rowIndex < inputOperatingPins[0].Count; rowIndex++)
            { output.Add(GetRowAtIndex(inputOperatingPins, rowIndex, inputEmptyCellSpacer)); }

            if (inputControlPins.Count > 0)
            {
                for (int rowIndex = 0; rowIndex < inputControlPins[0].Count; rowIndex++)
                { output.Add(GetRowAtIndex(inputControlPins, rowIndex, inputEmptyCellSpacer)); }
            }

            if (inputDriverPins.Count > 0)
            { output.Add(inputDriverPins); }

            return output;
        }
        private static int GetMaxRowLength(ObservableCollection<ObservableCollection<String>> inputRows)
        {
            int output = 0;

            foreach(ObservableCollection<String> currentRow in inputRows)
            {
                if (currentRow.Count > output)
                { output = currentRow.Count; }
            }

            return output;
        }
        #endregion
    }
}
