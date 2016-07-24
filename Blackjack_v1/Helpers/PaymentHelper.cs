using Blackjack.Interfaces;
using Blackjack.Models;

namespace Blackjack.Helpers
{
    public class PaymentHelper
    {
        public void CreditDebitBets(Enums.PaymentFlow whoWon, IPlayer player)
        {
            switch (whoWon)
            {
                case Enums.PaymentFlow.PayDealer:
                    player.IsAddingCurrentBetToBankroll(false);
                    break;
                case Enums.PaymentFlow.PayPlayer:
                    player.IsAddingCurrentBetToBankroll(true);
                    break;
                case Enums.PaymentFlow.PayBlackjack:
                    player.IsAddingCurrentBetToBankroll(true, true);
                    break;
                case Enums.PaymentFlow.Push:
                    break;
            }
        }

        public Enums.PaymentFlow DetermineWinner(PlayerHand playerHand, IDealer dealer)
        {
            var dealerHandValue = dealer.GetDealerHandValue();
            var isDealerHandBlackjack = dealer.IsHandBlackjack();
            if ((playerHand.IsBlackJack && isDealerHandBlackjack) ||
                (playerHand.HandTotal == dealerHandValue && !playerHand.IsBlackJack && isDealerHandBlackjack) ||
                (playerHand.HandTotal > 21 && dealerHandValue > 21))
            {
                return Enums.PaymentFlow.Push;
            }
            if (playerHand.IsBlackJack)
            {
                return Enums.PaymentFlow.PayBlackjack;
            }
            if (playerHand.HandTotal > 21 ||
                (playerHand.HandTotal < dealerHandValue && dealerHandValue <= 21) ||
                isDealerHandBlackjack)
            {
                return Enums.PaymentFlow.PayDealer;
            }
            return Enums.PaymentFlow.PayPlayer;
        }
    }
}
