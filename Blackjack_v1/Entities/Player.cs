using System;
using System.Collections.Generic;
using System.Linq;

namespace Blackjack_v1
{
    public class Player
    {
        public readonly bool IsCardCounter;
        public decimal BankRoll;
        private readonly Percent howCloseToPerfectStategy;
        private int lastBet {get; set;}

        public Player(int startingBankRoll, bool isCardCounter, Percent howCloseToPerfectStategy)
        {
            BankRoll = startingBankRoll;
            this.IsCardCounter = isCardCounter;
            this.howCloseToPerfectStategy = howCloseToPerfectStategy;
        }

        public int PlaceBet(int minimumBet, int theCount, int totalCardsDealt)
        {
            if (IsCardCounter)
            { 
                if (theCount <= 1 && BankRoll >= minimumBet)
                {
                    lastBet = minimumBet;
                    return minimumBet;
                }
                else
                {
                    var bet = FigureBetSize(minimumBet);
                    if (BankRoll >= bet) return bet;
                    if (BankRoll >= minimumBet) return minimumBet;
                    return 0;
                }
            }
            else
            {
                if (BankRoll >= minimumBet)
                {
                    return minimumBet;
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool IsRequestingCard(List<DealtCard> playerCardsSoFar, DealtCard dealerCard, out string specialAction, bool isForRegularPile)
        {
            specialAction = string.Empty;
            var cardValues = playerCardsSoFar.Select(card => (int)card.Value).ToList();
            var handValue = BasicStrategy.DetermineHandValue(cardValues);

            if (handValue > 21)
            {
                return false;
            }

            return ShouldAskForAnotherCard(cardValues, dealerCard, out specialAction, isForRegularPile);       
        }

        private int FigureBetSize(int minimumBet)
        {
            var ceilingBet = 8 * minimumBet;

            if (lastBet * 2 <= ceilingBet)
            {
                return lastBet * 2;
            }
            return ceilingBet;
        }
        
        private bool ShouldAskForAnotherCard(List<int> cardValues, DealtCard dealerCard, out string specialAction, bool isForRegularPile)
        {
            specialAction = string.Empty;
            var action = BasicStrategy.DeterminePlayerNextPlay(cardValues, (int)dealerCard.Value);

            var random = new Random();
            var chanceOfPlayingRight = random.Next(0, 101);

            if (chanceOfPlayingRight <= howCloseToPerfectStategy.PercentValue)
            {
                switch (action)
                {
                    case Enums.PlayAction.Double:
                        specialAction = "double";
                        return true;
                    case Enums.PlayAction.Stand:
                        return false;
                    case Enums.PlayAction.Hit:
                        return true;
                    default:
                        specialAction = "split";
                        return isForRegularPile;
                }
            }
            return true;
        }
    }
}
