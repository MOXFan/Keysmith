using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Keysmith.Models.Tests
{
    [TestClass()]
    public class PinningModelBaseTests
    {
        #region GetMaxKeyLength
        [TestMethod]
        public void GetMaxKeyLength_EmptyListReturnsZero()
        {
            List<KeyModel> input = new List<KeyModel>();

            int output = PinningModelBase.GetMaxKeyLength(input);

            Assert.AreEqual(output, 0);
        }
        [TestMethod]
        public void GetMaxKeyLength_ListOfBlankKeysReturnsZero()
        {
            List<KeyModel> input = new List<KeyModel> { new KeyModel(), new KeyModel(), new KeyModel() };

            int output = PinningModelBase.GetMaxKeyLength(input);

            Assert.AreEqual(output, 0);
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

            int output = PinningModelBase.GetMaxKeyLength(input);

            Assert.AreEqual(output, 6);
        }
        #endregion
        #region PadKey
        [TestMethod]
        public void PadKey_ZeroPaddingLengthReturnsEmptyList()
        {
            KeyModel input = new KeyModel() { Cuts = "123456" };

            List<int?> output = PinningModelBase.PadKey(input, 0);

            if (output.Count != 0)
            { Assert.Fail($"Output has length {output.Count} instead of zero."); }
        }
        [TestMethod]
        public void PadKey_PaddingLengthEqualsKeyLengthReturnsUnmodifiedCuts()
        {
            KeyModel input = new KeyModel() { Cuts = "123456" };

            List<int?> expected = new List<int?> { 1, 2, 3, 4, 5, 6 };

            List<int?> output = PinningModelBase.PadKey(input, 6);

            if (output.Count != expected.Count)
            { Assert.Fail("Output has wrong length."); }

            for (int index = 0; index < output.Count; index++)
            {
                if (output[index] != expected[index])
                { Assert.Fail($"Output has wrong value at index {index}"); }
            }
        }
        [TestMethod]
        public void PadKey_EndStoppedLeftTrimsRight()
        {
            KeyModel input = new KeyModel() { Cuts = "123456" };

            List<int?> expected = new List<int?> { 1, 2, 3 };

            List<int?> output = PinningModelBase.PadKey(input, 3, true);

            if (output.Count != expected.Count)
            { Assert.Fail("Output has wrong length."); }

            for (int index = 0; index < output.Count; index++)
            {
                if (output[index] != expected[index])
                { Assert.Fail($"Output has wrong value at index {index}"); }
            }
        }
        [TestMethod]
        public void PadKey_EndStoppedRightTrimsLeft()
        {
            KeyModel input = new KeyModel() { Cuts = "123456" };

            List<int?> expected = new List<int?> { 4, 5, 6 };

            List<int?> output = PinningModelBase.PadKey(input, 3, false);

            if (output.Count != expected.Count)
            { Assert.Fail("Output has wrong length."); }

            for (int index = 0; index < output.Count; index++)
            {
                if (output[index] != expected[index])
                { Assert.Fail($"Output has wrong value at index {index}"); }
            }
        }
        [TestMethod]
        public void PadKey_EndStoppedLeftPadsRight()
        {
            KeyModel input = new KeyModel() { Cuts = "123456" };

            List<int?> expected = new List<int?> { 1, 2, 3, 4, 5, 6, null, null, null };

            List<int?> output = PinningModelBase.PadKey(input, 9, true);

            if (output.Count != expected.Count)
            { Assert.Fail("Output has wrong length."); }

            for (int index = 0; index < output.Count; index++)
            {
                if (output[index] != expected[index])
                { Assert.Fail($"Output has wrong value at index {index}"); }
            }
        }
        [TestMethod]
        public void PadKey_EndStoppedRightPadsLeft()
        {
            KeyModel input = new KeyModel() { Cuts = "123456" };

            List<int?> expected = new List<int?> { null, null, null, 1, 2, 3, 4, 5, 6 };

            List<int?> output = PinningModelBase.PadKey(input, 9, false);

            if (output.Count != expected.Count)
            { Assert.Fail("Output has wrong length."); }

            for (int index = 0; index < output.Count; index++)
            {
                if (output[index] != expected[index])
                { Assert.Fail($"Output has wrong value at index {index}"); }
            }
        }
        #endregion
        #region GetPaddedKeys
        [TestMethod]
        public void GetPaddedKeys_EmptyInputListReturnsEmptyOutputList()
        {
            List<KeyModel> input = new List<KeyModel>();

            List<List<int?>> output = PinningModelBase.GetPaddedKeys(input, 5);

            if (output.Count != 0)
            { Assert.Fail($"Output count was {output.Count} instead of zero."); }
        }
        [TestMethod]
        public void GetPaddedKeys_ReturnedKeysAllHaveSameLength()
        {
            List<KeyModel> input = new List<KeyModel>
            {
                new KeyModel(){ Cuts = "123456" },
                new KeyModel(){ Cuts = "1234" },
                new KeyModel(){ Cuts = "12345678" }
            };

            List<List<int?>> output = PinningModelBase.GetPaddedKeys(input, 6);

            for (int index = 0; index < output.Count; index++)
            {
                int currentLength = output[index].Count;

                if (currentLength != 6)
                { Assert.Fail($"Result at index {index} had length {currentLength} instead of 6."); }
            }
        }
        #endregion
        #region GenerateEmptyPaddedKeys
        [TestMethod]
        public void GenerateEmptyPaddedKeys_ZeroLengthReturnsListOfOneEmptyKey()
        {
            List<List<int?>> output = PinningModelBase.GenerateEmptyPaddedKeys(0);

            if (output.Count != 1)
            { Assert.Fail($"Output contained {output.Count} records instead of one."); }
            else if (output[0].Count != 0)
            { Assert.Fail($"Output key was length {output[0].Count} instead of zero."); }
        }
        [TestMethod]
        public void GenerateEmptyPaddedKeys_ReturnsOneKeyOfSpecifiedLengthAllNull()
        {
            List<List<int?>> output = PinningModelBase.GenerateEmptyPaddedKeys(6);

            if (output.Count != 1)
            { Assert.Fail($"Output contained {output.Count} records instead of one."); }

            List<int?> currentKey = output[0];

            if (currentKey.Count != 6)
            { Assert.Fail($"Output key was length {output[0].Count} instead of six."); }

            for (int index = 0; index < currentKey.Count; index++)
            {
                if (currentKey[index] != null)
                { Assert.Fail($"Cut at index {index} was {currentKey[index]} instead of null."); }
            }
        }
        #endregion
        #region GetSortedCutsAtIndex
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

            List<int?> expected = new List<int?> { 1, 3, 4, 5, 7 };

            List<int?> output = PinningModelBase.GetSortedCutsAtIndex(input, 2);

            if (output.Count != expected.Count)
            { Assert.Fail($"Output had length {output.Count} instead of {expected.Count}."); }

            for (int index = 0; index < output.Count; index++)
            {
                int? currentOutput = output[index];
                int? currentExpected = expected[index];

                if (currentOutput != currentExpected)
                { Assert.Fail($"Output value {currentOutput} at index {index} did not match expected value {currentExpected}."); }
            }
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

            List<int?> output = PinningModelBase.GetSortedCutsAtIndex(input, 2);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { PinningModelBase.GetSortedCutsAtIndex(input, -1); });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { PinningModelBase.GetSortedCutsAtIndex(input, 7); });
        }
        #endregion
        #region GetMaxColumnHeight
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

            int output = PinningModelBase.GetMaxColumnHeight(input);

            Assert.AreEqual(output, 5);
        }
        [TestMethod]
        public void GetMaxColumnHeight_EmptyInputReturnsZeroHeight()
        {
            List<List<int?>> input = new List<List<int?>>();

            int output = PinningModelBase.GetMaxColumnHeight(input);

            Assert.AreEqual(output, 0);
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

            int output = PinningModelBase.GetMaxColumnHeight(input);

            Assert.AreEqual(output, 0);
        }
        #endregion
        #region GetSortedCuts
        [TestMethod]
        public void GetSortedCuts_EmptyInputReturnsEmptyOutput()
        {
            List<List<int?>> input = new List<List<int?>>();

            List<List<int?>> output = PinningModelBase.GetSortedCuts(input);

            Assert.AreEqual(output.Count, 0);
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
        public void GetSortedCuts_ValidInputReturnsSameNumberOfColumnsAsKeyLength()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?>{ 1, 2, 3, 4, 5, 6 },
                new List<int?>{ 4, 3, 5, 1, 2, 7 },
                new List<int?>{ 1, 3, 3, null, 2, 7 },
                new List<int?>{ 5, 1, 6, 3, 1, 2 }
            };

            List<List<int?>> output = PinningModelBase.GetSortedCuts(input);

            Assert.AreEqual(output.Count, input[0].Count);
        }
        #endregion
        #region ValidateCuts
        [TestMethod]
        public void ValidateCuts_EmptyInputReturnsFalse()
        {
            List<int?> input = new List<int?>();

            bool output = PinningModelBase.ValidateCuts(input);

            Assert.AreEqual(false, output);
        }
        [TestMethod]
        public void ValidateCuts_InputContainsNullThrowsException()
        {
            List<int?> input = new List<int?> { 1, 2, 3, null, 5, 6 };

            Assert.ThrowsException<ArgumentNullException>(() => { PinningModelBase.ValidateCuts(input); });
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

            Assert.AreEqual(true, output);
        }
        #endregion
        #region GetDeepestCut
        [TestMethod]
        public void GetDeepestCut_EmptyInputReturnsNull()
        {
            List<int?> input = new List<int?>();

            int? output = PinningModelBase.GetDeepestCut(input);

            Assert.AreEqual(null, output);
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

            int? output = PinningModelBase.GetDeepestCut(input);

            Assert.AreEqual(6, output);
        }
        #endregion
        #region GetDeepestCuts
        [TestMethod]
        public void GetDeepestCuts_EmptyInputReturnsEmptyOutput()
        {
            List<List<int?>> input = new List<List<int?>>();

            List<int?> output = PinningModelBase.GetDeepestCuts(input);

            Assert.AreEqual(0, output.Count);
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

            List<int?> output = PinningModelBase.GetDeepestCuts(input);

            Assert.AreEqual(3, output.Count);

            foreach (int? currentCut in output)
            { Assert.AreEqual(null, currentCut); }
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
            {
                int? currentExpected = expected[index];
                int? currentOutput = output[index];

                Assert.AreEqual(currentExpected, currentOutput);
            }
        }
        #endregion
        #region CalculatePinColumn
        [TestMethod]
        public void CalculatePinColumn_EmptyInputReturnsEmptyOutput()
        {
            List<int?> input = new List<int?>();

            List<int?> output = PinningModelBase.CalculatePinColumn(input);

            Assert.AreEqual(input.Count, output.Count);
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
            {
                int? currentExpected = expected[index];
                int? currentOutput = output[index];

                Assert.AreEqual(currentExpected, currentOutput);
            }
        }
        #endregion
        #region PadColumn
        [TestMethod]
        public void PadColumn_EmptyInputReturnsColumnOfNulls()
        {
            List<int?> input = new List<int?>();

            List<int?> expected = new List<int?> { null, null, null, null, null };

            List<int?> output = PinningModelBase.PadColumn(input, 5);

            if (output.Count != expected.Count)
            { Assert.Fail($"Output had length {output.Count} instead of {expected.Count}."); }

            for (int index = 0; index < output.Count; index++)
            {
                int? currentOutput = output[index];
                int? currentExpected = expected[index];

                if (currentOutput != currentExpected)
                { Assert.Fail($"Output value {currentOutput} at index {index} did not match expected value of {currentExpected}."); }
            }
        }
        [TestMethod]
        public void PadColumn_PaddingHeightEqualsInputHeightReturnsUnchangedInput()
        {
            List<int?> input = new List<int?> { 1, 2, 3, 4, 5 };

            List<int?> expected = input;

            List<int?> output = PinningModelBase.PadColumn(input, 5);

            if (output.Count != expected.Count)
            { Assert.Fail($"Output had length {output.Count} instead of {expected.Count}."); }

            for (int index = 0; index < output.Count; index++)
            {
                int? currentOutput = output[index];
                int? currentExpected = expected[index];

                if (currentOutput != currentExpected)
                { Assert.Fail($"Output value {currentOutput} at index {index} did not match expected value of {currentExpected}."); }
            }
        }
        [TestMethod]
        public void PadColumn_PaddingHeightGreaterThanInputReturnsPaddedInput()
        {
            List<int?> input = new List<int?> { 1, 2, 3, 4, 5 };

            List<int?> expected = new List<int?> { 1, 2, 3, 4, 5, null, null };

            List<int?> output = PinningModelBase.PadColumn(input, 7);

            if (output.Count != expected.Count)
            { Assert.Fail($"Output had length {output.Count} instead of {expected.Count}."); }

            for (int index = 0; index < output.Count; index++)
            {
                int? currentOutput = output[index];
                int? currentExpected = expected[index];

                if (currentOutput != currentExpected)
                { Assert.Fail($"Output value {currentOutput} at index {index} did not match expected value of {currentExpected}."); }
            }
        }
        [TestMethod]
        public void PadColumn_PaddingHeightLessThanInputThrowsException()
        {
            List<int?> input = new List<int?> { 1, 2, 3, 4, 5 };

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { PinningModelBase.PadColumn(input, 2); });
        }
        #endregion
        #region PadColumns
        [TestMethod]
        public void PadColumns_EmptyInputReturnsEmptyOutput()
        {
            List<List<int?>> input = new List<List<int?>>();

            List<List<int?>> output = PinningModelBase.PadColumns(input);

            Assert.AreEqual(output.Count, 0);
        }
        [TestMethod]
        public void PadColumns_ValidInputReturnsSameNumberOfColumns()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?>{ 1, 2, 3 },
                new List<int?>{ 1, 2, 3, 4, 5 },
                new List<int?>{ 1, 2 }
            };

            List<List<int?>> output = PinningModelBase.PadColumns(input);

            if (output.Count != input.Count)
            {
                Assert.Fail($"Output had {output.Count} columns instead of expected {input.Count}.");
            }
        }
        [TestMethod]
        public void PadColumns_ValidInputReturnsColumnsOfSameHeight()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?>{ 1, 2, 3 },
                new List<int?>{ 1, 2, 3, 4, 5 },
                new List<int?>{ 1, 2 }
            };

            List<List<int?>> output = PinningModelBase.PadColumns(input);

            for (int index = 0; index < output.Count; index++)
            {
                List<int?> currentColumn = output[index];

                if (currentColumn.Count != 5)
                { Assert.Fail($"Column at index {index} was height {currentColumn.Count} instead of expected 5."); }
            }
        }
        #endregion
        #region GetOperatingPins
        [TestMethod]
        public void GetOperatingPins_ValidInputReturnsColumnsOfAllEqualHeight()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?>{ 1, 2, 3, 4, 5, 6 },
                new List<int?>{ 3, 4, 5 },
                new List<int?>{ 6, 7, 8, 9 },
                new List<int?>{ 2, 3 }
            };

            List<List<int?>> output = PinningModelBase.GetOperatingPins(input);

            int firstColumnHeight = output[0].Count;

            for (int index = 1; index < output.Count; index++)
            {
                List<int?> currentColumn = output[index];

                if (currentColumn.Count != firstColumnHeight)
                { Assert.Fail($"Column at index {index} had height {currentColumn.Count} instead of expected {firstColumnHeight}"); }
            }
        }
        [TestMethod]
        public void GetOperatingPins_ValidInputReturnsSameNumberOfColumnsAsInput()
        {
            List<List<int?>> input = new List<List<int?>>
            {
                new List<int?>{ 1, 2, 3, 4, 5, 6 },
                new List<int?>{ 3, 4, 5 },
                new List<int?>{ 6, 7, 8, 9 },
                new List<int?>{ 2, 3 }
            };

            List<List<int?>> output = PinningModelBase.GetOperatingPins(input);

            Assert.AreEqual(input.Count, output.Count);
        }
        #endregion
        #region GetRowAtIndex
        #endregion
    }
}