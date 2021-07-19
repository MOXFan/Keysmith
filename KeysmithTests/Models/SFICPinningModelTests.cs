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
    }
}
