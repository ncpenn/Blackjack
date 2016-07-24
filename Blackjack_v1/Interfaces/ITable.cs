using System.Collections.Generic;
using System.Collections.Specialized;

namespace Blackjack.Interfaces
{
    public interface ITable
    {
        int CurrentCount { get; }
        uint TableMinBet { get; }
        uint TableMaxBet { get; }
        void ClearVisibleCardsOffTable();
        void AddCardToVisibleCards(uint card);
        void AddCardToVisibleCards(IEnumerable<uint> cards);
        void SetBetLimits(uint minBet, uint maxBet);
        void DealtCard_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e);
    }
}
