using Blackjack_v1.Helpers;
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
                    var bet = PlayerHelper.FigureBetSize(minimumBet, lastBet);
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

            return PlayerHelper.ShouldAskForAnotherCard(cardValues, dealerCard, out specialAction, isForRegularPile, howCloseToPerfectStategy);       
        }
    }
}
