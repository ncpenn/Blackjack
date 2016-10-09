using System.Collections.Generic;
using System.Threading;
using Blackjack.Actors;
using Blackjack.Actors.Interfaces;
using Blackjack.Contracts;

namespace Blackjack.Engine.Game
{
    public sealed class Control
    {
        private uint _numberOfRounds;
        private ITable _table;

        public Control(uint numberOfRounds, IList<PlayerInfo> playersInfo, TableInfo tableInfo)
        {
            _numberOfRounds = numberOfRounds;
            var players = MakePlayersFromPlayerInfo(playersInfo);
            _table = new Table(tableInfo.MinBet, tableInfo.MaxBet, tableInfo.NumberOfDecks, tableInfo.WhenToShuffleShoe, players, Dealer.Create());
        }

        public PlayStats PlayGame(CancellationToken cancelToken)
        {
            var resultsOfPlay = new PlayStats();
            for (uint round = 0; round < _numberOfRounds; round++)
            {
                if (cancelToken.IsCancellationRequested)
                {
                    resultsOfPlay.IsCancelledByUser(true);
                    return resultsOfPlay;
                }

                _table.InitialDealTo<IDealer>();
                _table.InitialDealTo<IPlayer>();
                _table.EngageEachPlayer();
                _table.EngageDealer();
                _table.SettlingWithEachPlayer();
                _table.UpdatePlayStats(resultsOfPlay);
                _table.CheckShoeIfNeedsShuffle();
            }

            return resultsOfPlay;
        }

        private static IList<IPlayer> MakePlayersFromPlayerInfo(IList<PlayerInfo> playersInfo)
        {
            var players = new List<IPlayer>();
            foreach (var playerInfo in playersInfo)
            {
                var player = new Player(playerInfo.Bankroll, playerInfo.IsCardCounter);
                players.Add(player);
            }

            return players;
        }
    }
}
