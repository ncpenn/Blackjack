using Blackjack.Models;

namespace Blackjack.Helpers
{
    public static class CardHelper
    {
        public static bool IsBlackJack(Card card1, Card card2)
        {
            return (card1.Value == 1 && card2.Value >= 10) ||
                   (card2.Value == 1 && card1.Value >= 10);
        }

        public static int GetCountValueForCard(int cardValue)
        {
            if (cardValue == 5)
            {
                return 1;
            }
            if (cardValue == 1)
            {
                return -1;
            }
            return 0;
        }
    }
}
