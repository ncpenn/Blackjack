using System.Collections.Generic;
using Blackjack.Helpers;
using Blackjack.Interfaces;

namespace Blackjack.Models
{
    public class PlayCollection
    {
        public List<IPlayer> Players { get; set; }
        public ITable Table { get; set; }
        public IShoe Shoe { get; set; }
        public IDealer Dealer { get; set; }
        public IPaymentHelper PaymentHelper { get; set; }
        public IBetHelper BetHelper { get; set; }
        public IBasicStrategy BasicStrategu { get; set; }
    }
}
