using System;
using Blackjack.Actors.Interfaces;
using Blackjack.Contracts;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Blackjack.Actors
{
    public class Table : ITable
    {
        private uint _minBet;
        private uint _maxBet;
        private List<uint> _visibleCards;
        private IList<IPlayer> _players;
        private IDealer _dealer;
        private IShoe _shoe;

        public Table(uint minBet, uint maxBet, uint numberOfDecks, double whenToShuffleShoe, IList<IPlayer> players, IDealer dealer)
        {
            _visibleCards = new List<uint>();
            _shoe = new Shoe(numberOfDecks, whenToShuffleShoe);
            _minBet = minBet;
            _maxBet = maxBet;
            _players = players;
            _dealer = dealer;
        }

        public Table(uint minBet, uint maxBet, uint numberOfDecks, double whenToShuffleShoe, IList<IPlayer> players, IDealer dealer, IShoe shoeMock)
        {
            _visibleCards = new List<uint>();
            _shoe = shoeMock;
            _minBet = minBet;
            _maxBet = maxBet;
            _players = players;
            _dealer = dealer;
        }

        public void EngageDealer()
        {
            var cardsNowVisible = _dealer.PlayHand(_shoe);
            _visibleCards.AddRange(cardsNowVisible);
        }

        public void EngageEachPlayer()
        {
            foreach (var player in _players)
            {
                var cardsNowVisible = player.PlayHand(_shoe, _dealer.Hand.Cards[0], _visibleCards, _minBet, _maxBet);
                _visibleCards.AddRange(cardsNowVisible);
            }
        }

        public void InitialDealTo<T>()
        {
            var incomingType = typeof(T).ToString();
            var dealerType = typeof(IDealer).ToString();
            var playerType = typeof(IPlayer).ToString();

            if (string.Compare(incomingType, dealerType, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                var cards = _shoe.CardRequest(2);
                _dealer.SetNewHand(cards);
                _dealer.Hand.CanSplit = false;
                _visibleCards.Add(cards[0]);
            }
            else if (string.Compare(incomingType, playerType, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                foreach (var player in _players)
                {
                    var cards = _shoe.CardRequest(2);
                    player.SetNewHand(cards);
                    _visibleCards.AddRange(cards);
                }
            }
            else
            {
                throw new ArgumentException("Unknown type", nameof(T));
            }
        }

        public void SettlingWithEachPlayer()
        {
            foreach (var player in _players)
            {
                player.Settle(_dealer.Hand);
            }
        }

        public void UpdatePlayStats(PlayStats resultsOfPlay)
        {
            resultsOfPlay.PlayersSnapshot.Clear();
            resultsOfPlay.PlayersSnapshot.AddRange(_players.Select(p => JsonConvert.SerializeObject(p)));
        }

        public void CheckShoeIfNeedsShuffle()
        {
            if (_shoe.ShuffleNeeded)
            {
                _shoe.Shuffle();
            }
        }
    }
}
