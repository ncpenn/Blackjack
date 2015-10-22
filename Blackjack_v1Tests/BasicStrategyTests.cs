using Microsoft.VisualStudio.TestTools.UnitTesting;
using Blackjack_v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack_v1.Tests
{
    [TestClass()]
    public class BasicStrategyTests
    {
        [TestMethod()]
        public void DetermineHandValueTest()
        {
            var listOfCardValues = new List<int>
            {
                1,
                1,
                1,
            };

            var handvalue = BasicStrategy.DetermineHandValue(listOfCardValues);
            Assert.AreEqual(13, handvalue);

            listOfCardValues = new List<int>
            {
                13,
                2,
                1,
            };

            handvalue = BasicStrategy.DetermineHandValue(listOfCardValues);
            Assert.AreEqual(13, handvalue);

            listOfCardValues = new List<int>
            {
                12,
                9,
            };

            handvalue = BasicStrategy.DetermineHandValue(listOfCardValues);
            Assert.AreEqual(19, handvalue);
        }
    }
}