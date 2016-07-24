using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackjack.Interfaces;

namespace Blackjack.Helpers
{
    public class BetHelper : IBetHelper
    {
        public uint CardCounterFigureBetSize(uint minimumBet, uint maxBet, int theCount)
        {
            var bet = (uint) Math.Pow(minimumBet, theCount);
            return bet <= maxBet ? bet : maxBet;
        }
    }
}
