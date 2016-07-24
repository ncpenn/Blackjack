using Blackjack.Interfaces;

namespace Blackjack.Helpers
{
    public class CardHelper : ICardHelper
    {
        public bool IsBlackJack(uint card1, uint card2)
        {
            return (card1 == 1 && card2 >= 10) ||
                   (card2 == 1 && card1 >= 10);
        }

        public int GetCountValueForCard(uint cardValue)
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
