using System;
using Blackjack.Actors;
using Blackjack.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackjackTests
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void SetInitalCardsTests()
        {
            var player = new Player(100, true);
            player.SetInitialCards(new Card[] {new Card {Value = 1}, new Card {Value = 10} });
            Assert.IsTrue(player.IsCurrentHandBlackjack);

            player.SetInitialCards(new Card[] { new Card { Value = 1 }, new Card { Value = 1 } });
            Assert.IsFalse(player.IsCurrentHandBlackjack);

            var handTotal = player.GetMainHandTotal();
            var splitTotal = player.GeSplitHandTotal();
            Assert.IsInstanceOfType(handTotal, typeof(PlayerHand));
            Assert.AreEqual(12, handTotal.HandTotal);
            Assert.IsFalse(handTotal.IsBlackJack);
            Assert.IsNull(splitTotal);
        }
    }
}
