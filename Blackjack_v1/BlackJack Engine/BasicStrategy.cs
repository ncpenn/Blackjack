using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Blackjack_v1
{
    public static class BasicStrategy
    {
        private static ReadOnlyCollection<int> notValidSplitValues = new ReadOnlyCollection<int>(
            new[] {
                (int)Enums.Value.Five,
                (int)Enums.Value.Ten,
                (int)Enums.Value.Jack,
                (int)Enums.Value.Queen,
                (int)Enums.Value.King
            });

        public static Enums.PlayAction DeterminePlayerNextPlay(List<int> cardValues, int dealerCardValue)
        {
            Enums.PlayAction result;
            bool isSplit;
            bool isSoft;

            var handValue = DetermineHandValue(cardValues, out isSplit, out isSoft);

            if (isSplit && !notValidSplitValues.Contains(cardValues.First()))
            {
                result = DoSplitRules(cardValues.First(), dealerCardValue);            
            }
            else if (isSoft)
            {
                result = DoSoftRules(handValue, dealerCardValue);
            }
            else
            {
                isSplit = false;
                result = DoHardRules(handValue, dealerCardValue);
            }
            return result;      
        }

        public static int DetermineHandValue(List<int> cards, out bool isSplit, out bool isSoft)
        {
            var handValue = 0;
            isSplit = false;
            isSoft = false;
            int? keepAceTillLast = null;
            if (cards.Count == 2 && cards.Distinct().ToArray().Length == 1)
            {
                isSplit = true;
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
                    handValue += 10;
                }
                else
                {
                    handValue += value;
                }                
            }

            if (keepAceTillLast != null)
            {
                if (handValue + 11 <= 21)
                {
                    if (handValue + 11 == 12 && isSplit)
                        isSoft = false;
                    else
                        isSoft = true;
                    return handValue + 11;
                }
                return handValue + 1;
            }
            return handValue;
        }

        public static int DetermineHandValue(List<int> cards)
        {
            var handValue = 0;
            int? keepAceTillLast = null;

            foreach (var value in cards)
            {
                if (value == 1 && keepAceTillLast == null)
                {
                    keepAceTillLast = 1;
                    continue;
                }
                if (value >= 10)
                {
                    handValue += 10;
                }
                else
                {
                    handValue += value;
                }
            }

            if (keepAceTillLast != null)
            {
                if (handValue + 11 <= 21)
                {
                    return handValue + 11;
                }
                return handValue + 1;
            }
            return handValue;
        }

        private static Enums.PlayAction DoSoftRules(int handValue, int dealerHandValue)
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

        private static Enums.PlayAction DoHardRules(int handValue, int dealerHandValue)
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

        private static Enums.PlayAction DoSplitRules(int cardValue, int dealerCardValue)
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
