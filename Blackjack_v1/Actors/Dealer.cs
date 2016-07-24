using System;
using System.Collections.Generic;
using System.Linq;
using Blackjack.Actors.Actions;
using Blackjack.Actors.BaseClasses;
using Blackjack.Helpers;
using Blackjack.Interfaces;
using Blackjack.Models;

namespace Blackjack.Actors
{
    public class Dealer : DealerAction
    {
        private readonly int _numberOfRounds;
        private readonly List<uint> _dealersCards;
        private readonly List<Player> _players;
        private readonly IShoe _shoe;
        private bool _isCurrentDealerHandBlackJack;
        private uint _dealerHandValue;
        private IDealerHelper _dealerHelper;
        private ITable _table;
        private PlayCollection _playCollection;
        private ICardHelper _cardHelper;
        private IBasicStrategy _basicStrategy;

        public Dealer(IEnumerator<IPlayer> players, int numberOfRounds, int numberOfDecksToBeUsed, int tableMinBet, int tableMaxBet, Percent whenToShuffle)
        {
            //////Do(new InitialCards().Deal(), new PlayCollection());
            _shoe = new Shoe(numberOfDecksToBeUsed, whenToShuffle);
            _shoe.AnnounceTimeToShuffle += ShoeOnAnnounceTimeToShuffle;
            //Table.TableMaxBet = tableMaxBet;
            //Table.TableMinBet = tableMinBet;
            //_players = players;
            _numberOfRounds = numberOfRounds;
            _dealersCards = new List<uint>();
           // TheCount.CurrentCount = 0;
        }

        public void PlayGame()
        {
            var initalCards = new InitialCards();
            var bet = new Bet();
            var turn = new Turn();
            var round = 0;
            while (round < _numberOfRounds && _players.Any(/*p => p.BankRoll >= Table.TableMinBet*/))
            {
                _dealerHelper.InitRound(_table);
                Do(initalCards.Deal(), _playCollection);
                Do(bet.SetMainBet(), _playCollection);
                Do(turn.Play(), _playCollection);
                Do(bet.SetSplitBet(), _playCollection);
                var cards = _dealerHelper.DealToDealer(_dealersCards, _shoe, _cardHelper, _basicStrategy);
                _table.AddCardToVisibleCards(cards);
                Do(bet.Settle(), _playCollection);
                round++;
            }
        }

        private void ShoeOnAnnounceTimeToShuffle()
        {
            _shoe.Shuffle();
            _table.ResetCount();
        }
    }
}
