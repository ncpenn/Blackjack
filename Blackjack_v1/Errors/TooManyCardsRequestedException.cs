using System;

namespace Blackjack.Errors
{
    public class TooManyCardsRequestedException : Exception
    {
        public TooManyCardsRequestedException(string msg) : base(msg)
        {
        }
    }
}
