namespace Blackjack.Models
{
    public class Enums
    {
        public enum Suit
        {
            Hearts,
            Clubs,
            Spades,
            Diamonds
        };

        public enum PlayAction
        {
            Hit = 1,
            Stand = 2,
            Double = 3,
            Split = 4,
        };

        public enum PaymentFlow
        {
            Push,
            PayDealer,
            PayPlayer,
            PayBlackjack
        };
    }
}
