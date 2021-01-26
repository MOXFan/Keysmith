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

            RowHeaders = GenerateRowHeaders(operatingRowCount, controlRowCount, BottomPinHeader, MasterPinHeader, ControlPinHeader, DriverPinHeader);
            Rows = GenerateRows(operatingPins, controlPins, driverPins, EmptyCellSpacer);
            ColumnCount = GetMaxRowLength(Rows);
        }
        #endregion
        #region Static Methods
        protected static ObservableCollection<KeyModel> GetControlKeys(IEnumerable<KeyModel> inputKeys)
        {
            ObservableCollection<KeyModel> outputKeys = new ObservableCollection<KeyModel>();

            foreach (KeyModel currentKey in inputKeys)
            {
                if (currentKey.IsControl)
                { outputKeys.Add(currentKey); }
            }

            return outputKeys;
        }
        protected static ObservableCollection<KeyModel> GetOperatingKeys(IEnumerable<KeyModel> inputKeys)
        {
            ObservableCollection<KeyModel> outputKeys = new ObservableCollection<KeyModel>();

            foreach (KeyModel currentKey in inputKeys)
            {
                if (currentKey.IsControl == false)
                { outputKeys.Add(currentKey); }
            }

            return outputKeys;
        }
        protected static List<List<int?>> GetControlPins(List<List<int?>> inputControlCuts, List<int?> inputDeepestOperatingCuts)
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
        protected static ObservableCollection<String> GetDriverPins(List<int?> inputDeepestControlCuts, List<int?> inputDeepestOperatingCuts, String inputEmptyCellSpacer)
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
        protected static int CountRowsFromColumns(List<List<int?>> inputColumns)
        {
            int output = 0;

            foreach (List<int?> currentColumn in inputColumns)
            {
                if (currentColumn.Count > output)
                { output = currentColumn.Count; }
            }

            return output;
        }
        protected static ObservableCollection<String> GenerateRowHeaders(int operatingRows, int controlRows, string inputBottomPinHeader = defaultBottomPinHeader, 
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
        protected static ObservableCollection<ObservableCollection<String>> GenerateRows(List<List<int?>> inputOperatingPins, List<List<int?>> inputControlPins, 
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
        #endregion
    }
}
