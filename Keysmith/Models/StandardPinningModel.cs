﻿using System;
using System.Collections.Generic;

namespace Keysmith.Models
{
    class StandardPinningModel : PinningModelBase
    {
        #region Constructors
        public StandardPinningModel() { }
        public StandardPinningModel(IEnumerable<KeyModel> inputKeys, bool inputIsEndStoppedLeft = true, String inputBottomPinHeader = defaultBottomPinHeader,
            String inputMasterPinHeader = defaultMasterPinHeader, String inputControlPinHeader = defaultControlPinHeader,
            String inputDriverPinHeader = defaultDriverPinHeader, String inputEmptyCellSpacer = defaultEmptyCellSpacer)
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
            { paddedKeys = GenerateEmptyPaddedKeys(6); }

            List<List<int?>> cuts = GetSortedCuts(paddedKeys);

            List<List<int?>> pins = GetOperatingPins(cuts);

            int rowCount = CountRowsFromColumns(pins);

            RowHeaders = GenerateStandardRowHeaders(rowCount, BottomPinHeader, MasterPinHeader);
            Rows = GenerateStandardRows(pins, EmptyCellSpacer);
            ColumnCount = GetMaxRowLength(Rows);
        }
        #endregion
    }
}
