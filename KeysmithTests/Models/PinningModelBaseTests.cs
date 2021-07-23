using Microsoft.VisualStudio.TestTools.UnitTesting;
using Keysmith.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KeysmithTests.Models
{
    [TestClass()]
    public class PinningModelBaseTests
    {
        #region GetMaxKeyLength
        [TestMethod]
        public void GetMaxKeyLength_NullInputThrowsException()
        {
            List<KeyModel> input = null;

            Assert.ThrowsException<NullReferenceException>(()=> { PinningModelBase.GetMaxKeyLength(input); });
        }
        [TestMethod]
        public void GetMaxKeyLength_EmptyListReturnsZero()
        {
            List<KeyModel> input = new List<KeyModel>();
            int expected = 0;

            int output = PinningModelBase.GetMaxKeyLength(input);

            Assert.AreEqual(expected, output);
        }
        [TestMethod]
        public void GetMaxKeyLength_ListOfBlankKeysReturnsZero()
        {
            List<KeyModel> input = new List<KeyModel> { new KeyModel(), new KeyModel(), new KeyModel() };
            int expected = 0;

            int output = PinningModelBase.GetMaxKeyLength(input);

            Assert.AreEqual(expected, output);
        }
        [TestMethod]
        public void GetMaxKeyLength_ValidListReturnsLongestKeyLength()
        {
            List<KeyModel> input = new List<KeyModel>
            {
                new KeyModel(){ Cuts = "123" },
                new KeyModel(){ Cuts = "1234" },
                new KeyModel(){ Cuts = "123456" },
                new KeyModel(){ Cuts = "12345" }
            };
            int expected = 6;

            int output = PinningModelBase.GetMaxKeyLength(input);

            Assert.AreEqual(expected, output);
        }
        #endregion
        #region GetMinKeyLength
        [TestMethod]
        public void GetMinKeyLength_NullInputThrowsException()
        {
            List<KeyModel> input = null;

            Assert.ThrowsException<NullReferenceException>(() => { PinningModelBase.GetMinKeyLength(input); });
        }
        [TestMethod]
        public void GetMinKeyLength_EmptyListReturnsZero()
        {
            List<KeyModel> input = new List<KeyModel>();
            int expected = 0;

            int output = PinningModelBase.GetMinKeyLength(input);

            Assert.AreEqual(expected, output);
        }
        [TestMethod]
        public void GetMinKeyLength_ListOfBlankKeysReturnsZero()
        {
            List<KeyModel> input = new List<KeyModel> { new KeyModel(), new KeyModel(), new KeyModel() };
            int expected = 0;

            int output = PinningModelBase.GetMinKeyLength(input);

            Assert.AreEqual(expected, output);
        }
        [TestMethod]
        public void GetMinKeyLength_EmptyKeysIgnoredUnlessAllKeysEmpty()
        {
            List<KeyModel> input = new List<KeyModel>
            {
                new KeyModel(){ Cuts = "123456" },
                new KeyModel(){ Cuts = "123" },
                new KeyModel(),
                new KeyModel(){ Cuts = "12345" }
            };
            int expected = 3;

            int output = PinningModelBase.GetMinKeyLength(input);

            Assert.AreEqual(expected, output);
        }
        [TestMethod]
        public void GetMinKeyLength_ValidListReturnsShortestKeyLength()
        {
            List<KeyModel> input = new List<KeyModel>
            {
                new KeyModel(){ Cuts = "123456" },
                new KeyModel(){ Cuts = "123" },
                new KeyModel(){ Cuts = "1234" },
                new KeyModel(){ Cuts = "12345" }
            };
            int expected = 3;

            int output = PinningModelBase.GetMinKeyLength(input);

            Assert.AreEqual(expected, output);
        }
        #endregion
        #region PadKey
        [TestMethod]
        public void PadKey_NullInputThrowsException()
        {
            KeyModel input = null;

            Assert.ThrowsException<NullReferenceException>(() => { PinningModelBase.PadKey(input, 0); });
        }
        [TestMethod]
        public void PadKey_ZeroPaddingLengthReturnsEmptyList()
        {
            KeyModel input = new KeyModel() { Cuts = "123456" };
            List<int?> expected = new List<int?>();

            List<int?> output = PinningModelBase.PadKey(input, 0);

            Assert.AreEqual(expected.Count, output.Count);
        }
        [TestMethod]
        public void PadKey_PaddingLengthEqualsKeyLengthReturnsUnmodifiedCuts()
        {
            KeyModel input = new KeyModel() { Cuts = "123456" };
            List<int?> expected = new List<int?> { 1, 2, 3, 4, 5, 6 };

            List<int?> output = PinningModelBase.PadKey(input, 6);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        [TestMethod]
        public void PadKey_EndStoppedLeftTrimsRight()
        {
            KeyModel input = new KeyModel() { Cuts = "123456" };
            List<int?> expected = new List<int?> { 1, 2, 3 };

            List<int?> output = PinningModelBase.PadKey(input, 3, true);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        [TestMethod]
        public void PadKey_EndStoppedRightTrimsLeft()
        {
            KeyModel input = new KeyModel() { Cuts = "123456" };

            List<int?> expected = new List<int?> { 4, 5, 6 };

            List<int?> output = PinningModelBase.PadKey(input, 3, false);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        [TestMethod]
        public void PadKey_EndStoppedLeftPadsRight()
        {
            KeyModel input = new KeyModel() { Cuts = "123456" };

            List<int?> expected = new List<int?> { 1, 2, 3, 4, 5, 6, null, null, null };

            List<int?> output = PinningModelBase.PadKey(input, 9, true);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        [TestMethod]
        public void PadKey_EndStoppedRightPadsLeft()
        {
            KeyModel input = new KeyModel() { Cuts = "123456" };

            List<int?> expected = new List<int?> { null, null, null, 1, 2, 3, 4, 5, 6 };

            List<int?> output = PinningModelBase.PadKey(input, 9, false);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        #endregion
        #region GetPaddedKeys
        [TestMethod]
        public void GetPaddedKeys_NullInputThrowsException()
        {
            List<KeyModel> input = null;

            Assert.ThrowsException<NullReferenceException>(() => { PinningModelBase.GetPaddedKeys(input, 0); });
        }
        [TestMethod]
        public void GetPaddedKeys_EmptyInputListReturnsEmptyOutputList()
        {
            List<KeyModel> input = new List<KeyModel>();
            List<List<int?>> expected = new List<List<int?>>();

            List<List<int?>> output = PinningModelBase.GetPaddedKeys(input, 5);

            Assert.AreEqual(expected.Count, output.Count);
        }
        [TestMethod]
        public void GetPaddedKeys_ReturnedKeysAllHaveSpecifiedLength()
        {
            List<KeyModel> input = new List<KeyModel>
            {
                new KeyModel(){ Cuts = "123456" },
                new KeyModel(){ Cuts = "1234" },
                new KeyModel(){ Cuts = "12345678" }
            };
            int inputLength = 6;

            List<List<int?>> output = PinningModelBase.GetPaddedKeys(input, inputLength);

            foreach (List<int?> currentKey in output)
            { Assert.AreEqual(inputLength, currentKey.Count); }
        }
        #endregion
        #region GenerateEmptyPaddedKeys
        [TestMethod]
        public void GenerateEmptyPaddedKeys_ZeroLengthReturnsListOfOneEmptyKey()
        {
            int input = 0;
            List<List<int?>> expected = new List<List<int?>> { new List<int?>() };

            List<List<int?>> output = PinningModelBase.GenerateEmptyPaddedKeys(input);

            Assert.AreEqual(expected.Count, output.Count);
            
            for(int index = 0; index < output.Count;index++)
            { Assert.AreEqual(expected[index].Count, output[index].Count); }
        }
        [TestMethod]
        public void GenerateEmptyPaddedKeys_NonzeroInputReturnsOneKeyOfSpecifiedLengthAllNull()
        {
            int input = 6;
            List<List<int?>> expected = new List<List<int?>>
            {
                new List<int?>{ null, null, null, null, null, null }
            };

            List<List<int?>> output = PinningModelBase.GenerateEmptyPaddedKeys(input);

            Assert.AreEqual(expected.Count, output.Count);

            for(int keyIndex = 0; keyIndex < output.Count;keyIndex++)
            {
                List<int?> currentExpectedKey = expected[keyIndex];
                List<int?> currentOutputKey = expected[keyIndex];

                Assert.AreEqual(currentExpectedKey.Count, currentOutputKey.Count);

                for (int cutIndex = 0; cutIndex < currentOutputKey.Count; cutIndex++)
                { Assert.AreEqual(currentExpectedKey[cutIndex], currentOutputKey[cutIndex]); }
            }
        }
        #endregion
        #region GetSortedCutsAtIndex
        [TestMethod]
        public void GetSortedCutsAtIndex_NullInputThrowsException()
        {
            List<List<int?>> input = null;
            int inputIndex = 0;

            Assert.ThrowsException<NullReferenceException>(() => { PinningModelBase.GetSortedCutsAtIndex(input, inputIndex); });
        }
        [TestMethod]
        public void GetSortedCutsAtIndex_ValidIndexReturnsProperlySortedCutsWithNullsAndDuplicatesOmitted()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?> { null, null, 4, null },
                new List<int?> { null, null, 3, null },
                new List<int?> { null, null, 7, null },
                new List<int?> { null, null, 5, null },
                new List<int?> { null, null, null, null },
                new List<int?> { null, null, 1, null },
                new List<int?> { null, null, 3, null },
                new List<int?> { null, null, 7, null }
            };
            int inputIndex = 2;
            List<int?> expected = new List<int?> { 1, 3, 4, 5, 7 };

            List<int?> output = PinningModelBase.GetSortedCutsAtIndex(input, inputIndex);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        [TestMethod]
        public void GetSortedCutsAtIndex_IndexOutOfRangeThrowsException()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?> { null, null, 4, null },
                new List<int?> { null, null, 3, null },
                new List<int?> { null, null, 5, null },
                new List<int?> { null, null, 1, null },
                new List<int?> { null, null, 7, null },
            };

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { PinningModelBase.GetSortedCutsAtIndex(input, -1); });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { PinningModelBase.GetSortedCutsAtIndex(input, 7); });
        }
        #endregion
        #region GetMaxColumnHeight
        [TestMethod]
        public void GetMaxColumnHeight_NullInputThrowsException()
        {
            List<List<int?>> input = null;

            Assert.ThrowsException<NullReferenceException>(() => { PinningModelBase.GetMaxColumnHeight(input); });
        }
        [TestMethod]
        public void GetMaxColumnHeight_ValidDataReturnsTallestColumnHeight()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?>{ 1, 2, 3, 4 },
                new List<int?>{ 1, 2, 3 },
                new List<int?>{ 1, 2, 3, 4, 5 },
                new List<int?>{ 1, 2 }
            };
            int expected = 5;

            int output = PinningModelBase.GetMaxColumnHeight(input);

            Assert.AreEqual(expected, output);
        }
        [TestMethod]
        public void GetMaxColumnHeight_EmptyInputReturnsZeroHeight()
        {
            List<List<int?>> input = new List<List<int?>>();
            int expected = 0;

            int output = PinningModelBase.GetMaxColumnHeight(input);

            Assert.AreEqual(expected, output);
        }
        [TestMethod]
        public void GetMaxColumnHeight_InputListOfEmptyListsReturnsZeroHeight()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?>(),
                new List<int?>(),
                new List<int?>()
            };
            int expected = 0;

            int output = PinningModelBase.GetMaxColumnHeight(input);

            Assert.AreEqual(expected, output);
        }
        #endregion
        #region GetSortedCuts
        [TestMethod]
        public void GetSortedCuts_NullInputThrowsException()
        {
            List<List<int?>> input = null;

            Assert.ThrowsException<NullReferenceException>(() => { PinningModelBase.GetSortedCuts(input); });
        }
        [TestMethod]
        public void GetSortedCuts_EmptyInputReturnsEmptyOutput()
        {
            List<List<int?>> input = new List<List<int?>>();
            List<List<int?>> expected = new List<List<int?>>();

            List<List<int?>> output = PinningModelBase.GetSortedCuts(input);

            Assert.AreEqual(expected.Count, output.Count);
        }
        [TestMethod]
        public void GetSortedCuts_UnequalLengthKeysThrowsException()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?>{ 1, 2, 3, 4, 5, 6 },
                new List<int?>{ 4, 3, 5, 1, 2 }
            };

            Assert.ThrowsException<ArgumentException>(() => { PinningModelBase.GetSortedCuts(input); });
        }
        [TestMethod]
        public void GetSortedCuts_ValidInputReturnsCorrectOutput()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?>{ 1, 2, 3, 4, 5, 6 },
                new List<int?>{ 4, 3, 5, 1, 2, 7 },
                new List<int?>{ 1, 3, 3, null, 2, 7 },
                new List<int?>{ 5, 1, 6, 3, 1, 2 }
            };
            List<List<int?>> expected = new List<List<int?>>
            {
                new List<int?>{ 1, 4, 5 },
                new List<int?>{ 1, 2, 3 },
                new List<int?>{ 3, 5, 6 },
                new List<int?>{ 1, 3, 4 },
                new List<int?>{ 1, 2, 5 },
                new List<int?>{ 2, 6, 7 }
            };

            List<List<int?>> output = PinningModelBase.GetSortedCuts(input);

            Assert.AreEqual(expected.Count, output.Count);

            for (int columnIndex = 0; columnIndex < output.Count; columnIndex++)
            {
                List<int?> currentExpectedColumn = expected[columnIndex];
                List<int?> currentOutputColumn = output[columnIndex];

                Assert.AreEqual(currentExpectedColumn.Count, currentOutputColumn.Count);

                for (int rowIndex = 0; rowIndex < currentOutputColumn.Count; rowIndex++)
                { Assert.AreEqual(currentExpectedColumn[rowIndex], currentOutputColumn[rowIndex]); }
            }
        }
        #endregion
        #region ValidateCuts
        [TestMethod]
        public void ValidateCuts_NullInputThrowsException()
        {
            List<int?> input = null;

            Assert.ThrowsException<NullReferenceException>(() => { PinningModelBase.ValidateCuts(input); });
        }
        [TestMethod]
        public void ValidateCuts_EmptyInputReturnsFalse()
        {
            List<int?> input = new List<int?>();

            bool output = PinningModelBase.ValidateCuts(input);

            Assert.IsFalse(output);
        }
        [TestMethod]
        public void ValidateCuts_InputContainsOnlySingleNullThrowsException()
        {
            List<int?> input = new List<int?> { null };

            Assert.ThrowsException<ArgumentNullException>(() => { PinningModelBase.ValidateCuts(input); });
        }
        [TestMethod]
        public void ValidateCuts_InputContainsNullThrowsException()
        {
            List<int?> input = new List<int?> { 1, 2, 3, null, 5, 6 };

            Assert.ThrowsException<ArgumentNullException>(() => { PinningModelBase.ValidateCuts(input); });

            input = new List<int?> { null, 2, 3, 4, 5, 6 };

            Assert.ThrowsException<ArgumentNullException>(() => { PinningModelBase.ValidateCuts(input); });
        }
        [TestMethod]
        public void ValidateCuts_InputContainsDuplicatesThrowsException()
        {
            List<int?> input = new List<int?> { 1, 2, 3, 3, 5, 6 };

            Assert.ThrowsException<ArgumentException>(() => { PinningModelBase.ValidateCuts(input); });
        }
        [TestMethod]
        public void ValidateCuts_UnsortedInputThrowsException()
        {
            List<int?> input = new List<int?> { 2, 3, 1, 4, 6, 5 };

            Assert.ThrowsException<ArgumentException>(() => { PinningModelBase.ValidateCuts(input); });
        }
        [TestMethod]
        public void ValidateCuts_ValidInputReturnsTrue()
        {
            List<int?> input = new List<int?> { 1, 2, 3 };

            bool output = PinningModelBase.ValidateCuts(input);

            Assert.IsTrue(output);
        }
        #endregion
        #region ValidateCuts_ListOverload
        [TestMethod]
        public void ValidateCuts_ListOverload_NullInputThrowsException()
        {
            List<List<int?>> input = null;

            Assert.ThrowsException<NullReferenceException>(() => { PinningModelBase.ValidateCuts(input); });
        }
        [TestMethod]
        public void ValidateCuts_ListOverload_EmptyInputReturnsFalse()
        {
            List<List<int?>> input = new List<List<int?>>();

            Assert.IsFalse(PinningModelBase.ValidateCuts(input));
        }
        [TestMethod]
        public void ValidateCuts_ListOverload_InputContainsNullThrowsException()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?> { 1, 2, 3 },
                new List<int?> { 4, null, 6 },
                new List<int?> { 7, 8, 9 }
            };

            Assert.ThrowsException<ArgumentNullException>(() => { PinningModelBase.ValidateCuts(input); });

            input = new List<List<int?>>
            {
                new List<int?> { null, 2, 3 },
                new List<int?> { 4, 5, 6 },
                new List<int?> { 7, 8, 9 }
            };

            Assert.ThrowsException<ArgumentNullException>(() => { PinningModelBase.ValidateCuts(input); });
        }
        [TestMethod]
        public void ValidateCuts_ListOverload_InputContainsDuplicatesThrowsException()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?> { 1, 2, 3 },
                new List<int?> { 4, 5, 5 },
                new List<int?> { 7, 8, 9 }
            };

            Assert.ThrowsException<ArgumentException>(() => { PinningModelBase.ValidateCuts(input); });
        }
        [TestMethod]
        public void ValidateCuts_ListOverload_UnsortedInputThrowsException()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?> { 1, 2, 3 },
                new List<int?> { 4, 5, 6 },
                new List<int?> { 7, 8, 9 },
                new List<int?> { 2, 3, 1, 4, 6, 5 }
            };

            Assert.ThrowsException<ArgumentException>(() => { PinningModelBase.ValidateCuts(input); });
        }
        [TestMethod]
        public void ValidateCuts_ListOverload_ValidInputReturnsTrue()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?> { 1, 2, 3 },
                new List<int?> { 4, 5, 6 },
                new List<int?> { 7, 8, 9 },
                new List<int?>()
            };

            Assert.IsTrue(PinningModelBase.ValidateCuts(input));
        }
        #endregion
        #region GetDeepestCut
        [TestMethod]
        public void GetDeepestCut_NullInputThrowsException()
        {
            List<int?> input = null;

            Assert.ThrowsException<NullReferenceException>(() => { PinningModelBase.GetDeepestCut(input); });
        }
        [TestMethod]
        public void GetDeepestCut_EmptyInputReturnsNull()
        {
            List<int?> input = new List<int?>();

            int? output = PinningModelBase.GetDeepestCut(input);

            Assert.IsNull(output);
        }
        [TestMethod]
        public void GetDeepestCut_InputContainsNullThrowsException()
        {
            List<int?> input = new List<int?> { 1, 2, 3, null, 5, 6 };

            Assert.ThrowsException<ArgumentNullException>(() => { PinningModelBase.GetDeepestCut(input); });
        }
        [TestMethod]
        public void GetDeepestCut_UnsortedInputThrowsException()
        {
            List<int?> input = new List<int?> { 2, 3, 1, 4, 6, 5 };

            Assert.ThrowsException<ArgumentException>(() => { PinningModelBase.GetDeepestCut(input); });
        }
        [TestMethod]
        public void GetDeepestCut_ValidInputReturnsDeepestCut()
        {
            List<int?> input = new List<int?> { 1, 2, 3, 4, 5, 6 };
            int expected = 6;

            int? output = PinningModelBase.GetDeepestCut(input);

            Assert.AreEqual(expected, output);
        }
        #endregion
        #region GetDeepestCuts
        [TestMethod]
        public void GetDeepestCuts_NullInputThrowsException()
        {
            List<List<int?>> input = null;

            Assert.ThrowsException<NullReferenceException>(() => { PinningModelBase.GetDeepestCuts(input); });
        }
        [TestMethod]
        public void GetDeepestCuts_EmptyInputReturnsEmptyOutput()
        {
            List<List<int?>> input = new List<List<int?>>();
            List<int?> expected = new List<int?>();

            List<int?> output = PinningModelBase.GetDeepestCuts(input);

            Assert.AreEqual(expected.Count, output.Count);
        }
        [TestMethod]
        public void GetDeepestCuts_InputListOfEmptyColumnsReturnsListOfNulls()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?>(),
                new List<int?>(),
                new List<int?>()
            };
            List<int?> expected = new List<int?> { null, null, null };

            List<int?> output = PinningModelBase.GetDeepestCuts(input);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        [TestMethod]
        public void GetDeepestCuts_ValidInputReturnsCorrectResults()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?> { 1, 2, 3, 4 },
                new List<int?> { 3, 4, 5 },
                new List<int?> { 1, 2, 3, 4, 5, 6 },
                new List<int?> { 4, 5, 6, 7 }
            };
            List<int?> expected = new List<int?> { 4, 5, 6, 7 };

            List<int?> output = PinningModelBase.GetDeepestCuts(input);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        #endregion
        #region CalculatePinColumn
        [TestMethod]
        public void CalculatePinColumn_NullInputThrowsException()
        {
            List<int?> input = null;

            Assert.ThrowsException<NullReferenceException>(() => { PinningModelBase.CalculatePinColumn(input); });
        }
        [TestMethod]
        public void CalculatePinColumn_EmptyInputReturnsEmptyOutput()
        {
            List<int?> input = new List<int?>();
            List<int?> expected = new List<int?>();

            List<int?> output = PinningModelBase.CalculatePinColumn(input);

            Assert.AreEqual(expected.Count, output.Count);
        }
        [TestMethod]
        public void CalculatePinColumn_InputContainsNullThrowsException()
        {
            List<int?> input = new List<int?> { 1, 2, null, 3 };

            Assert.ThrowsException<ArgumentNullException>(() => { PinningModelBase.CalculatePinColumn(input); });
        }
        [TestMethod]
        public void CalculatePinColumn_UnsortedInputThrowsException()
        {
            List<int?> input = new List<int?> { 3, 1, 2, 4 };

            Assert.ThrowsException<ArgumentException>(() => { PinningModelBase.CalculatePinColumn(input); });
        }
        [TestMethod]
        public void CalculatePinColumn_ValidInputReturnsCorrectOutput()
        {
            List<int?> input = new List<int?> { 1, 3, 4, 9 };
            List<int?> expected = new List<int?> { 1, 2, 1, 5 };

            List<int?> output = PinningModelBase.CalculatePinColumn(input);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        #endregion
        #region PadColumn
        [TestMethod]
        public void PadColumn_NullInputThrowsException()
        {
            List<int?> input = null;
            int inputPaddingHeight = 5;

            Assert.ThrowsException<NullReferenceException>(() => { PinningModelBase.PadColumn(input, inputPaddingHeight); });
        }
        [TestMethod]
        public void PadColumn_EmptyInputReturnsColumnOfNulls()
        {
            List<int?> input = new List<int?>();
            int inputPaddingHeight = 5;
            List<int?> expected = new List<int?> { null, null, null, null, null };

            List<int?> output = PinningModelBase.PadColumn(input, inputPaddingHeight);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        [TestMethod]
        public void PadColumn_PaddingHeightEqualsInputHeightReturnsUnchangedInput()
        {
            List<int?> input = new List<int?> { 1, 2, 3, 4, 5 };
            int inputPaddingHeight = 5;
            List<int?> expected = input;

            List<int?> output = PinningModelBase.PadColumn(input, inputPaddingHeight);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        [TestMethod]
        public void PadColumn_PaddingHeightGreaterThanInputReturnsPaddedInput()
        {
            List<int?> input = new List<int?> { 1, 2, 3, 4, 5 };
            int inputPaddingHeight = 7;
            List<int?> expected = new List<int?> { 1, 2, 3, 4, 5, null, null };

            List<int?> output = PinningModelBase.PadColumn(input, inputPaddingHeight);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        [TestMethod]
        public void PadColumn_PaddingHeightLessThanInputThrowsException()
        {
            List<int?> input = new List<int?> { 1, 2, 3, 4, 5 };
            int inputPaddingHeight = 2;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { PinningModelBase.PadColumn(input, inputPaddingHeight); });
        }
        #endregion
        #region PadColumns
        [TestMethod]
        public void PadColumns_NullInputThrowsException()
        {
            List<List<int?>> input = null;

            Assert.ThrowsException<NullReferenceException>(() => { PinningModelBase.PadColumns(input); });
        }
        [TestMethod]
        public void PadColumns_EmptyInputReturnsEmptyOutput()
        {
            List<List<int?>> input = new List<List<int?>>();
            List<List<int?>> expected = new List<List<int?>>();

            List<List<int?>> output = PinningModelBase.PadColumns(input);

            Assert.AreEqual(expected.Count, output.Count);
        }
        [TestMethod]
        public void PadColumns_ValidInputReturnsCorrectOutput()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?>{ 1, 2, 3 },
                new List<int?>{ 1, 2, 3, 4, 5 },
                new List<int?>{ 1, 2 }
            };
            List<List<int?>> expected = new List<List<int?>>
            {
                new List<int?>{ 1, 2, 3, null, null },
                new List<int?>{ 1, 2, 3, 4, 5 },
                new List<int?>{ 1, 2, null, null, null }
            };

            List<List<int?>> output = PinningModelBase.PadColumns(input);

            Assert.AreEqual(expected.Count, output.Count);

            for(int columnIndex = 0;columnIndex<output.Count;columnIndex++)
            {
                List<int?> currentExpectedColumn = expected[columnIndex];
                List<int?> currentOutputColumn = output[columnIndex];

                Assert.AreEqual(currentExpectedColumn.Count, currentOutputColumn.Count);

                for (int rowIndex = 0; rowIndex < currentOutputColumn.Count; rowIndex++)
                { Assert.AreEqual(currentExpectedColumn[rowIndex], currentOutputColumn[rowIndex]); }
            }
        }
        #endregion
        #region GetOperatingPins
        [TestMethod]
        public void GetOperatingPins_NullInputThrowsException()
        {
            List<List<int?>> input = null;

            Assert.ThrowsException<NullReferenceException>(() => { PinningModelBase.GetOperatingPins(input); });
        }
        [TestMethod]
        public void GetOperatingPins_EmptyInputReturnsEmptyOutput()
        {
            List<List<int?>> input = new List<List<int?>>();
            List<List<int?>> expected = new List<List<int?>>();

            List<List<int?>> output = PinningModelBase.GetOperatingPins(input);

            Assert.AreEqual(expected.Count, output.Count);
        }
        [TestMethod]
        public void GetOperatingPins_ValidInputReturnsCorrectOutput()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?>{ 1, 2, 3, 4, 5, 6 },
                new List<int?>{ 3, 4, 5 },
                new List<int?>{ 6, 7, 8, 9 },
                new List<int?>{ 2, 3 },
                new List<int?>{ 2, 4, 8, 9 }
            };
            List<List<int?>> expected = new List<List<int?>>
            {
                new List<int?>{ 1, 1, 1, 1, 1, 1 },
                new List<int?>{ 3, 1, 1, null, null, null },
                new List<int?>{ 6, 1, 1, 1, null, null },
                new List<int?>{ 2, 1, null, null, null, null },
                new List<int?>{ 2, 2, 4, 1, null, null }
            };

            List<List<int?>> output = PinningModelBase.GetOperatingPins(input);

            Assert.AreEqual(expected.Count, output.Count);

            for (int columnIndex = 0; columnIndex < output.Count; columnIndex++)
            {
                List<int?> currentExpectedColumn = expected[columnIndex];
                List<int?> currentOutputColumn = output[columnIndex];

                Assert.AreEqual(currentExpectedColumn.Count, currentOutputColumn.Count);

                for (int rowIndex = 0; rowIndex < currentOutputColumn.Count; rowIndex++)
                { Assert.AreEqual(currentExpectedColumn[rowIndex], currentOutputColumn[rowIndex]); }
            }

        }
        #endregion
        #region GetRowAtIndex
        [TestMethod]
        public void GetRowAtIndex_NullInputThrowsException()
        {
            List<List<int?>> input = null;
            int inputIndex = 0;

            Assert.ThrowsException<NullReferenceException>(() => { PinningModelBase.GetRowAtIndex(input, inputIndex); });
        }
        [TestMethod]
        public void GetRowAtIndex_IndexOutOfRangeThrowsException()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?>{1, 2, 3},
                new List<int?>{4, 5, 6},
                new List<int?>{7, 8, 9, 10}
            };
            int inputIndexLow = -1;
            int inputIndexHigh = 5;

            Assert.ThrowsException<IndexOutOfRangeException>(() => { PinningModelBase.GetRowAtIndex(input, inputIndexLow); });
            Assert.ThrowsException<IndexOutOfRangeException>(() => { PinningModelBase.GetRowAtIndex(input, inputIndexHigh); });
        }
        [TestMethod]
        public void GetRowAtIndex_EmptyInputThrowsException()
        {
            List<List<int?>> input = new List<List<int?>>();
            int inputIndex = 0;

            Assert.ThrowsException<IndexOutOfRangeException>(() => { PinningModelBase.GetRowAtIndex(input, inputIndex); });
        }
        [TestMethod]
        public void GetRowAtIndex_ValidInputWithCompleteRowReturnsCorrectResult()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?>{1, 2, 3},
                new List<int?>{4, 5, 6},
                new List<int?>{7, 8, 9, 10}
            };
            int inputIndex = 0;
            ObservableCollection<string> expected = new ObservableCollection<string> { "1", "4", "7" };

            ObservableCollection<string> output = PinningModelBase.GetRowAtIndex(input, inputIndex);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        [TestMethod]
        public void GetRowAtIndex_ValidInputWithInompleteRowReturnsResultWithCorrectSpacer()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?>{1, 2, 3},
                new List<int?>{4, 5, 6},
                new List<int?>{7, 8, 9, 10}
            };

            int inputIndex = 3;
            string inputSpacer = "X";

            ObservableCollection<string> expected = new ObservableCollection<string> { "X", "X", "10" };

            ObservableCollection<string> output = PinningModelBase.GetRowAtIndex(input, inputIndex, inputSpacer);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        #endregion
        #region CountRowsFromColumns
        [TestMethod]
        public void CountRowsFromColumns_NullInputThrowsException()
        {
            List<List<int?>> input = null;

            Assert.ThrowsException<NullReferenceException>(() => { PinningModelBase.CountRowsFromColumns(input); });
        }
        [TestMethod]
        public void CountRowsFromColumns_EmptyInputReturnsZero()
        {
            List<List<int?>> input = new List<List<int?>>();
            int expected = 0;

            int output = PinningModelBase.CountRowsFromColumns(input);

            Assert.AreEqual(expected, output);
        }
        [TestMethod]
        public void CountRowsFromColumns_ValidInputReturnsCorrectCount()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?>{1, 2, 3},
                new List<int?>{4, 5, 6},
                new List<int?>{7, 8, 9, 10}
            };

            int expectedOutput = 4;

            int output = PinningModelBase.CountRowsFromColumns(input);

            Assert.AreEqual(expectedOutput, output);
        }
        #endregion
        #region GenerateStandardRowHeaders
        [TestMethod]
        public void GenerateStandardRowHeaders_ZeroInputCountReturnsEmptyOutput()
        {
            int inputCount = 0;
            ObservableCollection<string> expected = new ObservableCollection<string>();

            ObservableCollection<string> output = PinningModelBase.GenerateStandardRowHeaders(inputCount);

            Assert.AreEqual(expected.Count, output.Count);
        }
        [TestMethod]
        public void GenerateStandardRowHeaders_OneInputCountReturnsBottomHeaderOnly()
        {
            int inputCount = 1;
            string inputBottomPinHeader = "bottomHeader";
            ObservableCollection<string> expected = new ObservableCollection<string> { inputBottomPinHeader };

            ObservableCollection<string> output = PinningModelBase.GenerateStandardRowHeaders(inputCount, inputBottomPinHeader);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        [TestMethod]
        public void GenerateStandardRowHeaders_FourInputCountReturnsBottomHeaderAndThreeMasterHeaders()
        {
            int inputCount = 4;
            string inputBottomPinHeader = "bottomHeader";
            string inputMasterPinHeader = "masterHeader";
            ObservableCollection<string> expected = new ObservableCollection<string> { inputBottomPinHeader, inputMasterPinHeader, inputMasterPinHeader, inputMasterPinHeader };

            ObservableCollection<string> output = PinningModelBase.GenerateStandardRowHeaders(inputCount, inputBottomPinHeader, inputMasterPinHeader);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        #endregion
        #region GenerateStandardRows
        [TestMethod]
        public void GenerateStandardRows_NullInputThrowsException()
        {
            List<List<int?>> input = null;

            Assert.ThrowsException<NullReferenceException>(() => { PinningModelBase.GenerateStandardRows(input); });
        }
        [TestMethod]
        public void GenerateStandardRows_EmptyInputReturnsEmptyOutput()
        {
            List<List<int?>> input = new List<List<int?>>();
            int expectedCount = 0;

            ObservableCollection<ObservableCollection<string>> output = PinningModelBase.GenerateStandardRows(input);

            Assert.AreEqual(expectedCount, output.Count);
        }
        [TestMethod]
        public void GenerateStandardRows_ValidInputReturnsCorrectNumberOfRows()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?>{1, 2, 3},
                new List<int?>{4, 5, 6},
                new List<int?>{7, 8, 9, 10}
            };

            int expectedCount = 4;

            ObservableCollection<ObservableCollection<string>> output = PinningModelBase.GenerateStandardRows(input);

            Assert.AreEqual(expectedCount, output.Count);
        }
        #endregion
        #region GetMaxRowLength
        [TestMethod]
        public void GetMaxRowLength_NullInputThrowsException()
        {
            ObservableCollection<ObservableCollection<string>> input = null;

            Assert.ThrowsException<NullReferenceException>(() => { PinningModelBase.GetMaxRowLength(input); });
        }
        [TestMethod]
        public void GetMaxRowLength_EmptyInputReturnsZeroRowLength()
        {
            ObservableCollection<ObservableCollection<string>> input = new ObservableCollection<ObservableCollection<string>>();
            int expected = 0;

            int output = PinningModelBase.GetMaxRowLength(input);

            Assert.AreEqual(expected, output);
        }
        [TestMethod]
        public void GetMaxRowLength_ValidInputReturnsCorrectRowLength()
        {
            ObservableCollection<ObservableCollection<string>> input = new ObservableCollection<ObservableCollection<string>>
            {
                new ObservableCollection<string>{ "1", "2" },
                new ObservableCollection<string>{ "A", "B", "C", "D" },
                new ObservableCollection<string>{ "X", "Y", "Z" }
            };
            int expected = 4;

            int output = PinningModelBase.GetMaxRowLength(input);

            Assert.AreEqual(expected, output);
        }
        #endregion
    }
}