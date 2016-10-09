using Blackjack.Contracts;
using Blackjack.Engine.Game;
using System.Collections.Generic;
using System.Threading;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameControl = new Control(1000000, new List<PlayerInfo> { new PlayerInfo { Bankroll = 1000, IsCardCounter = false } }, new TableInfo { WhenToShuffleShoe = .25, MaxBet = 100, MinBet = 5, NumberOfDecks = 6 });
            var cancel = new CancellationToken();
            gameControl.PlayGame(cancel);
        }
    }
}
