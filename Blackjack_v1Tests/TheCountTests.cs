using System;
using Blackjack.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackjackTests
{
    [TestClass]
    public class TheCountTests
    {
        [TestMethod]
        public void EventTest()
        {
            TheCount.CurrentCount = 0;

            Table.VisibleCards.Add(5);
            Assert.AreEqual(1, TheCount.CurrentCount);
            Table.VisibleCards.Add(1);
            Table.VisibleCards.Add(1);
            Assert.AreEqual(-1, TheCount.CurrentCount);
            Table.VisibleCards.Add(2);
            Assert.AreEqual(-1, TheCount.CurrentCount);
        }
    }
}
