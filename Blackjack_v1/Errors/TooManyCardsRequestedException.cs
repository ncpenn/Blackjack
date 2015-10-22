using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack_v1
{
    class TooManyCardsRequestedException : Exception
    {
        public TooManyCardsRequestedException(string msg) : base(msg)
        {
        }
    }
}
