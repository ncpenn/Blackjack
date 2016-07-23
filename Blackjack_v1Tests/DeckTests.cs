using System.Linq;
using Blackjack.Actors;
using Blackjack.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackjackTests
{
    [TestClass]
    public class DeckTest
    {
        [TestMethod]
        public void TestReadyDeck()
        {
            var deck = new Deck();
            Assert.AreEqual(52, deck.ReadyDeck.Count);
            Assert.AreEqual(13, deck.ReadyDeck.GroupBy(c => c).Count());
        }
    }
}
