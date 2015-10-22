using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack_v1
{
    public class Deck
    {
        public readonly List<Card> ReadyDeck;
        public Deck()
        {
            ReadyDeck = new List<Card>();
            var suits = new List<Enums.Suit>();

            suits.Add(Enums.Suit.Clubs);
            suits.Add(Enums.Suit.Hearts);
            suits.Add(Enums.Suit.Diamonds);
            suits.Add(Enums.Suit.Spades);

            foreach (var suit in suits)
            {
                for (var i = 1; i <= 13; i++)
                {
                    ReadyDeck.Add(new Card { Suit = suit, Value = (Enums.Value)i});
                }
            }
        }
    }
}
