using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Keysmith.Models
{
    public class SFICPinningModel : PinningModelBase
    {
        #region Properties
        public ObservableCollection<KeyModel> ControlKeys { get; private set; } = new ObservableCollection<KeyModel>();
        public ObservableCollection<KeyModel> OperatingKeys { get; private set; } = new ObservableCollection<KeyModel>();
        #endregion
        #region Constructors
        public SFICPinningModel() { }
        public SFICPinningModel(IEnumerable<KeyModel> inputKeys, bool inputIsEndStoppedLeft = true, String inputBottomPinHeader = defaultBottomPinHeader,
            String inputMasterPinHeader = defaultMasterPinHeader, String inputControlPinHeader = defaultControlPinHeader,
            String inputDriverPinHeader = defaultDriverPinHeader, String inputEmptyCellSpacer = defaultEmptyCellSpacer)
            : base(inputKeys, inputIsEndStoppedLeft, inputBottomPinHeader, inputMasterPinHeader, inputControlPinHeader,
            inputDriverPinHeader, inputEmptyCellSpacer)
        { }
        #endregion
        #region Instance Methods
        protected override void Initialize()
        {
            ControlKeys = GetControlKeys(Keys);
            OperatingKeys = GetOperatingKeys(Keys);

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

            RowHeaders = GenerateSFICRowHeaders(operatingRowCount, controlRowCount, BottomPinHeader, MasterPinHeader, ControlPinHeader, DriverPinHeader);
            Rows = GenerateSFICRows(operatingPins, controlPins, driverPins, EmptyCellSpacer);
            ColumnCount = GetMaxRowLength(Rows);
        }
        #endregion
        #region Static Methods
        public static ObservableCollection<KeyModel> GetControlKeys(IEnumerable<KeyModel> inputKeys)
        {
            ObservableCollection<KeyModel> outputKeys = new ObservableCollection<KeyModel>();

            foreach (KeyModel currentKey in inputKeys)
            {
                if (currentKey.IsControl)
                { outputKeys.Add(currentKey); }
            }

            return outputKeys;
        }
        public static ObservableCollection<KeyModel> GetOperatingKeys(IEnumerable<KeyModel> inputKeys)
        {
            ObservableCollection<KeyModel> outputKeys = new ObservableCollection<KeyModel>();

            foreach (KeyModel currentKey in inputKeys)
            {
                if (currentKey.IsControl == false)
                { outputKeys.Add(currentKey); }
            }

            return outputKeys;
        }
        public static List<List<int?>> GetControlPins(List<List<int?>> inputControlCuts, List<int?> inputDeepestOperatingCuts)
        {
            List<List<int?>> outputColumns = new List<List<int?>>();
            if (inputDeepestOperatingCuts.Count < 1 || ValidateCuts(inputControlCuts) == false)
            { return outputColumns; }

            int maxColumnHeight = GetMaxColumnHeight(inputControlCuts);
            int columnCount = inputDeepestOperatingCuts.Count;
            if (inputControlCuts.Count > inputDeepestOperatingCuts.Count)
            { columnCount = inputControlCuts.Count; }

            for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                List<int?> currentPinColumn = new List<int?>();
                List<int?> currentCutColumn = null;

                if (columnIndex < inputControlCuts.Count)
                { currentCutColumn = inputControlCuts[columnIndex]; }

                int? currentDeepestOperatingCut = null;

                if (columnIndex < inputDeepestOperatingCuts.Count)
                { currentDeepestOperatingCut = inputDeepestOperatingCuts[columnIndex]; }

                int? previousCut = null;

                for (int rowIndex = 0; rowIndex < maxColumnHeight; rowIndex++)
                {
                    if (currentCutColumn == null || rowIndex >= currentCutColumn.Count)
                    { currentPinColumn.Add(null); }
                    else
                    {
                        int? currentCut = null;
                        if (rowIndex < currentCutColumn.Count)
                        { currentCut = currentCutColumn[rowIndex]; }

                        if (currentCut == null || currentDeepestOperatingCut == null)
                        { currentPinColumn.Add(null); }
                        else if (previousCut == null)
                        { currentPinColumn.Add(currentCut + 10 - currentDeepestOperatingCut); }
                        else
                        { currentPinColumn.Add(currentCut - previousCut); }

                        previousCut = currentCut;
                    }
                }

                outputColumns.Add(currentPinColumn);
            }

            return outputColumns;
        }
        public static ObservableCollection<String> GetDriverPins(List<int?> inputDeepestControlCuts, List<int?> inputDeepestOperatingCuts, String inputEmptyCellSpacer)
        {
            ObservableCollection<String> output = new ObservableCollection<String>();

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
        public static ObservableCollection<String> GenerateSFICRowHeaders(int operatingRows, int controlRows, string inputBottomPinHeader = defaultBottomPinHeader, 
            string inputMasterPinHeader = defaultMasterPinHeader, string inputControlPinHeader = defaultControlPinHeader, 
            string inputDriverPinHeader = defaultDriverPinHeader)
        {
            ObservableCollection<String> output = GenerateStandardRowHeaders(operatingRows, inputBottomPinHeader, inputMasterPinHeader);

            if (output.Count < 1)
            { return output; }

            for (int rowIndex = 0; rowIndex < controlRows; rowIndex++)
            { output.Add(inputControlPinHeader); }

            output.Add(inputDriverPinHeader);

            return output;
        }
        public static ObservableCollection<ObservableCollection<String>> GenerateSFICRows(List<List<int?>> inputOperatingPins, List<List<int?>> inputControlPins, 
            ObservableCollection<String> inputDriverPins, String inputEmptyCellSpacer = defaultEmptyCellSpacer)
        {
            ObservableCollection<ObservableCollection<String>> output = GenerateStandardRows(inputOperatingPins, inputEmptyCellSpacer);

            if (output.Count < 1)
            { return output; }

            if (inputControlPins.Count > 0)
            {
                for (int rowIndex = 0; rowIndex < inputControlPins[0].Count; rowIndex++)
                { output.Add(GetRowAtIndex(inputControlPins, rowIndex, inputEmptyCellSpacer)); }
            }

            if (inputDriverPins.Count > 0)
            { output.Add(inputDriverPins); }

            return output;
        }
        #endregion
    }
}
