namespace Blackjack.Contracts
{
    public class TableInfo
    {
        public double WhenToShuffleShoe { get; set; }
        public uint MaxBet { get; set; }
        public uint MinBet { get; set; }
        public uint NumberOfDecks { get; set; }
    }
}
