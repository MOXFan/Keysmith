﻿using System;
using System.Collections.Generic;

namespace Keysmith.Models
{
    public class AbloyPinningModel : PinningModelBase
    {
        #region Constant Values
        protected const string abloyDriverPinHeader = "";
        protected const string abloyControlPinHeader = "";
        protected const string abloyMasterPinHeader = "";
        protected const string abloyBottomPinHeader = "Discs";
        #endregion
        #region Constructors
        public AbloyPinningModel() { }
        public AbloyPinningModel(IEnumerable<KeyModel> inputKeys, bool inputIsEndStoppedLeft = true, String inputBottomPinHeader = abloyBottomPinHeader,
            String inputMasterPinHeader = abloyMasterPinHeader, String inputControlPinHeader = abloyControlPinHeader,
            String inputDriverPinHeader = abloyDriverPinHeader, String inputEmptyCellSpacer = defaultEmptyCellSpacer)
            : base(inputKeys, inputIsEndStoppedLeft, inputBottomPinHeader, inputMasterPinHeader, inputControlPinHeader,
            inputDriverPinHeader, inputEmptyCellSpacer)
        { }
        #endregion
        #region Instance Methods
        protected override void Initialize()
        {
            int minKeyLength = GetMinKeyLength(Keys);

            List<List<int?>> paddedKeys = GetPaddedKeys(Keys, minKeyLength, IsEndStoppedLeft);

            if (minKeyLength < 1)
            { paddedKeys = GenerateEmptyPaddedKeys(10); }

            List<List<int?>> cuts = GetSortedCuts(paddedKeys);

            int rowCount = CountRowsFromColumns(cuts);

            RowHeaders = GenerateStandardRowHeaders(rowCount, BottomPinHeader, MasterPinHeader);
            Rows = GenerateStandardRows(cuts, EmptyCellSpacer);
            ColumnCount = GetMaxRowLength(Rows);
        }
        #endregion
    }
}
