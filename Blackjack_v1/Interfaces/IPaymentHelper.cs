using Blackjack.Models;

namespace Blackjack.Interfaces
{
    public interface IPaymentHelper
    {
        void CreditDebitBets(Enums.PaymentFlow whoWon, IPlayer player);
        Enums.PaymentFlow DetermineWinner(PlayerHand playerHand, IDealer dealer);
    }
}
