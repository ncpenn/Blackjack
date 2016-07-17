using System.Collections.Generic;
using Blackjack.Models;

namespace Blackjack.Actors
{
    public class Deck
    {
        public readonly List<Card> ReadyDeck;
        public Deck()
        {
            ReadyDeck = new List<Card>();

            for (var suit = 0; suit < 4; suit++)
            {
                for (var i = 1; i <= 13; i++)
                {
                    ReadyDeck.Add(new Card { Suit = (Enums.Suit) suit, Value = i});
                }
            }
        }
    }
}
