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

            Table.VisibleCards.Add(new Card {Value = 5});
            Assert.AreEqual(1, TheCount.CurrentCount);
            Table.VisibleCards.Add(new Card { Value = 1 });
            Table.VisibleCards.Add(new Card { Value = 1 });
            Assert.AreEqual(-1, TheCount.CurrentCount);
            Table.VisibleCards.Add(new Card { Value = 2 });
            Assert.AreEqual(-1, TheCount.CurrentCount);
        }
    }
}
