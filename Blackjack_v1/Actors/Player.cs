using System;
using System.Collections.Generic;
using System.Linq;
using Blackjack.Helpers;
using Blackjack.Models;

namespace Blackjack.Actors
{
    public class Player
    {
        private readonly bool _isCardCounter;
        public bool IsCurrentHandBlackjack { get; private set; }
        public decimal BankRoll { get; private set; }
        private decimal _currentBet;
        private decimal _splitBet;
        private readonly List<Card> _splitHand;
        private bool _isStandingOnCurrentHand;
        private readonly List<Card> _currentHand;

        public Player(int startingBankRoll, bool isCardCounter)
        {
            BankRoll = startingBankRoll;
            _isCardCounter = isCardCounter;
            _currentHand = new List<Card>();
            _splitHand = new List<Card>();
        }

        public int PlaceBet(int minimumBet, int maxBet)
        {
            if (_isCardCounter)
            { 
                if (TheCount.CurrentCount <= 1 && BankRoll >= minimumBet) return minimumBet;
                var bet = CardCounterFigureBetSize(minimumBet, maxBet, TheCount.CurrentCount);
                if (BankRoll >= bet) return bet;
                if (BankRoll >= minimumBet) return minimumBet;
                return 0;
            }
            if (BankRoll >= minimumBet)
            {
                return minimumBet;
            }
            return 0;
        }

        public void IsAddingCurrentBetToBankroll(bool isAddingCurrentBet, bool isBlackjack = false)
        {
            if (isAddingCurrentBet)
            {
                BankRoll = isBlackjack ? BankRoll + (_currentBet * (decimal)1.5): BankRoll + _currentBet;
            }
            else
            {
                BankRoll -= _currentBet;
            }
        }

        public Enums.PlayAction Turn(Card card, bool placeBetThisTurn, Card dealersVisibleCard)
        {
            if (placeBetThisTurn)
            {
                _currentHand.Clear();
                _splitHand.Clear();
                _isStandingOnCurrentHand = false;
                _currentBet = PlaceBet(Table.TableMinBet, Table.TableMaxBet);
            }
            var isSplit = _isStandingOnCurrentHand && _splitHand.Any();
            if (isSplit)
            {
                _splitHand.Add(card);
                _splitBet = PlaceBet(Table.TableMinBet, Table.TableMaxBet);
            }
            else
            {
                _currentHand.Add(card);
            }
            var playAction = isSplit
                ? BasicStrategy.DeterminePlayerNextPlay(_splitHand.Select(c => c.Value).ToArray(), dealersVisibleCard.Value, !_splitHand.Any())
                : BasicStrategy.DeterminePlayerNextPlay(_currentHand.Select(c => c.Value).ToArray(), dealersVisibleCard.Value, false);
            if (playAction == Enums.PlayAction.Split)
            {
                _splitHand.Add(_currentHand[1]);
                _currentHand.RemoveAt(1);
            }
            if (playAction == Enums.PlayAction.Double)
            {
                if (isSplit)
                {
                    _splitBet = BankRoll - _currentBet * 2 + _splitBet * 2 >= 0 ? _splitBet * 2 : _splitBet + BankRoll;
                }
                else
                {
                    _currentBet = BankRoll - _currentBet * 2 >= 0 ? _currentBet * 2 : _currentBet + BankRoll;
                }  
                playAction = Enums.PlayAction.Stand;
            }
            if (playAction == Enums.PlayAction.Stand && _splitHand.Count == 1)
            {
                _isStandingOnCurrentHand = true;
                playAction = Enums.PlayAction.Hit;
            }
            return playAction;
        }

        public void SetInitialCards(Card[] cards)
        {
            _currentHand.Clear();
            _currentHand.AddRange(cards);
            IsCurrentHandBlackjack = CardHelper.IsBlackJack(cards[0], cards[1]);
        }

        public PlayerHand GetMainHandTotal()
        {
            return new PlayerHand
            {
                HandTotal = BasicStrategy.DetermineHandValue(_currentHand.Select(c => c.Value).ToArray()).Value,
                IsBlackJack = IsCurrentHandBlackjack
            };
        }

        public PlayerHand GeSplitHandTotal()
        {
            if (_splitHand.Any())
            {
                return new PlayerHand
                {
                    HandTotal = BasicStrategy.DetermineHandValue(_splitHand.Select(c => c.Value).ToArray()).Value,
                    IsBlackJack = IsCurrentHandBlackjack
                };
            }
            return null;
        }

        private int CardCounterFigureBetSize(int minimumBet, int maxBet, int theCount)
        {
            var bet = Math.Pow(minimumBet, theCount);
            return (int)(bet <= maxBet ? bet : maxBet);
        }
    }
}
