using System.Collections.Specialized;
using Blackjack.Helpers;

namespace Blackjack.Models
{
    public static class TheCount
    {
        public static int CurrentCount { get; set; }

        static TheCount()
        {
            Table.VisibleCards.CollectionChanged += DealtCard_CollectionChanged;
        }

        public static void DealtCard_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e?.NewItems != null)
            {
                foreach (Card card in e.NewItems)
                {
                    CurrentCount += CardHelper.GetCountValueForCard(card.Value);
                }
            }
        }
    }
}
