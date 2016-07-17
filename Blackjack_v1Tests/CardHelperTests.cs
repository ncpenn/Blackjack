using Blackjack.Helpers;
using Blackjack.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackjackTests
{
    [TestClass]
    public class CardHelperTests
    {
        [TestMethod]
        public void BlackJackTests()
        {
            for (var card1 = 1; card1 <= 13; card1++)
            {
                for (var card2 = 1; card2 < 13; card2++)
                {
                    var isBlackjack = CardHelper.IsBlackJack(new Card {Value = card1}, new Card {Value = card2});
                    if (card1 == 1 && card2 >= 10 || card2 == 1 && card1 >= 10)
                    {
                        Assert.IsTrue(isBlackjack);
                    }
                    else
                    {
                        Assert.IsFalse(isBlackjack);
                    }
                }
            }
        }

        [TestMethod]
        public void GetCountValuesTests()
        {
            for (var card = 1; card <= 13; card++)
            {
                var count = CardHelper.GetCountValueForCard(card);
                if (card == 1)
                {
                    Assert.AreEqual(-1, count);
                }
                else if (card == 5)
                {
                    Assert.AreEqual(1, count);
                }
                else
                {
                    Assert.AreEqual(0, count);
                }
            }
        }
    }
}
