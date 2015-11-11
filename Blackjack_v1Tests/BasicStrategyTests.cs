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
            //insure both methods return the same values
            bool isSoft;
            bool isSplit;
            int expectedHandValue;
            var listOfCardValues = new List<int> { 1, 1 };
            while (listOfCardValues[0] != (int)Enums.Value.King)
            {
                while (listOfCardValues[1] != (int)Enums.Value.King)
                {
                    var handvalue = BasicStrategy.DetermineHandValue(listOfCardValues);
                    Assert.AreEqual(BasicStrategy.DetermineHandValue(listOfCardValues, out isSplit,out isSoft), handvalue);
                    listOfCardValues[1]++;
                }
                listOfCardValues[0]++;
            }

            // hard hand values from 2 pip to 10 pip
            listOfCardValues = new List<int> { 2, 2 };
            while (listOfCardValues[0] != (int)Enums.Value.Ten)
            {
                while (listOfCardValues[1] != (int)Enums.Value.Ten)
                {
                    var handvalue = BasicStrategy.DetermineHandValue(listOfCardValues, out isSplit, out isSoft);
                    Assert.AreEqual(listOfCardValues.Sum(), handvalue);
                    Assert.IsFalse(isSoft);
                    listOfCardValues[1]++;
                }
                listOfCardValues[0]++;
            }

            // soft hands
            listOfCardValues = new List<int> { 1, 1 };
            while (listOfCardValues[0] != (int)Enums.Value.King)
            {
                var handvalue = BasicStrategy.DetermineHandValue(listOfCardValues, out isSplit , out isSoft);
                expectedHandValue = listOfCardValues[0] < 10 ? 11 + listOfCardValues[0] : 21;
                Assert.AreEqual(expectedHandValue, handvalue);
                if(listOfCardValues[0] == 1 && listOfCardValues[1] == 1)
                    Assert.IsFalse(isSoft);
                else
                    Assert.IsTrue(isSoft);
                listOfCardValues[0]++;
            }

            // hands with ten values
            listOfCardValues = new List<int> { 10, 1 };
            while (listOfCardValues[0] != (int)Enums.Value.King)
            {
                while (listOfCardValues[1] != (int)Enums.Value.King)
                {
                    var handvalue = BasicStrategy.DetermineHandValue(listOfCardValues);
                    if (listOfCardValues.Any(c => c == 1))
                        expectedHandValue = 21;
                    else if (listOfCardValues[1] > 10)
                        expectedHandValue = 20;
                    else
                        expectedHandValue = 10 + listOfCardValues[1];
                    Assert.AreEqual(expectedHandValue, handvalue);
                    listOfCardValues[1]++;
                }
                listOfCardValues[0]++;
            }

            // hands with splits
            listOfCardValues = new List<int> { 1, 1 };
            while (listOfCardValues[0] != (int)Enums.Value.King)
            {
                var handvalue = BasicStrategy.DetermineHandValue(listOfCardValues, out isSplit, out isSoft);
                if (listOfCardValues.Any(c => c == 1))
                {
                    Assert.AreEqual(12, handvalue);
                    Assert.IsFalse(isSoft);
                    Assert.IsTrue(isSplit);
                }
                else if (listOfCardValues[1] > 10)
                {
                    Assert.AreEqual(20, handvalue);
                    Assert.IsTrue(isSplit);
                    Assert.IsFalse(isSoft);
                }
                else
                {
                    Assert.AreEqual(listOfCardValues[0] * 2, handvalue);
                    Assert.IsTrue(isSplit);
                    Assert.IsFalse(isSoft);
                }                
                listOfCardValues[1]++;
                listOfCardValues[0]++;
            }

            listOfCardValues = new List<int>
            {
                1,
                1,
                1,
            };

            var handValue = BasicStrategy.DetermineHandValue(listOfCardValues, out isSplit, out isSoft);
            Assert.AreEqual(13, handValue);   
            Assert.IsTrue(!isSplit && isSoft);

            listOfCardValues = new List<int>
            {
                13,
                2,
                1,
            };

            handValue = BasicStrategy.DetermineHandValue(listOfCardValues, out isSplit, out isSoft);
            Assert.AreEqual(13, handValue);
            Assert.IsTrue(!isSplit && !isSoft);

            listOfCardValues = new List<int>
            {
                11,
                1,
                1,
                1
            };

            handValue = BasicStrategy.DetermineHandValue(listOfCardValues, out isSplit, out isSoft);
            Assert.AreEqual(13, handValue);
            Assert.IsTrue(!isSplit && !isSoft);

            listOfCardValues = new List<int>
            {
                12,
                9,
                8
            };

            handValue = BasicStrategy.DetermineHandValue(listOfCardValues, out isSplit, out isSoft);
            Assert.AreEqual(27, handValue);
            Assert.IsTrue(!isSplit && !isSoft);
        }

        [TestMethod]
        public void DeterminePlayerNextPlay()
        {
            //testing splits      
            var dcard = 1;
            var pcard1 = 1;
            while (dcard <= (int)Enums.Value.King)
            {
                while (pcard1 <= (int)Enums.Value.King)
                {
                    var nextPlayerAction = BasicStrategy.DeterminePlayerNextPlay(new List<int> { pcard1 , pcard1 }, dcard);
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

            //testing hard hands
            dcard = 1;
            var playerHandTotal = 5;
            while (dcard <= (int)Enums.Value.King)
            {
                while (playerHandTotal <= 21)
                {
                    var listOfCardValues = CardValueCreator("hard", playerHandTotal);
                    var nextPlayerAction = BasicStrategy.DeterminePlayerNextPlay(listOfCardValues, dcard);
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

            //testing soft hands
            dcard = 1;
            playerHandTotal = 13;
            while (dcard <= (int)Enums.Value.King)
            {
                while (playerHandTotal <= 21)
                {
                    var listOfCardValues = CardValueCreator("soft", playerHandTotal);
                    var nextPlayerAction = BasicStrategy.DeterminePlayerNextPlay(listOfCardValues, dcard);

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

        private List<int> CardValueCreator(string typeOfHand, int handTotal)
        {
            if (typeOfHand == "hard")
            {
                if (handTotal % 2 == 0)
                {
                    return new List<int> { handTotal / 2 - 1, handTotal / 2 + 1 };
                }
                else
                {
                    return new List<int> { handTotal / 2, handTotal - (handTotal / 2) };
                }
            }
            if (typeOfHand == "soft")
            {
                return new List<int> { 1, handTotal - 11 };
            }
            Assert.Fail("error in CardValueCreator method");
            return null;
        }
    }
}