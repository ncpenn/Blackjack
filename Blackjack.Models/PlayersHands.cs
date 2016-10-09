using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Blackjack.Models
{
    public class PlayersHands
    {
        public PlayersHands()
        {
            Hands = new HandInformation[2];
            Hands[0] = new HandInformation(isDealerHand: false);
            Hands[1] = new HandInformation(isDealerHand: false);
        }

        public HandInformation[] Hands { get; }

        public bool CanSplit
        {
            get
            {
                return !Hands[1].Cards.Any();
            }
        }

        public void SetMainHand(uint[] cards)
        {
            Hands[0].Cards.AddRange(cards);
        }

        public void SetSplit()
        {
            Hands[1].Cards.Add(Hands[0].Cards[1]);
            Hands[0].Cards.RemoveAt(1);
        }
    }
}
