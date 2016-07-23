using System.Collections.Generic;
using Blackjack.Models;

namespace Blackjack.Actors
{
    public class Deck
    {
        public readonly List<uint> ReadyDeck;
        public Deck()
        {
            ReadyDeck = new List<uint>();

            for (var suit = 0; suit < 4; suit++)
            {
                for (uint i = 1; i <= 13; i++)
                {
                    ReadyDeck.Add(i);
                }
            }
        }
    }
}
