using Blackjack.Helpers;
using Blackjack.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackjackTests
{
    [TestClass]
    public class CardHelperTests
    {   
        private readonly CardHelper _cardHelper = new CardHelper();

        [TestMethod]
        public void BlackJackTests()
        {
            for (uint card1 = 1; card1 <= 13; card1++)
            {
                for (uint card2 = 1; card2 < 13; card2++)
                {
                    var isBlackjack = _cardHelper.IsBlackJack(card1, card2);
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
            for (uint card = 1; card <= 13; card++)
            {
                var count = _cardHelper.GetCountValueForCard(card);
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
