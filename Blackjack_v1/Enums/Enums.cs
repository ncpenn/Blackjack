namespace Blackjack_v1
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

        public enum Value
        {
            Ace = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10,
            Jack = 11,
            Queen = 12,
            King = 13,
        };

        public enum PlayAction
        {
            Hit = 1,
            Stand = 2,
            Double = 3,
            Split = 4,
        };
    }
}
