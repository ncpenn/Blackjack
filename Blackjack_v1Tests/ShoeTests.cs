using System;
using Blackjack.Actors;
using Blackjack.Errors;
using Blackjack.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackjackTests
{
    [TestClass]
    public class ShoeTests
    {
        [TestMethod]
        public void ShoeSetupTests()
        {
            var shoe = new Shoe(8, new Percent(50));
            Assert.IsFalse(shoe.NeedsToBeShuffled);
        }

        [TestMethod]
        public void GiveMeCardsTests()
        {
            var shoe = new Shoe(8, new Percent(50));
            Assert.AreEqual(1, shoe.GiveMeSomeCards(1).Length);
            Assert.AreEqual(10, shoe.GiveMeSomeCards(10).Length);
            Assert.AreEqual(50, shoe.GiveMeSomeCards(50).Length);
            Assert.AreEqual(2, shoe.GiveMeSomeCards(2).Length);

            shoe = new Shoe(1, new Percent(1));
            shoe.GiveMeSomeCards(1);
            Assert.IsTrue(shoe.NeedsToBeShuffled);

            shoe = new Shoe(1, new Percent(50));
            Assert.AreEqual(52, shoe.GiveMeSomeCards(52).Length);
            Assert.IsTrue(shoe.NeedsToBeShuffled);
            try
            {
                shoe.GiveMeSomeCards(1);
                Assert.Fail();
            }
            catch (TooManyCardsRequestedException ex)
            {
                Assert.IsInstanceOfType(ex, typeof (TooManyCardsRequestedException));
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ShoeTestShuffle()
        {
            var shoe = new Shoe(2, new Percent(50));
            shoe.GiveMeSomeCards(53);
            Assert.IsTrue(shoe.NeedsToBeShuffled);
            shoe.Shuffle();
            Assert.IsFalse(shoe.NeedsToBeShuffled);
            shoe.GiveMeSomeCards(100);
        }
    }
}
