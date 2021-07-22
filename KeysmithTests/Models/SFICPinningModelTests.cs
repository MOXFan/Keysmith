using Keysmith.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KeysmithTests.Models
{
    [TestClass]
    public class SFICPinningModelTests
    {
        #region GetControlKeys
        [TestMethod]
        public void GetControlKeys_NullInputThrowsException()
        {
            IEnumerable<KeyModel> input = null;

            Assert.ThrowsException<NullReferenceException>(() => { SFICPinningModel.GetControlKeys(input); });
        }
        [TestMethod]
        public void GetControlKeys_EmptyInputReturnsEmptyResult()
        {
            IEnumerable<KeyModel> input = new List<KeyModel>();
            ObservableCollection<KeyModel> expectedOutput = new ObservableCollection<KeyModel>();

            ObservableCollection<KeyModel> output = SFICPinningModel.GetControlKeys(input);

            Assert.AreEqual(expectedOutput.Count, output.Count);
        }
        [TestMethod]
        public void GetControlKeys_InputWithNoControlKeysReturnsEmptyResult()
        {
            IEnumerable<KeyModel> input = new List<KeyModel>
            {
                new KeyModel{ Code = "1AA", Cuts = "12345", IsControl = false },
                new KeyModel{ Code = "2AA", Cuts = "23456", IsControl = false },
                new KeyModel{ Code = "3AA", Cuts = "34567", IsControl = false },
                new KeyModel{ Code = "4AA", Cuts = "45678", IsControl = false },
                new KeyModel{ Code = "5AA", Cuts = "56789", IsControl = false }
            };
            ObservableCollection<KeyModel> expectedOutput = new ObservableCollection<KeyModel>();

            ObservableCollection<KeyModel> output = SFICPinningModel.GetControlKeys(input);

            Assert.AreEqual(expectedOutput.Count, output.Count);
        }
        [TestMethod]
        public void GetControlKeys_InputWithControlKeysReturnsCorrectResult()
        {
            IEnumerable<KeyModel> input = new List<KeyModel>
            {
                new KeyModel{ Code = "1AA", Cuts = "12345", IsControl = false },
                new KeyModel{ Code = "2AA", Cuts = "23456", IsControl = true },
                new KeyModel{ Code = "3AA", Cuts = "34567", IsControl = false },
                new KeyModel{ Code = "4AA", Cuts = "45678", IsControl = true },
                new KeyModel{ Code = "5AA", Cuts = "56789", IsControl = false }
            };
            ObservableCollection<KeyModel> expectedOutput = new ObservableCollection<KeyModel>
            {
                new KeyModel{ Code = "2AA", Cuts = "23456", IsControl = true },
                new KeyModel{ Code = "4AA", Cuts = "45678", IsControl = true }
            };

            ObservableCollection<KeyModel> output = SFICPinningModel.GetControlKeys(input);

            Assert.AreEqual(expectedOutput.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            {
                bool resultMatches = expectedOutput[index].Equals(output[index]);
                Assert.IsTrue(resultMatches);
            }
        }
        #endregion
        #region GetOperatingKeys
        [TestMethod]
        public void GetOperatingKeys_NullInputThrowsException()
        {
            IEnumerable<KeyModel> input = null;

            Assert.ThrowsException<NullReferenceException>(() => { SFICPinningModel.GetOperatingKeys(input); });
        }
        [TestMethod]
        public void GetOperatingKeys_EmptyInputReturnsEmptyResult()
        {
            IEnumerable<KeyModel> input = new List<KeyModel>();
            List<KeyModel> expected = new List<KeyModel>();

            ObservableCollection<KeyModel> output = SFICPinningModel.GetOperatingKeys(input);

            Assert.AreEqual(expected.Count, output.Count);
        }
        [TestMethod]
        public void GetOperatingKeys_InputWithNoOperatingKeysReturnsEmptyResult()
        {
            IEnumerable<KeyModel> input = new List<KeyModel>
            {
                new KeyModel{ Code = "1AA", Cuts = "12345", IsControl = true },
                new KeyModel{ Code = "2AA", Cuts = "23456", IsControl = true },
                new KeyModel{ Code = "3AA", Cuts = "34567", IsControl = true },
                new KeyModel{ Code = "4AA", Cuts = "45678", IsControl = true },
                new KeyModel{ Code = "5AA", Cuts = "56789", IsControl = true }
            };
            ObservableCollection<KeyModel> expectedOutput = new ObservableCollection<KeyModel>();

            ObservableCollection<KeyModel> output = SFICPinningModel.GetOperatingKeys(input);

            Assert.AreEqual(expectedOutput.Count, output.Count);
        }
        [TestMethod]
        public void GetOperatingKeys_InputWithOperatingKeysReturnsCorrectResult()
        {
            IEnumerable<KeyModel> input = new List<KeyModel>
            {
                new KeyModel{ Code = "1AA", Cuts = "12345", IsControl = false },
                new KeyModel{ Code = "2AA", Cuts = "23456", IsControl = true },
                new KeyModel{ Code = "3AA", Cuts = "34567", IsControl = false },
                new KeyModel{ Code = "4AA", Cuts = "45678", IsControl = true },
                new KeyModel{ Code = "5AA", Cuts = "56789", IsControl = false }
            };
            ObservableCollection<KeyModel> expectedOutput = new ObservableCollection<KeyModel>
            {
                new KeyModel{ Code = "1AA", Cuts = "12345", IsControl = false },
                new KeyModel{ Code = "3AA", Cuts = "34567", IsControl = false },
                new KeyModel{ Code = "5AA", Cuts = "56789", IsControl = false }
            };

            ObservableCollection<KeyModel> output = SFICPinningModel.GetOperatingKeys(input);

            Assert.AreEqual(expectedOutput.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            {
                bool resultMatches = expectedOutput[index].Equals(output[index]);
                Assert.IsTrue(resultMatches);
            }
        }
        #endregion
        #region GetControlPins
        [TestMethod]
        public void GetControlPins_EitherInputNullThrowsException()
        {
            List<List<int?>> inputControlCuts = new List<List<int?>>
            {
                new List<int?> { 2, 5, 7, 3, 4, 6, 1 },
                new List<int?> { 4, 7, 3, 5, 6, 2, 4 }
            };
            List<int?> inputDeepestOperatingCuts = new List<int?> { 6, 3, 9, 8, 2, 7, 9 };

            Assert.ThrowsException<NullReferenceException>(() => { SFICPinningModel.GetControlPins(inputControlCuts, null); });
            Assert.ThrowsException<NullReferenceException>(() => { SFICPinningModel.GetControlPins(null, inputDeepestOperatingCuts); });
        }
        [TestMethod]
        public void GetControlPins_EmptyOperatingCutsReturnsEmptyResult()
        {
            List<List<int?>> inputControlCuts = new List<List<int?>>
            {
                new List<int?> { 2, 5, 7, 3, 4, 6, 1 },
                new List<int?> { 4, 7, 3, 5, 6, 2, 4 }
            };
            List<int?> inputDeepestOperatingCuts = new List<int?>();
            List<List<int?>> expected = new List<List<int?>>();

            List<List<int?>> output = SFICPinningModel.GetControlPins(inputControlCuts, inputDeepestOperatingCuts);

            Assert.AreEqual(expected.Count, output.Count);
        }
        [TestMethod]
        public void GetControlPins_EmptyControlCutsReturnsEmptyResult()
        {
            List<List<int?>> inputControlCuts = new List<List<int?>>();
            List<int?> inputDeepestOperatingCuts = new List<int?> { 6, 3, 9, 8, 2, 7, 9 };
            List<List<int?>> expected = new List<List<int?>>();

            List<List<int?>> output = SFICPinningModel.GetControlPins(inputControlCuts, inputDeepestOperatingCuts);

            Assert.AreEqual(expected.Count, output.Count);
        }
        [TestMethod]
        public void GetControlPins_InvalidInputThrowsException()
        {
            List<List<int?>> inputControlCuts = new List<List<int?>>
            {
                new List<int?> { 2, 4 },
                new List<int?> { 5, 7 },
                new List<int?> { 7, 3 },
                new List<int?> { 3, 5 },
                new List<int?> { 4, 6 },
                new List<int?> { 6, 2 },
                new List<int?> { 1, 4 }
            };
            List<int?> inputDeepestOperatingCuts = new List<int?> { 6, 3, 9, 8, 2, 7, 9 };

            Assert.ThrowsException<ArgumentException>(() => { SFICPinningModel.GetControlPins(inputControlCuts, inputDeepestOperatingCuts); });
        }
        [TestMethod]
        public void GetControlPins_ValidInputReturnsCorrectPaddedOutput()
        {
            List<List<int?>> inputControlCuts = new List<List<int?>>
            {
                new List<int?> { 2, 4 },
                new List<int?> { 5, 7 },
                new List<int?> { 3 },
                new List<int?> { 3, 5 },
                new List<int?> { 4, 6 },
                new List<int?> { 2, 6 },
                new List<int?> { 1, 4, 6 }
            };
            List<int?> inputDeepestOperatingCuts = new List<int?> { 6, 3, 9, 8, 2, 7, 9 };
            List<List<int?>> expected = new List<List<int?>>
            {
                new List<int?>{ 6, 2, null },
                new List<int?>{ 12, 2, null },
                new List<int?>{ 4, null, null },
                new List<int?>{ 5, 2, null },
                new List<int?>{ 12, 2, null },
                new List<int?>{ 5, 4, null },
                new List<int?>{ 2, 3, 2 }
            };

            List<List<int?>> output = SFICPinningModel.GetControlPins(inputControlCuts, inputDeepestOperatingCuts);

            Assert.AreEqual(expected.Count, output.Count);

            for (int rowIndex = 0; rowIndex < output.Count; rowIndex++)
            {
                List<int?> currentOutputRow = output[rowIndex];
                List<int?> currentExpectedRow = expected[rowIndex];

                Assert.AreEqual(currentExpectedRow.Count, currentOutputRow.Count);

                for (int columnIndex = 0; columnIndex < currentOutputRow.Count; columnIndex++)
                { Assert.AreEqual(currentExpectedRow[columnIndex], currentOutputRow[columnIndex]); }
            }
        }
        [TestMethod]
        public void GetControlPins_ValidInputWithLongerOperatingCutsReturnsCorrectPaddedOutput()
        {
            List<List<int?>> inputControlCuts = new List<List<int?>>
            {
                new List<int?> { 2, 4 },
                new List<int?> { 5, 7 },
                new List<int?> { 3 },
                new List<int?> { 3, 5 },
                new List<int?> { 1, 4, 6 }
            };
            List<int?> inputDeepestOperatingCuts = new List<int?> { 6, 3, 9, 8, 2, 7, 9 };
            List<List<int?>> expected = new List<List<int?>>
            {
                new List<int?>{ 6, 2, null },
                new List<int?>{ 12, 2, null },
                new List<int?>{ 4, null, null },
                new List<int?>{ 5, 2, null },
                new List<int?>{ 9, 3, 2 },
                new List<int?>{ null, null, null },
                new List<int?>{ null, null, null}
            };

            List<List<int?>> output = SFICPinningModel.GetControlPins(inputControlCuts, inputDeepestOperatingCuts);

            Assert.AreEqual(expected.Count, output.Count);

            for (int rowIndex = 0; rowIndex < output.Count; rowIndex++)
            {
                List<int?> currentOutputRow = output[rowIndex];
                List<int?> currentExpectedRow = expected[rowIndex];

                Assert.AreEqual(currentExpectedRow.Count, currentOutputRow.Count);

                for (int columnIndex = 0; columnIndex < currentOutputRow.Count; columnIndex++)
                { Assert.AreEqual(currentExpectedRow[columnIndex], currentOutputRow[columnIndex]); }
            }
        }
        [TestMethod]
        public void GetControlPins_ValidInputWithLongerControlCutsReturnsCorrectPaddedOutput()
        {
            List<List<int?>> inputControlCuts = new List<List<int?>>
            {
                new List<int?> { 2, 4 },
                new List<int?> { 5, 7 },
                new List<int?> { 3 },
                new List<int?> { 3, 5 },
                new List<int?> { 4, 6 },
                new List<int?> { 2, 6 },
                new List<int?> { 1, 4, 6 }
            };
            List<int?> inputDeepestOperatingCuts = new List<int?> { 6, 3, 9, 8, 2 };
            List<List<int?>> expected = new List<List<int?>>
            {
                new List<int?>{ 6, 2, null },
                new List<int?>{ 12, 2, null },
                new List<int?>{ 4, null, null },
                new List<int?>{ 5, 2, null },
                new List<int?>{ 12, 2, null },
                new List<int?>{ null, null, null },
                new List<int?>{ null, null, null }
            };

            List<List<int?>> output = SFICPinningModel.GetControlPins(inputControlCuts, inputDeepestOperatingCuts);

            Assert.AreEqual(expected.Count, output.Count);

            for (int rowIndex = 0; rowIndex < output.Count; rowIndex++)
            {
                List<int?> currentOutputRow = output[rowIndex];
                List<int?> currentExpectedRow = expected[rowIndex];

                Assert.AreEqual(currentExpectedRow.Count, currentOutputRow.Count);

                for (int columnIndex = 0; columnIndex < currentOutputRow.Count; columnIndex++)
                { Assert.AreEqual(currentExpectedRow[columnIndex], currentOutputRow[columnIndex]); }
            }
        }
        #endregion
        #region GetDriverPins
        [TestMethod]
        public void GetDriverPins_NullInputThrowsException()
        {
            List<int?> inputDeepestControlCuts = new List<int?> { 2, 3, 8, 6, 5, 2, 4 };
            List<int?> inputDeepestOperatingCuts = new List<int?> { 4, 6, 2, 4, 9, 6, 8 };

            Assert.ThrowsException<NullReferenceException>(() => { SFICPinningModel.GetDriverPins(inputDeepestControlCuts, null); });
            Assert.ThrowsException<NullReferenceException>(() => { SFICPinningModel.GetDriverPins(null, inputDeepestOperatingCuts); });
        }
        [TestMethod]
        public void GetDriverPins_OperatingCutsContainsNullsReturnsOutputWithCorrespondingDriverPinsNull()
        {
            List<int?> inputDeepestControlCuts = new List<int?> { 2, null, 8, 6, null, 2, 4 };
            List<int?> inputDeepestOperatingCuts = new List<int?> { 4, 6, 2, 4, 9, 6, 8 };
            string inputEmptyCellSpacer = "X";
            ObservableCollection<string> expected = new ObservableCollection<string> { "11", "X", "5", "7", "X", "11", "9" };

            ObservableCollection<string> output = SFICPinningModel.GetDriverPins(inputDeepestControlCuts, inputDeepestOperatingCuts, inputEmptyCellSpacer);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        [TestMethod]
        public void GetDriverPins_ControlCutsContainsNullsReturnsOutputWithCorrespondingDriverPinsAsSpacer()
        {
            List<int?> inputDeepestControlCuts = new List<int?> { 2, 3, 8, 6, 5, 2, 4 };
            List<int?> inputDeepestOperatingCuts = new List<int?> { 4, null, 2, 4, null, 6, 8 };
            string inputEmptyCellSpacer = "X";
            ObservableCollection<string> expected = new ObservableCollection<string> { "11", "X", "5", "7", "X", "11", "9" };

            ObservableCollection<string> output = SFICPinningModel.GetDriverPins(inputDeepestControlCuts, inputDeepestOperatingCuts, inputEmptyCellSpacer);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        [TestMethod]
        public void GetDriverPins_ControlLongerThanOperatingReturnsOutputTruncatedToOperatingLengthAndPaddedToControlLengthWithSpacers()
        {
            List<int?> inputDeepestControlCuts = new List<int?> { 2, 3, 8, 6, 5, 2, 4 };
            List<int?> inputDeepestOperatingCuts = new List<int?> { 4, 6, 2, 4, 9 };
            string inputEmptyCellSpacer = "X";
            ObservableCollection<string> expected = new ObservableCollection<string> { "11", "10", "5", "7", "8", "X", "X" };

            ObservableCollection<string> output = SFICPinningModel.GetDriverPins(inputDeepestControlCuts, inputDeepestOperatingCuts, inputEmptyCellSpacer);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        [TestMethod]
        public void GetDriverPins_OperatingLongerThanControlReturnsOutputTruncatedToControlLengthAndPaddedToOperatingLengthWithSpacers()
        {
            List<int?> inputDeepestControlCuts = new List<int?> { 2, 3, 8, 6 };
            List<int?> inputDeepestOperatingCuts = new List<int?> { 4, 6, 2, 4, 9, 6, 8 };
            string inputEmptyCellSpacer = "X";
            ObservableCollection<string> expected = new ObservableCollection<string> { "11", "10", "5", "7", "X", "X", "X" };

            ObservableCollection<string> output = SFICPinningModel.GetDriverPins(inputDeepestControlCuts, inputDeepestOperatingCuts, inputEmptyCellSpacer);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        [TestMethod]
        public void GetDriverPins_ValidInputReturnsCorrectResult()
        {
            List<int?> inputDeepestControlCuts = new List<int?> { 2, 3, 8, 6, 5, 2, 4 };
            List<int?> inputDeepestOperatingCuts = new List<int?> { 4, 6, 2, 4, 9, 6, 8 };
            ObservableCollection<string> expected = new ObservableCollection<string> { "11", "10", "5", "7", "8", "11", "9" };

            ObservableCollection<string> output = SFICPinningModel.GetDriverPins(inputDeepestControlCuts, inputDeepestOperatingCuts);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        #endregion
        #region GenerateSFICRowHeaders
        [TestMethod]
        public void GenerateSFICRowHeaders_NegativeOperatingRowCountThrowsException()
        {
            int operatingRows = -1;
            int controlRows = 2;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { SFICPinningModel.GenerateSFICRowHeaders(operatingRows, controlRows); });
        }
        [TestMethod]
        public void GenerateSFICRowHeaders_NegativeControlRowCountThrowsException()
        {
            int operatingRows = 4;
            int controlRows = -1;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { SFICPinningModel.GenerateSFICRowHeaders(operatingRows, controlRows); });
        }
        [TestMethod]
        public void GenerateSFICRowHeaders_MultipleControlAndZeroOperatingRowInputReturnsEmptyResult()
        {
            int operatingRows = 0;
            int controlRows = 2;
            string inputBottomPinHeader = "B";
            string inputMasterPinHeader = "M";
            string inputControlPinHeader = "C";
            string inputDriverPinHeader = "D";

            ObservableCollection<string> expected = new ObservableCollection<string>();

            ObservableCollection<string> output = SFICPinningModel.GenerateSFICRowHeaders(operatingRows, controlRows, inputBottomPinHeader,
                inputMasterPinHeader, inputControlPinHeader, inputDriverPinHeader);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        [TestMethod]
        public void GenerateSFICRowHeaders_MultipleOperatingAndZeroControlRowInputReturnsCorrectResults()
        {
            int operatingRows = 4;
            int controlRows = 0;
            string inputBottomPinHeader = "B";
            string inputMasterPinHeader = "M";
            string inputControlPinHeader = "C";
            string inputDriverPinHeader = "D";

            ObservableCollection<string> expected = new ObservableCollection<string> { "B", "M", "M", "M", "D" };

            ObservableCollection<string> output = SFICPinningModel.GenerateSFICRowHeaders(operatingRows, controlRows, inputBottomPinHeader,
                inputMasterPinHeader, inputControlPinHeader, inputDriverPinHeader);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        [TestMethod]
        public void GenerateSFICRowHeaders_MultipleControlAndOperatingRowInputReturnsCorrectResults()
        {
            int operatingRows = 4;
            int controlRows = 2;
            string inputBottomPinHeader = "B";
            string inputMasterPinHeader = "M";
            string inputControlPinHeader = "C";
            string inputDriverPinHeader = "D";

            ObservableCollection<string> expected = new ObservableCollection<string> { "B", "M", "M", "M", "C", "C", "D" };

            ObservableCollection<string> output = SFICPinningModel.GenerateSFICRowHeaders(operatingRows, controlRows, inputBottomPinHeader,
                inputMasterPinHeader, inputControlPinHeader, inputDriverPinHeader);

            Assert.AreEqual(expected.Count, output.Count);

            for (int index = 0; index < output.Count; index++)
            { Assert.AreEqual(expected[index], output[index]); }
        }
        #endregion
        #region GenerateSFICRows
        [TestMethod]
        public void GenerateSFICRows_NullControlInputThrowsException()
        {
            List<List<int?>> inputOperatingPins = new List<List<int?>>();
            List<List<int?>> inputControlPins = new List<List<int?>>();
            ObservableCollection<String> inputDriverPins = new ObservableCollection<string>();
            string inputEmptyCellSpacer = "X";

            Assert.ThrowsException<NullReferenceException>(() => { SFICPinningModel.GenerateSFICRows(null, inputControlPins, inputDriverPins, inputEmptyCellSpacer); });
        }
        [TestMethod]
        public void GenerateSFICRows_NullOperatingInputThrowsException()
        {
            List<List<int?>> inputOperatingPins = new List<List<int?>>();
            List<List<int?>> inputControlPins = new List<List<int?>>();
            ObservableCollection<String> inputDriverPins = new ObservableCollection<string>();
            string inputEmptyCellSpacer = "X";

            Assert.ThrowsException<NullReferenceException>(() => { SFICPinningModel.GenerateSFICRows(inputOperatingPins, null, inputDriverPins, inputEmptyCellSpacer); });
        }
        [TestMethod]
        public void GenerateSFICRows_NullDriverInputThrowsException()
        {
            List<List<int?>> inputOperatingPins = new List<List<int?>>();
            List<List<int?>> inputControlPins = new List<List<int?>>();
            ObservableCollection<String> inputDriverPins = new ObservableCollection<string>();
            string inputEmptyCellSpacer = "X";

            Assert.ThrowsException<NullReferenceException>(() => { SFICPinningModel.GenerateSFICRows(inputOperatingPins, inputControlPins, null, inputEmptyCellSpacer); });
        }
        [TestMethod]
        public void GenerateSFICRows_EmptyOperatingInputReturnsEmptyResult()
        {
            List<List<int?>> inputOperatingPins = new List<List<int?>>();
            List<List<int?>> inputControlPins = new List<List<int?>>();
            ObservableCollection<String> inputDriverPins = new ObservableCollection<string>();
            string inputEmptyCellSpacer = "X";
            ObservableCollection<ObservableCollection<string>> expected = new ObservableCollection<ObservableCollection<string>>();

            ObservableCollection<ObservableCollection<string>> output = SFICPinningModel.GenerateSFICRows(inputOperatingPins, 
                inputControlPins, inputDriverPins, inputEmptyCellSpacer);

            Assert.AreEqual(expected.Count, output.Count);
        }
        [TestMethod]
        public void GenerateSFICRows_InputWithEmptyDriverPinsReturnsResulWithNoDriverRow()
        {
            List<List<int?>> inputOperatingPins = new List<List<int?>>
            {
                new List<int?> { 2, 4 },
                new List<int?> { 3, null },
                new List<int?> { 7, 2 },
                new List<int?> { 4, 4 },
                new List<int?> { 2, null },
                new List<int?> { 3, 4 },
                new List<int?> { 3, 6 }
            };
            List<List<int?>> inputControlPins = new List<List<int?>>
            {
                new List<int?> { 6, 2, null },
                new List<int?> { 12, 2, null },
                new List<int?> { 4, null, null },
                new List<int?> { 5, 2, null },
                new List<int?> { 12, 2, null },
                new List<int?> { 5, 4, null },
                new List<int?> { 2, 3, 2 }
            };
            ObservableCollection<String> inputDriverPins = new ObservableCollection<string>();
            string inputEmptyCellSpacer = "X";
            ObservableCollection<ObservableCollection<string>> expected = new ObservableCollection<ObservableCollection<string>>
            {
                new ObservableCollection<string> { "2", "3", "7", "4", "2", "3", "3" },
                new ObservableCollection<string> { "4", "X", "2", "4", "X", "4", "6" },
                new ObservableCollection<string> { "6", "12", "4", "5", "12", "5", "2" },
                new ObservableCollection<string> { "2", "2", "X", "2", "2", "4", "3" },
                new ObservableCollection<string> { "X", "X", "X", "X", "X", "X", "2" },
            };

            ObservableCollection<ObservableCollection<string>> output = SFICPinningModel.GenerateSFICRows(inputOperatingPins,
                inputControlPins, inputDriverPins, inputEmptyCellSpacer);

            Assert.AreEqual(expected.Count, output.Count);

            for (int rowIndex = 0; rowIndex < output.Count; rowIndex++)
            {
                ObservableCollection<string> currentExpectedRow = expected[rowIndex];
                ObservableCollection<string> currentOutputRow = output[rowIndex];

                Assert.AreEqual(currentExpectedRow.Count, currentOutputRow.Count);

                for (int columnIndex = 0; columnIndex < currentOutputRow.Count; columnIndex++)
                { Assert.AreEqual(currentExpectedRow[columnIndex], currentOutputRow[columnIndex]); }
            }
        }
        [TestMethod]
        public void GenerateSFICRows_InputWithEmptyOperatingPinsReturnsEmptyResult()
        {
            List<List<int?>> inputOperatingPins = new List<List<int?>>();
            List<List<int?>> inputControlPins = new List<List<int?>>
            {
                new List<int?> { 6, 2, null },
                new List<int?> { 12, 2, null },
                new List<int?> { 4, null, null },
                new List<int?> { 5, 2, null },
                new List<int?> { 12, 2, null },
                new List<int?> { 5, 4, null },
                new List<int?> { 2, 3, 2 }
            };
            ObservableCollection<String> inputDriverPins = new ObservableCollection<string> { "9", "6", "10", "8", "7", "7", "7" };
            string inputEmptyCellSpacer = "X";
            ObservableCollection<ObservableCollection<string>> expected = new ObservableCollection<ObservableCollection<string>>();

            ObservableCollection<ObservableCollection<string>> output = SFICPinningModel.GenerateSFICRows(inputOperatingPins,
                inputControlPins, inputDriverPins, inputEmptyCellSpacer);

            Assert.AreEqual(expected.Count, output.Count);

            for (int rowIndex = 0; rowIndex < output.Count; rowIndex++)
            {
                ObservableCollection<string> currentExpectedRow = expected[rowIndex];
                ObservableCollection<string> currentOutputRow = output[rowIndex];

                Assert.AreEqual(currentExpectedRow.Count, currentOutputRow.Count);

                for (int columnIndex = 0; columnIndex < currentOutputRow.Count; columnIndex++)
                { Assert.AreEqual(currentExpectedRow[columnIndex], currentOutputRow[columnIndex]); }
            }
        }
        [TestMethod]
        public void GenerateSFICRows_InputWithEmptyControlPinsReturnsResultWithNoControlRows()
        {
            List<List<int?>> inputOperatingPins = new List<List<int?>>
            {
                new List<int?> { 2, 4 },
                new List<int?> { 3, null },
                new List<int?> { 7, 2 },
                new List<int?> { 4, 4 },
                new List<int?> { 2, null },
                new List<int?> { 3, 4 },
                new List<int?> { 3, 6 }
            };
            List<List<int?>> inputControlPins = new List<List<int?>>();
            ObservableCollection<String> inputDriverPins = new ObservableCollection<string> { "9", "6", "10", "8", "7", "7", "7" };
            string inputEmptyCellSpacer = "X";
            ObservableCollection<ObservableCollection<string>> expected = new ObservableCollection<ObservableCollection<string>>
            {
                new ObservableCollection<string> { "2", "3", "7", "4", "2", "3", "3" },
                new ObservableCollection<string> { "4", "X", "2", "4", "X", "4", "6" },
                inputDriverPins
            };

            ObservableCollection<ObservableCollection<string>> output = SFICPinningModel.GenerateSFICRows(inputOperatingPins,
                inputControlPins, inputDriverPins, inputEmptyCellSpacer);

            Assert.AreEqual(expected.Count, output.Count);

            for (int rowIndex = 0; rowIndex < output.Count; rowIndex++)
            {
                ObservableCollection<string> currentExpectedRow = expected[rowIndex];
                ObservableCollection<string> currentOutputRow = output[rowIndex];

                Assert.AreEqual(currentExpectedRow.Count, currentOutputRow.Count);

                for (int columnIndex = 0; columnIndex < currentOutputRow.Count; columnIndex++)
                { Assert.AreEqual(currentExpectedRow[columnIndex], currentOutputRow[columnIndex]); }
            }
        }
        [TestMethod]
        public void GenerateSFICRows_ValidInputReturnsCorrectResult()
        {
            List<List<int?>> inputOperatingPins = new List<List<int?>>
            {
                new List<int?> { 2, 4 },
                new List<int?> { 3, null },
                new List<int?> { 7, 2 },
                new List<int?> { 4, 4 },
                new List<int?> { 2, null },
                new List<int?> { 3, 4 },
                new List<int?> { 3, 6 }
            };
            List<List<int?>> inputControlPins = new List<List<int?>>
            {
                new List<int?> { 6, 2, null },
                new List<int?> { 12, 2, null },
                new List<int?> { 4, null, null },
                new List<int?> { 5, 2, null },
                new List<int?> { 12, 2, null },
                new List<int?> { 5, 4, null },
                new List<int?> { 2, 3, 2 }
            };                 
            ObservableCollection<String> inputDriverPins = new ObservableCollection<string> { "9", "6", "10", "8", "7", "7", "7" };
            string inputEmptyCellSpacer = "X";
            ObservableCollection<ObservableCollection<string>> expected = new ObservableCollection<ObservableCollection<string>>
            {
                new ObservableCollection<string> { "2", "3", "7", "4", "2", "3", "3" },
                new ObservableCollection<string> { "4", "X", "2", "4", "X", "4", "6" },
                new ObservableCollection<string> { "6", "12", "4", "5", "12", "5", "2" },
                new ObservableCollection<string> { "2", "2", "X", "2", "2", "4", "3" },
                new ObservableCollection<string> { "X", "X", "X", "X", "X", "X", "2" },
                inputDriverPins
            };

            ObservableCollection<ObservableCollection<string>> output = SFICPinningModel.GenerateSFICRows(inputOperatingPins,
                inputControlPins, inputDriverPins, inputEmptyCellSpacer);

            Assert.AreEqual(expected.Count, output.Count);

            for (int rowIndex = 0; rowIndex < output.Count; rowIndex++)
            {
                ObservableCollection<string> currentExpectedRow = expected[rowIndex];
                ObservableCollection<string> currentOutputRow = output[rowIndex];

                Assert.AreEqual(currentExpectedRow.Count, currentOutputRow.Count);

                for (int columnIndex = 0; columnIndex < currentOutputRow.Count; columnIndex++)
                { Assert.AreEqual(currentExpectedRow[columnIndex], currentOutputRow[columnIndex]); }
            }
        }
        #endregion
    }
}
