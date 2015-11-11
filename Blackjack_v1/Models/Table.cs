using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Blackjack_v1
{
    public class Table
    {
        public List<PlayerSlot> PlayerSlots { get; }
        public Shoe Shoe;
        public ObservableCollection<DealtCard> CardsVisibleThisRound { get; set; }
        public int TableMinBet { get; }
        public int TheCount { get; set; }
        public int DecksInUse { get; }
        public Percent PercentOfDeckLeftTriggeringShuffle { get; }
        public int TotalNumberOfCardsDealt { get; set; }

        public Table(List<Player> players, int numberOfDecksToBeUsed, int minBet, Percent whenToShuffle)
        {
            Shoe = new Shoe(numberOfDecksToBeUsed, whenToShuffle);
            PlayerSlots = new List<PlayerSlot>();
            TableMinBet = minBet;
            PercentOfDeckLeftTriggeringShuffle = whenToShuffle;
            DecksInUse = numberOfDecksToBeUsed;
            CardsVisibleThisRound = new ObservableCollection<DealtCard>();

            foreach (var player in players)
            {
                var info = new TableInformationVisibleToPlayers
                {
                    WhenDeckIsGoingToBeShuffled = whenToShuffle,
                    NumberOfDecksInUse = DecksInUse
                };
                PlayerSlots.Add(new PlayerSlot(player, info));
            }
        }

        public void DealtCard_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e != null && e.NewItems != null)
            {                   
                foreach (DealtCard card in e.NewItems)
                {
                    if (card.Value == Enums.Value.Five)
                    {
                        TheCount += 1;
                    }
                    if (card.Value == Enums.Value.Ace)
                    {
                        TheCount -= 1;
                    }
                }
            }
            //foreach (var y in e.OldItems)
            //{
            //    //do something
            //}
            //if (e.Action == NotifyCollectionChangedAction.Move)
            //{
            //    //do something
            //}
        }
    }
}
