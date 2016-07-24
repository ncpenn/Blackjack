namespace Blackjack.Interfaces
{
    public interface IDealer
    {
        bool IsHandBlackjack();
        uint GetDealerHandValue();
        uint DealerUpCard { get; set; }
    }
}
