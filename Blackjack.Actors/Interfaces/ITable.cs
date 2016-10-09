using Blackjack.Contracts;

namespace Blackjack.Actors.Interfaces
{
    public interface ITable
    {
        void InitialDealTo<T>();

        void EngageEachPlayer();

        void EngageDealer();

        void SettlingWithEachPlayer();
        void UpdatePlayStats(PlayStats resultsOfPlay);
        void CheckShoeIfNeedsShuffle();
    }
}
