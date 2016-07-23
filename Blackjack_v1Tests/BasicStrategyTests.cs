using System.Collections.Generic;
using System.Linq;
using Blackjack;
using Blackjack.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackjackTests
{
    [TestClass]
    public class BasicStrategyTests
    {
        [TestMethod]
        public void DetermineHandValueTest_2to10Hard()
        {
            //hard hand values from 2 pip to 10 pip
           var listOfCardValues = new uint[] { 2, 2 };
            while (listOfCardValues[0] != 10)
            {
                while (listOfCardValues[1] != 10)
                {
                    var handvalue = BasicStrategy.DetermineHandValue(listOfCardValues);
                    Assert.AreEqual(listOfCardValues.Aggregate((a, b) => a + b), handvalue.Value);
                    Assert.IsFalse(handvalue.IsSoft);
                    listOfCardValues[1]++;
                }
                listOfCardValues[0]++;
            }
        }

        [TestMethod]
        public void DetermineHandValueTest_SoftHands()
        {
            var listOfCardValues = new uint[] { 1, 1 };
            while (listOfCardValues[0] != 13)
            {
                var handvalue = BasicStrategy.DetermineHandValue(listOfCardValues);
                var expectedHandValue = listOfCardValues[0] < 10 ? 11 + listOfCardValues[0] : 21;
                Assert.AreEqual(expectedHandValue, handvalue.Value);
                if (listOfCardValues[0] == 1 && listOfCardValues[1] == 1)
                    Assert.IsFalse(handvalue.IsSoft);
                else
                    Assert.IsTrue(handvalue.IsSoft);
                listOfCardValues[0]++;
            }
        }

        [TestMethod]
        public void DetermineHandValueTest_10ValueHands()
        {
            var listOfCardValues = new uint[] { 10, 1 };
            while (listOfCardValues[0] != 13)
            {
                while (listOfCardValues[1] != 13)
                {
                    var handvalue = BasicStrategy.DetermineHandValue(listOfCardValues);
                    uint expectedHandValue;
                    if (listOfCardValues.Any(c => c == 1))
                        expectedHandValue = 21;
                    else if (listOfCardValues[1] > 10)
                        expectedHandValue = 20;
                    else
                        expectedHandValue = 10 + listOfCardValues[1];
                    Assert.AreEqual(expectedHandValue, handvalue.Value);
                    listOfCardValues[1]++;
                }
                listOfCardValues[0]++;
            }
        }

        [TestMethod]
        public void DetermineHandValueTests_Splits()
        {
            var listOfCardValues = new uint[]{ 1, 1 };
            while (listOfCardValues[0] != 13)
            {
                var handvalue = BasicStrategy.DetermineHandValue(listOfCardValues);
                if (listOfCardValues.Any(c => c == 1))
                {
                    Assert.AreEqual((uint)12, handvalue.Value);
                    Assert.IsFalse(handvalue.IsSoft);
                    Assert.IsTrue(handvalue.IsSplit);
                }
                else if (listOfCardValues[1] > 10)
                {
                    Assert.AreEqual((uint)20, handvalue.Value);
                    Assert.IsTrue(handvalue.IsSplit);
                    Assert.IsFalse(handvalue.IsSoft);
                }
                else
                {
                    Assert.AreEqual(listOfCardValues[0] * 2, handvalue.Value);
                    Assert.IsTrue(handvalue.IsSplit);
                    Assert.IsFalse(handvalue.IsSoft);
                }
                listOfCardValues[1]++;
                listOfCardValues[0]++;
            }
        }

        [TestMethod]
        public void DetermineHandValueTests_RandomTests()
        {
            var listOfCardValues = new uint[]
            {
                1,
                1,
                1,
            };

            var handValue = BasicStrategy.DetermineHandValue(listOfCardValues);
            Assert.AreEqual((uint)13, handValue.Value);
            Assert.IsTrue(!handValue.IsSplit && handValue.IsSoft);

            listOfCardValues = new uint[]
            {
                13,
                2,
                1,
            };

            handValue = BasicStrategy.DetermineHandValue(listOfCardValues);
            Assert.AreEqual((uint)13, handValue.Value);
            Assert.IsTrue(!handValue.IsSplit && !handValue.IsSoft);

            listOfCardValues = new uint []
            {
                11,
                1,
                1,
                1
            };

            handValue = BasicStrategy.DetermineHandValue(listOfCardValues);
            Assert.AreEqual((uint)13, handValue.Value);
            Assert.IsTrue(!handValue.IsSplit && !handValue.IsSoft);

            listOfCardValues = new uint[]
            {
                12,
                9,
                8
            };

            handValue = BasicStrategy.DetermineHandValue(listOfCardValues);
            Assert.AreEqual((uint)27, handValue.Value);
            Assert.IsTrue(!handValue.IsSplit && !handValue.IsSoft);
        }

        [TestMethod]
        public void DetermineNPlayerNextPlay_Splits()
        {
            uint dcard = 1;
            uint pcard1 = 1;
            while (dcard <= 13)
            {
                while (pcard1 <= 13)
                {
                    var nextPlayerAction = BasicStrategy.DeterminePlayerNextPlay(new uint[] { pcard1, pcard1 }, dcard, true);
                    if (pcard1 == 1 ||
                        pcard1 == 8 ||
                        (pcard1 == 9 && dcard != 1 && dcard != 7 && dcard < 10) ||
                        (pcard1 == 7 && dcard != 1 && dcard <= 7) ||
                        (pcard1 == 6 && (dcard >= 2 && dcard <= 6)) ||
                        ((pcard1 == 3 || pcard1 == 4) && (dcard >= 4 && dcard <= 7)))
                    {
                        Assert.AreEqual(Enums.PlayAction.Split, nextPlayerAction);
                    }
                    else if (pcard1 == 9 &&
                      dcard == 1 || dcard == 7 || dcard >= 10)
                    {
                        Assert.AreEqual(Enums.PlayAction.Stand, nextPlayerAction);
                    }
                    else if ((pcard1 == 7 && (dcard == 1 || dcard >= 8)) ||
                      (pcard1 == 6 && (dcard <= 2 || dcard >= 7)) ||
                      (pcard1 == 4) ||
                      ((pcard1 == 3 || pcard1 == 2) && (dcard <= 3 || dcard >= 8)))
                    {
                        Assert.AreEqual(Enums.PlayAction.Hit, nextPlayerAction);
                    }
                    else
                    {
                        if (pcard1 == 5 || pcard1 >= 10)
                            Assert.AreNotEqual(Enums.PlayAction.Split, nextPlayerAction);
                        else
                            Assert.Fail("a split card condition was not accounted for");
                    }
                    pcard1++;
                }
                dcard++;
            }

            dcard = 1;
            pcard1 = 1;
            while (dcard <= 13)
            {
                while (pcard1 <= 13)
                {
                    var nextPlayerAction = BasicStrategy.DeterminePlayerNextPlay(new uint[] { pcard1, pcard1 }, dcard, false);
                    if (pcard1 == 1 ||
                        pcard1 == 8 ||
                        (pcard1 == 9 && dcard != 1 && dcard != 7 && dcard < 10) ||
                        (pcard1 == 7 && dcard != 1 && dcard <= 7) ||
                        (pcard1 == 6 && (dcard >= 2 && dcard <= 6)) ||
                        ((pcard1 == 3 || pcard1 == 4) && (dcard >= 4 && dcard <= 7)))
                    {
                        Assert.AreNotEqual(Enums.PlayAction.Split, nextPlayerAction);
                    }
                    else if (pcard1 == 9 &&
                      dcard == 1 || dcard == 7 || dcard >= 10)
                    {
                        Assert.AreEqual(Enums.PlayAction.Stand, nextPlayerAction);
                    }
                    else if ((pcard1 == 7 && (dcard == 1 || dcard >= 8)) ||
                      (pcard1 == 6 && (dcard <= 2 || dcard >= 7)) ||
                      (pcard1 == 4) ||
                      ((pcard1 == 3 || pcard1 == 2) && (dcard <= 3 || dcard >= 8)))
                    {
                        Assert.AreEqual(Enums.PlayAction.Hit, nextPlayerAction);
                    }
                    else
                    {
                        if (pcard1 == 5 || pcard1 >= 10)
                            Assert.AreNotEqual(Enums.PlayAction.Split, nextPlayerAction);
                        else
                            Assert.Fail("a split card condition was not accounted for");
                    }
                    pcard1++;
                }
                dcard++;
            }
        }

        [TestMethod]
        public void DeterminePlayerNextPlay_HardHands()
        {
            uint dcard = 1;
            while (dcard <= 13)
            {
                uint playerHandTotal = 5;
                while (playerHandTotal <= 21)
                {
                    var listOfCardValues = CardValueCreator("hard", playerHandTotal);
                    var nextPlayerAction = BasicStrategy.DeterminePlayerNextPlay(listOfCardValues, dcard, true);
                    if (playerHandTotal <= 8 ||
                        (playerHandTotal == 9 && (dcard <= 2 || dcard >= 7)) ||
                        (playerHandTotal == 10 && (dcard == 1 || dcard >= 10)) ||
                        (playerHandTotal == 11 && dcard == 1) ||
                        (playerHandTotal == 12 && (dcard <= 3 || dcard >= 7)) ||
                        ((playerHandTotal >= 13 && playerHandTotal <= 16) && (dcard == 1 || dcard >= 7)))
                    {
                        Assert.AreEqual(Enums.PlayAction.Hit, nextPlayerAction);
                    }
                    else if ((playerHandTotal == 9 && (dcard >= 3 && dcard <= 6)) ||
                      (playerHandTotal == 10 && (dcard >= 2 && dcard <= 9)) ||
                      (playerHandTotal == 11 && dcard != 1))
                    {
                        Assert.AreEqual(Enums.PlayAction.Double, nextPlayerAction);
                    }
                    else if ((playerHandTotal == 12 && (dcard >= 4 && dcard <= 6)) ||
                        ((playerHandTotal >= 13 && playerHandTotal <= 16) && (dcard >= 2 && dcard <= 6)) ||
                        (playerHandTotal >= 17))
                    {
                        Assert.AreEqual(Enums.PlayAction.Stand, nextPlayerAction);
                    }
                    else
                    {
                        Assert.Fail("a hard card condition was not accounted for");
                    }

                    playerHandTotal++;
                }
                dcard++;
            }
        }

        [TestMethod]
        public void DeterminePlayerNextPlay_SoftHands()
        {
            uint dcard = 1; 
            while (dcard <= 13)
            {
                uint playerHandTotal = 13;
                while (playerHandTotal <= 21)
                {
                    var listOfCardValues = CardValueCreator("soft", playerHandTotal);
                    var nextPlayerAction = BasicStrategy.DeterminePlayerNextPlay(listOfCardValues, dcard, true);

                    if (((playerHandTotal == 13 || playerHandTotal == 14) && (dcard <= 4 || dcard >= 7)) ||
                        ((playerHandTotal == 15 || playerHandTotal == 16) && (dcard <= 3 || dcard >= 7)) ||
                        (playerHandTotal == 17 && (dcard <= 2 || dcard >= 7)) ||
                        (playerHandTotal == 18 && (dcard >= 9 || dcard == 1)))
                    {
                        Assert.AreEqual(Enums.PlayAction.Hit, nextPlayerAction);
                    }
                    else if (playerHandTotal == 18 && (dcard == 2 || dcard == 7 || dcard == 8) ||
                        (playerHandTotal >= 19))
                    {
                        Assert.AreEqual(Enums.PlayAction.Stand, nextPlayerAction);
                    }
                    else if (((playerHandTotal == 13 || playerHandTotal == 14) && (dcard >= 5 && dcard <= 6)) ||
                        ((playerHandTotal == 15 || playerHandTotal == 16) && (dcard >= 4 && dcard <= 6)) ||
                        ((playerHandTotal == 17 || playerHandTotal == 18) && (dcard >= 3 && dcard <= 6)))
                    {
                        Assert.AreEqual(Enums.PlayAction.Double, nextPlayerAction);
                    }
                    else
                    {
                        Assert.Fail("a soft card condition was not accounted for");
                    }
                    playerHandTotal++;
                }
                dcard++;
            }
        }

        private uint[] CardValueCreator(string typeOfHand, uint handTotal)
        {
            if (typeOfHand == "hard")
            {
                if (handTotal % 2 == 0)
                {
                    return new uint[] { handTotal / 2 - 1, handTotal / 2 + 1 };
                }
                else
                {
                    return new uint[] { handTotal / 2, handTotal - (handTotal / 2) };
                }
            }
            if (typeOfHand == "soft")
            {
                return new uint[] { 1, handTotal - 11 };
            }
            Assert.Fail("error in CardValueCreator method");
            return null;
        }
    }
}