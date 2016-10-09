using Blackjack.Models;
using Blackjack.Rules.BasicStrategy.Interfaces;

namespace Blackjack.Rules.BasicStrategy
{
    public class BasicStrategy : IBasicStrategy
    {
        public Enums.PlayAction DetermineCorrectPlayAction(HandInformation handInformation, uint dealerUpCard)
        {
            Enums.PlayAction result;

            if (handInformation.IsSplitable)
            {
                result = DoSplitRules(handInformation.Cards[0], dealerUpCard);
            }
            else if (handInformation.IsSoft)
            {
                result = DoSoftRules(handInformation.HandValue, dealerUpCard);
            }
            else
            {
                result = DoHardRules(handInformation.HandValue, dealerUpCard);
            }

            if (!handInformation.OkayToDouble && result == Enums.PlayAction.Double)
            {
                result = Enums.PlayAction.Stand;
            }

            return result;
        }

        private Enums.PlayAction DoSoftRules(uint handValue, uint dealerHandValue)
        {
            var result = Enums.PlayAction.Hit;
            if (handValue >= 19)
            {
                result = Enums.PlayAction.Stand;
            }
            else if (handValue == 18)
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
            else if (handValue == 17)
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
            else if (handValue >= 15)
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
            else
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

        private Enums.PlayAction DoHardRules(uint handValue, uint dealerHandValue)
        {
            var result = Enums.PlayAction.Hit;

            if (handValue >= 17)
            {
                result = Enums.PlayAction.Stand;
            }
            else if (handValue >= 13)
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
            else if (handValue == 12)
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
            else if (handValue == 11)
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
            else if (handValue == 10)
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
            else if (handValue == 9)
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
            else
            {
                result = Enums.PlayAction.Hit;
            }
            return result;
        }

        private Enums.PlayAction DoSplitRules(uint cardValue, uint dealerCardValue)
        {
            var result = Enums.PlayAction.Hit;
            if (cardValue == 1 || cardValue == 8)
            {
                result = Enums.PlayAction.Split;
            }
            else if (cardValue == 9)
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
            else if (cardValue == 7)
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
            else if (cardValue == 6)
            {
                if (dealerCardValue >= 2 && dealerCardValue <= 6)
                {
                    result = Enums.PlayAction.Split;
                }
                else
                {
                    result = Enums.PlayAction.Hit;
                }
            }
            else if (cardValue == 4)
            {
                result = Enums.PlayAction.Hit;
            }
            else if (cardValue == 2 || cardValue == 3)
            {
                if (dealerCardValue >= 2 && dealerCardValue <= 7)
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
