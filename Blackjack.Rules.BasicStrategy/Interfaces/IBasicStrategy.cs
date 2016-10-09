using Blackjack.Models;

namespace Blackjack.Rules.BasicStrategy.Interfaces
{
    public interface IBasicStrategy
    {
        Enums.PlayAction DetermineCorrectPlayAction(HandInformation handInformation, uint dealerUpCard);
    }
}
