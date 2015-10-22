using System.Collections.Generic;

namespace Blackjack_v1
{
    public class PlayerSlot
    {
        public Player Player { get; }
        public List<DealtCard> DealtCards { get; set; }
        public int BetAmount { get; set; }
        public bool IsStanding { get; set; }

        public List<DealtCard> Split { get; set; }
        public bool IsStandingOnSplit { get; set; }
        public int? BetAmountOnSplit { get; set; }
        public TableInformationVisibleToPlayers CardTotalsInfo { get; set; }
        private int numberOfTimesShoeHasBeenShuffled { get; set; }

        public PlayerSlot(Player player, TableInformationVisibleToPlayers info)
        {
            DealtCards = new List<DealtCard>();
            Split = new List<DealtCard>();
            IsStandingOnSplit = true;
            Player = player;
            CardTotalsInfo = info;
        }

        public void LayDownBet(int minimumBet, int theCount, int totalCardsDealt)
        {
            BetAmount = Player.PlaceBet(minimumBet, theCount, totalCardsDealt);
        }

        public bool ShouldCountBeReset(int totalCardsDealt)
        {
            var cardsInShoeIfItsFull = CardTotalsInfo.NumberOfDecksInUse * TableInformationVisibleToPlayers.CardsInADeck;
            double actualPercentOfCardsDealtPerShoe = 1 - CardTotalsInfo.WhenDeckIsGoingToBeShuffled.PercentValue / (double)100;
            var cardsActuallyDealtOutOfShoe = cardsInShoeIfItsFull * actualPercentOfCardsDealtPerShoe;
            var timesShoeHasBeenShuffled = totalCardsDealt / cardsActuallyDealtOutOfShoe;

            if (numberOfTimesShoeHasBeenShuffled != timesShoeHasBeenShuffled)
            {
                numberOfTimesShoeHasBeenShuffled = (int)timesShoeHasBeenShuffled;
                return true;
            }
            return false;
        }
    }
}
