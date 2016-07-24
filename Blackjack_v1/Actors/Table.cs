using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Blackjack.Interfaces;

namespace Blackjack.Actors
{
    public class Table : ITable
    {
        public int CurrentCount { get; private set; }
        public uint TableMinBet { get; private set; }
        public uint TableMaxBet { get; private set; }

        private readonly ICardHelper _cardHelper;
        private readonly ObservableCollection<uint> _visibleCards;
        
        public Table(ICardHelper cardHelper)
        {
            _cardHelper = cardHelper;
            _visibleCards = new ObservableCollection<uint>();
            _visibleCards.CollectionChanged += DealtCard_CollectionChanged;
        }

        public void ClearVisibleCardsOffTable()
        {
            _visibleCards.Clear();
        }

        public void AddCardToVisibleCards(uint card)
        {
            _visibleCards.Add(card);
        }

        public void AddCardToVisibleCards(IEnumerable<uint> cards)
        {
            foreach (var c in cards)
            {
                _visibleCards.Add(c);
            }
        }

        public void SetBetLimits(uint minBet, uint maxBet)
        {
            TableMaxBet = Math.Max(minBet, maxBet);
            TableMinBet = Math.Min(minBet, maxBet);
        }

        public void DealtCard_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e?.NewItems != null)
            {
                foreach (uint card in e.NewItems)
                {
                    CurrentCount += _cardHelper.GetCountValueForCard(card);
                }
            }
        }
    }
}
