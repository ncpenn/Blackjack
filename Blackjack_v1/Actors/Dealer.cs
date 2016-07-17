﻿using System.Collections.Generic;
using System.Linq;
using Blackjack.Helpers;
using Blackjack.Models;

namespace Blackjack.Actors
{
    public class Dealer
    {
        private readonly int _numberOfRounds;
        private readonly List<Card> _dealersCards;
        private readonly List<Player> _players;
        private readonly Shoe _shoe;
        private bool _isCurrentDealerHandBlackJack;
        private int _dealerHandValue;

        public Dealer(List<Player> players, int numberOfRounds, int numberOfDecksToBeUsed, int tableMinBet, int tableMaxBet, Percent whenToShuffle)
        {
            _shoe = new Shoe(numberOfDecksToBeUsed, whenToShuffle);
            Table.TableMaxBet = tableMaxBet;
            Table.TableMinBet = tableMinBet;
            _players = players;
            _numberOfRounds = numberOfRounds;
            _dealersCards = new List<Card>();
            TheCount.CurrentCount = 0;
        }

        public void PlayGame()
        {
            var round = 0;
            while (round < _numberOfRounds && _players.Any(p => p.BankRoll >= Table.TableMinBet))
            {
                SetUpTableForRound();
                ShuffleShoeIfNeeded();
                DealInitialCards();
                HandleEachPlayer();
                DealerDealsToSelf();
                SettleBets();
                round++;
            }
        }

        private void SetUpTableForRound()
        {
            Table.VisibleCards.Clear();
            Table.DealersUpCard = null;
        }

        private void SettleBets()
        {
            foreach (var player in _players.Where(p => p.BankRoll >= Table.TableMinBet))
            {
                var whoWon = DetermineWinner(player.GetMainHandTotal());
                CreditDebitBets(whoWon, player);
                var splitHand = player.GeSplitHandTotal();
                if (splitHand != null)
                {
                    var splitBet = DetermineWinner(splitHand);
                    CreditDebitBets(splitBet, player);
                }
            }
        }

        private static void CreditDebitBets(Enums.PaymentFlow whoWon, Player player)
        {
            switch (whoWon)
            {
                case Enums.PaymentFlow.PayDealer:
                    player.IsAddingCurrentBetToBankroll(false);
                    break;
                case Enums.PaymentFlow.PayPlayer:
                    player.IsAddingCurrentBetToBankroll(true);
                    break;
                case Enums.PaymentFlow.PayBlackjack:
                    player.IsAddingCurrentBetToBankroll(true, true);
                    break;
                case Enums.PaymentFlow.Push:
                    break;
            }
        }

        private void HandleEachPlayer()
        {
            foreach (var player in _players)
            {
                var placeBetThisTurn = true;
                Enums.PlayAction playerDecision;
                do
                {
                    if (!player.IsCurrentHandBlackjack)
                    {
                        var card = _shoe.GiveMeSomeCards(1)[0];
                        Table.VisibleCards.Add(card);
                        playerDecision = player.Turn(card, placeBetThisTurn, Table.DealersUpCard);
                        placeBetThisTurn = false;
                        ShuffleShoeIfNeeded();
                    }
                    else
                    {
                        playerDecision = Enums.PlayAction.Stand;
                    }
                } while (playerDecision != Enums.PlayAction.Stand);
            }
        }

        private void DealInitialCards()
        {
            foreach (var player in _players)
            {
                var cards = _shoe.GiveMeSomeCards(2);
                player.SetInitialCards(cards);
                foreach (var card in cards)
                {
                    Table.VisibleCards.Add(card);
                }
            }
            _dealersCards.AddRange(_shoe.GiveMeSomeCards(2));
            Table.DealersUpCard = _dealersCards[0];
            ShuffleShoeIfNeeded();
        }

        private void ShuffleShoeIfNeeded()
        {
            if (_shoe.NeedsToBeShuffled)
            {
                _shoe.Shuffle();
                TheCount.CurrentCount = 0;
            }
        }

        private Enums.PaymentFlow DetermineWinner(PlayerHand playerHand)
        {
            if ((playerHand.IsBlackJack && _isCurrentDealerHandBlackJack) || 
                (playerHand.HandTotal == _dealerHandValue && !playerHand.IsBlackJack && _isCurrentDealerHandBlackJack) ||
                (playerHand.HandTotal > 21 && _dealerHandValue > 21))
            {
                return Enums.PaymentFlow.Push;
            }
            if (playerHand.IsBlackJack)
            {
                return Enums.PaymentFlow.PayBlackjack;
            }
            if (playerHand.HandTotal > 21 || 
                (playerHand.HandTotal < _dealerHandValue && _dealerHandValue <= 21) || 
                _isCurrentDealerHandBlackJack)
            {
                return Enums.PaymentFlow.PayDealer;
            }
            return Enums.PaymentFlow.PayPlayer;
        }

        private void DealerDealsToSelf()
        {
            _isCurrentDealerHandBlackJack = CardHelper.IsBlackJack(_dealersCards[0], _dealersCards[1]);
            if (!_isCurrentDealerHandBlackJack)
            {
                _dealerHandValue = BasicStrategy.DetermineHandValue(_dealersCards.Select(card => card.Value).ToArray()).Value;
                Table.VisibleCards.Add(Table.DealersUpCard);
                while (_dealerHandValue < 17)
                {
                    var card = _shoe.GiveMeSomeCards(1)[0];
                    Table.VisibleCards.Add(card);
                    _dealersCards.Add(card);
                    _dealerHandValue = BasicStrategy.DetermineHandValue(_dealersCards.Select(c => c.Value).ToArray()).Value;
                }
            }
        }
    }
}