using System.Collections.ObjectModel;
using System.Linq;
using Blackjack.Models;

namespace Blackjack
{
    public static class BasicStrategy
    {
        private static readonly ReadOnlyCollection<uint> NotValidSplitValues = new ReadOnlyCollection<uint>(
            new uint[] {
                5,
                10,
                11,
                12,
                13
            });

        public static Enums.PlayAction DeterminePlayerNextPlay(uint[] cardValues, uint dealerCardValue, bool canSplit)
        {
            Enums.PlayAction result;
            
            var handValue = DetermineHandValue(cardValues);
            handValue.IsSplit = handValue.IsSplit && canSplit;
            if (handValue.IsSplit && !NotValidSplitValues.Contains(cardValues.First()))
            {
                result = DoSplitRules(cardValues.First(), dealerCardValue);            
            }
            else if (handValue.IsSoft)
            {
                result = DoSoftRules(handValue.Value, dealerCardValue);
            }
            else
            {
                result = DoHardRules(handValue.Value, dealerCardValue);
            }
            return result;      
        }

        public static HandValue DetermineHandValue(uint[] cards)
        {
            var handValue = new HandValue();
            int? keepAceTillLast = null;
            if (cards.Length == 2 && cards.Distinct().Count() == 1)
            {
                handValue.IsSplit = true;
            }

            foreach (var value in cards)
            {
                if (value == 1 && keepAceTillLast == null)
                {
                    keepAceTillLast = 1;
                    continue;
                }
                if (value >= 10)
                {
                    handValue.Value += 10;
                }
                else
                {
                    handValue.Value += value;
                }                
            }

            if (keepAceTillLast != null)
            {
                if (handValue.Value + 11 <= 21)
                {
                    if (handValue.Value + 11 == 12 && handValue.IsSplit) handValue.IsSoft = false;
                    else handValue.IsSoft = true;
                    handValue.Value += 11;
                }
                else
                {
                    handValue.Value += 1;
                }
            }
            return handValue;
        }

        private static Enums.PlayAction DoSoftRules(uint handValue, uint dealerHandValue)
        {
            var result = Enums.PlayAction.Hit;
            if (handValue >= 19)
            {
                result = Enums.PlayAction.Stand;
            }
            if (handValue == 18)
            {
                if (dealerHandValue >= 9 || dealerHandValue == 1)
                {
                    result = Enums.PlayAction.Hit;
                }
                else
                {
                    if (dealerHandValue >= 3 && dealerHandValue <= 6)
                    {
                        result = Enums.PlayAction.Double;
                    }
                    else
                    {
                        result = Enums.PlayAction.Stand;
                    }
                }
            }
            if (handValue == 17)
            {
                if (dealerHandValue >= 3 && dealerHandValue <= 6)
                {
                    result = Enums.PlayAction.Double;
                }
                else
                {
                    result = Enums.PlayAction.Hit;
                }
            }
            if (handValue == 16 || handValue == 15)
            {
                if (dealerHandValue >= 4 && dealerHandValue <= 6)
                {
                    result = Enums.PlayAction.Double;
                }
                else
                {
                    result = Enums.PlayAction.Hit;
                }
            }
            if (handValue == 14 || handValue == 13)
            {
                if (dealerHandValue >= 5 && dealerHandValue <= 6)
                {
                    result = Enums.PlayAction.Double;
                }
                else
                {
                    result = Enums.PlayAction.Hit;
                }
            }
            return result;
        }

        private static Enums.PlayAction DoHardRules(uint handValue, uint dealerHandValue)
        {
            var result = Enums.PlayAction.Hit;

            if (handValue >= 17)
            {
                result = Enums.PlayAction.Stand;
            }
            if (handValue <= 16 && handValue >= 13)
            {
                if (dealerHandValue <= 6 && dealerHandValue != 1)
                {
                    result = Enums.PlayAction.Stand;
                }
                else
                {
                    result = Enums.PlayAction.Hit;
                }
            }
            if (handValue == 12)
            {
                if (dealerHandValue >= 4 && dealerHandValue <= 6)
                {
                    result = Enums.PlayAction.Stand;
                }
                else
                {
                    result = Enums.PlayAction.Hit;
                }
            }
            if (handValue == 11)
            {
                if (dealerHandValue == 1)
                {
                    result = Enums.PlayAction.Hit;
                }
                else
                {
                    result = Enums.PlayAction.Double;
                }
            }
            if (handValue == 10)
            {
                if (dealerHandValue >= 2 && dealerHandValue <= 9)
                {
                    result = Enums.PlayAction.Double;
                }
                else
                {
                    result = Enums.PlayAction.Hit;
                }
            }
            if (handValue == 9)
            {
                if (dealerHandValue >= 3 && dealerHandValue <= 6)
                {
                    result = Enums.PlayAction.Double;
                }
                else
                {
                    result = Enums.PlayAction.Hit;
                }
            }
            if (handValue <= 8)
            {
                result = Enums.PlayAction.Hit;
            }
            return result;
        }

        private static Enums.PlayAction DoSplitRules(uint cardValue, uint dealerCardValue)
        {
            var result = Enums.PlayAction.Hit;
            if (cardValue == 1 || cardValue == 8) result = Enums.PlayAction.Split;

            if (cardValue == 9)
            {
                if (dealerCardValue == 7 || dealerCardValue >= 10 || dealerCardValue == 1)
                {
                    result = Enums.PlayAction.Stand;
                }
                else
                {
                    result = Enums.PlayAction.Split;
                }
            }

            if (cardValue == 7)
            {
                if (dealerCardValue >= 8 || dealerCardValue == 1)
                {
                    result = Enums.PlayAction.Hit;
                }
                else
                {
                    result = Enums.PlayAction.Split;
                }
            }

            if (cardValue == 6)
            {
                if (dealerCardValue >= 3 && dealerCardValue <= 6)
                {
                    result = Enums.PlayAction.Split;
                }
                else
                {
                    result = Enums.PlayAction.Hit;
                }
            }

            if (cardValue == 4)
            {
                result = Enums.PlayAction.Hit;
            }

            if (cardValue == 2 || cardValue == 3)
            {
                if (dealerCardValue >= 4 && dealerCardValue <= 7)
                {
                    result = Enums.PlayAction.Split;
                }
                else
                {
                    result = Enums.PlayAction.Hit;
                }
            }
            return result;
        }
    }
}
