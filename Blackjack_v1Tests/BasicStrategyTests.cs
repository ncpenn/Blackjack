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
            bool isSplit;
            bool isSoft;

            var listOfCardValues = new List<int>
            {
                1,
                1,
                1,
            };

            var handvalue = BasicStrategy.DetermineHandValue(listOfCardValues);
            Assert.AreEqual(13, handvalue);

            var handValue = BasicStrategy.DetermineHandValue(listOfCardValues, out isSplit, out isSoft);
            Assert.AreEqual(13, handvalue);
            Assert.IsTrue(!isSplit && isSoft);

            listOfCardValues = new List<int>
            {
                13,
                2,
                1,
            };

            handvalue = BasicStrategy.DetermineHandValue(listOfCardValues);
            Assert.AreEqual(13, handvalue);

            handValue = BasicStrategy.DetermineHandValue(listOfCardValues, out isSplit, out isSoft);
            Assert.AreEqual(13, handvalue);
            Assert.IsTrue(!isSplit && !isSoft);

            listOfCardValues = new List<int>
            {
                12,
                9,
            };

            handvalue = BasicStrategy.DetermineHandValue(listOfCardValues);
            Assert.AreEqual(19, handvalue);

            handValue = BasicStrategy.DetermineHandValue(listOfCardValues, out isSplit, out isSoft);
            Assert.AreEqual(19, handvalue);
            Assert.IsTrue(!isSplit && !isSoft);

            listOfCardValues = new List<int>
            {
                1,
                1,
            };

            handvalue = BasicStrategy.DetermineHandValue(listOfCardValues);
            Assert.AreEqual(12, handvalue);

            handValue = BasicStrategy.DetermineHandValue(listOfCardValues, out isSplit, out isSoft);
            Assert.AreEqual(12, handvalue);
            Assert.IsTrue(isSplit && !isSoft);

            listOfCardValues = new List<int>
            {
                3,
                3,
            };

            handvalue = BasicStrategy.DetermineHandValue(listOfCardValues);
            Assert.AreEqual(6, handvalue);

            handValue = BasicStrategy.DetermineHandValue(listOfCardValues, out isSplit, out isSoft);
            Assert.AreEqual(6, handvalue);
            Assert.IsTrue(isSplit && !isSoft);
        }

        [TestMethod]
        public void DeterminePlayerNextPlay()
        {
            var dealerCard = 1;
            var listOfPlayerCardValues = new List<int>
            {
                13,
                13
            };
            var nextPlayerAction = BasicStrategy.DeterminePlayerNextPlay(listOfPlayerCardValues, dealerCard);
            Assert.AreEqual(Enums.PlayAction.Stand, nextPlayerAction);

            dealerCard = 7;
            listOfPlayerCardValues = new List<int>
            {
                9,
                9
            };
            nextPlayerAction = BasicStrategy.DeterminePlayerNextPlay(listOfPlayerCardValues, dealerCard);
            Assert.AreEqual(Enums.PlayAction.Stand, nextPlayerAction);

            dealerCard = 8;
            listOfPlayerCardValues = new List<int>
            {
                9,
                9
            };
            nextPlayerAction = BasicStrategy.DeterminePlayerNextPlay(listOfPlayerCardValues, dealerCard);
            Assert.AreEqual(Enums.PlayAction.Split, nextPlayerAction);

            dealerCard = 1;
            listOfPlayerCardValues = new List<int>
            {
                2,
                2
            };
            nextPlayerAction = BasicStrategy.DeterminePlayerNextPlay(listOfPlayerCardValues, dealerCard);
            Assert.AreEqual(Enums.PlayAction.Hit, nextPlayerAction);

            dealerCard = 2;
            listOfPlayerCardValues = new List<int>
            {
                6,
                6
            };
            nextPlayerAction = BasicStrategy.DeterminePlayerNextPlay(listOfPlayerCardValues, dealerCard);
            Assert.AreEqual(Enums.PlayAction.Hit, nextPlayerAction);


            dealerCard = 5;
            listOfPlayerCardValues = new List<int>
            {
                13,
                3
            };
            nextPlayerAction = BasicStrategy.DeterminePlayerNextPlay(listOfPlayerCardValues, dealerCard);
            Assert.AreEqual(Enums.PlayAction.Stand, nextPlayerAction);


            dealerCard = 6;
            listOfPlayerCardValues = new List<int>
            {
                1,
                7
            };
            nextPlayerAction = BasicStrategy.DeterminePlayerNextPlay(listOfPlayerCardValues, dealerCard);
            Assert.AreEqual(Enums.PlayAction.Double, nextPlayerAction);

            dealerCard = 11;
            listOfPlayerCardValues = new List<int>
            {
                5,
                6
            };
            nextPlayerAction = BasicStrategy.DeterminePlayerNextPlay(listOfPlayerCardValues, dealerCard);
            Assert.AreEqual(Enums.PlayAction.Double, nextPlayerAction);

            dealerCard = 5;
            listOfPlayerCardValues = new List<int>
            {
                4,
                4
            };
            nextPlayerAction = BasicStrategy.DeterminePlayerNextPlay(listOfPlayerCardValues, dealerCard);
            Assert.AreEqual(Enums.PlayAction.Hit, nextPlayerAction);

            dealerCard = 2;
            listOfPlayerCardValues = new List<int>
            {
                2,
                2
            };
            nextPlayerAction = BasicStrategy.DeterminePlayerNextPlay(listOfPlayerCardValues, dealerCard);
            Assert.AreEqual(Enums.PlayAction.Hit, nextPlayerAction);

            dealerCard = 1;
            listOfPlayerCardValues = new List<int>
            {
                6,
                1
            };
            nextPlayerAction = BasicStrategy.DeterminePlayerNextPlay(listOfPlayerCardValues, dealerCard);
            Assert.AreEqual(Enums.PlayAction.Hit, nextPlayerAction);

            dealerCard = 13;
            listOfPlayerCardValues = new List<int>
            {
                5,
                5
            };
            nextPlayerAction = BasicStrategy.DeterminePlayerNextPlay(listOfPlayerCardValues, dealerCard);
            Assert.AreEqual(Enums.PlayAction.Hit, nextPlayerAction);
        }
    }
}