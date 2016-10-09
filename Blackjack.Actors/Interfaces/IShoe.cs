namespace Blackjack.Actors.Interfaces
{
    public interface IShoe
    {
        bool ShuffleNeeded { get; }
        uint[] CardRequest(int numberOfCardsRequested);
        uint CardRequest();
        void Shuffle(uint numberOfTimesToShuffle = 5);
    }
}
