using System;
using System.Collections.Generic;

namespace Blackjack.Models
{
    public class HandInformation
    {
        private uint _currentBet;
        private bool _canDouble;

        public HandInformation()
        {
            _canDouble = true;
            Cards = new List<uint>();
        }

        public List<uint> Cards { get; }

        public bool IsBusted
        {
            get
            {
                return HandValue() >= 21;
            }
        }

        public bool CanSplit { get; set; } = true;

        public bool IsSplitable
        {
            get
            {
                return Cards.Count == 2 && Cards[0] == Cards[1] && CanSplit;
            }
        }

        public bool IsBlackjack
        {
            get
            {
                return Cards.Count == 2 
                    && ((Cards[0] == 1 && Cards[1] >=10) || (Cards[0] >=10 && Cards[1] == 1));
            }
        }

        public bool OkayToDouble()
        {
            return _canDouble;
        }

        public bool IsSoft()
        {
            uint handValue = 0;
            int? keepAceTillLast = null;

            foreach (var card in Cards)
            {
                if (card == 1 && keepAceTillLast == null)
                {
                    keepAceTillLast = 1;
                    continue;
                }
                if (card >= 10)
                {
                    handValue += 10;
                }
                else
                {
                    handValue += card;
                }
            }

            if (keepAceTillLast != null)
            {
                if (handValue + 11 <= 21)
                {
                    return true;
                }
            }
            return false;
        }

        public uint HandValue()
        {
            uint handValue = 0;
            int? keepAceTillLast = null;
           
            foreach (var value in Cards)
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
                    handValue += 11;
                }
                else
                {
                    handValue += 1;
                }
            }
            return handValue;
        }

        public uint GetCurrentBet()
        {
            return _currentBet;
        }

        public void SetBet(uint betAount)
        {
            _currentBet = betAount;
        }

        public void InformHandThatItsDouble()
        {
            _canDouble = false;
            _currentBet = _currentBet * 2;
        }

        public void PayBlackjack(Bankroll bankroll)
        {
            bankroll.Amount = bankroll.Amount + (_currentBet * 1.5);
        }

        public void BetAmountToPlayer(Bankroll bankroll)
        {
            bankroll.Amount = bankroll.Amount + _currentBet;
        }

        public void BetAmountToDealer(Bankroll bankroll)
        {
            bankroll.Amount = bankroll.Amount - _currentBet;
        }
    }
}
