using System.Collections.Generic;
using Blackjack.Models;

namespace Blackjack.Interfaces
{
    public interface IPlayer
    {
        bool IsCardCounter { get; }
        decimal BankRoll { get; }
        uint MainBet { get; set; }
        List<uint> MainHand { get; }
        List<uint> SplitHand { get; set; }
        void SetInitialCards(IEnumerable<uint> cards);
        PlayerHand GetMainHandTotal();
        PlayerHand GeSplitHandTotal();
        void IsAddingCurrentBetToBankroll(bool isAdding, bool wonWithBlackjack = false);
        void SetMainBet(uint bet);
        void SetSplitBet(uint bet);
    }
}
