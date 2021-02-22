using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Keysmith.Models.Tests
{
    [TestClass()]
    public class PinningModelBaseTests
    {
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
    }
}