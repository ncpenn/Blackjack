using Blackjack.Actors.Interfaces;
using System;
using Blackjack.Models;
using System.Collections.Generic;

namespace Blackjack.Actors
{
    public class Dealer : IDealer
    {
        public HandInformation Hand { get; private set; }

        public static IDealer Create()
        {
            return new Dealer();
        }

        public IEnumerable<uint> PlayHand(IShoe _shoe)
        {
            var cardsDealt = new List<uint>();
            while (Hand.HandValue() < 17)
            {
                var card = _shoe.CardRequest();
                Hand.Cards.Add(card);
                cardsDealt.Add(card);
            }
            return cardsDealt;
        }

        public void SetNewHand(uint[] cards)
        {
            Hand = new HandInformation();
            Hand.Cards.AddRange(cards);
        }
    }
}
