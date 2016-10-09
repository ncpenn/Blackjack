using System.Collections.Generic;
using Blackjack.Actors.Interfaces;
using Blackjack.Models;
using System.Linq;
using Blackjack.Rules.BasicStrategy.Interfaces;
using Blackjack.Rules.BasicStrategy;
using Blackjack.Rules.CardCounting.Base.Interfaces;
using BlackJack.Rules.CardCounting.AceFive;

namespace Blackjack.Actors
{
    public class Player : IPlayer
    {
        private ICardCount _cardCountingRules;
        private IBasicStrategy _basicStrategy;
        private Bankroll _bankroll;
        private bool _isCardCounter;

        public Player(uint bankroll, bool isCardCounter)
        {
            _cardCountingRules = new AceFiveCountingRules();
            _basicStrategy = new BasicStrategy();
            Initialize(bankroll, isCardCounter);
        }

        public Player(uint bankroll, bool isCardCounter, IBasicStrategy basicStrategyMock, ICardCount cardCountingRulesMock)
        {
            _cardCountingRules = cardCountingRulesMock;
            _basicStrategy = basicStrategyMock;
            Initialize(bankroll, isCardCounter);
        }

        public HandInformation[] Hands { get; private set; }

        public IEnumerable<uint> PlayHand(IShoe shoe, uint dealerUpCard, List<uint> visibleCards, uint tableMinBet, uint tableMaxBet)
        {
            var cardsNowVisible = new List<uint>();
            foreach (var hand in Hands.Where(h => h.Cards.Any()))
            {
                hand.SetBet(DetermineBetSize(visibleCards, tableMinBet, tableMaxBet));
                var playAction = _basicStrategy.DetermineCorrectPlayAction(hand, dealerUpCard);
                while (playAction != Enums.PlayAction.Stand)
                {
                    if (playAction == Enums.PlayAction.Split)
                    {
                        if (Hands.Where(h => h.Cards.Any()).Count() == 1)
                        {
                            Hands[1].CanSplit = false;
                            Hands[1].Cards.Add(hand.Cards[1]);
                            hand.Cards.RemoveAt(1);
                            Hands[1].SetBet(DetermineBetSize(visibleCards, tableMinBet, tableMaxBet));
                        }
                    }
                    else if(playAction == Enums.PlayAction.Double)
                    {
                        if (hand.GetCurrentBet() * 2 <= _bankroll.Amount)
                        {
                            hand.InformHandThatItsDouble();
                        }
                    }

                    var card = shoe.CardRequest();
                    cardsNowVisible.Add(card);
                    hand.Cards.Add(card);

                    playAction = _basicStrategy.DetermineCorrectPlayAction(hand, dealerUpCard);
                } 
            }
            return cardsNowVisible;
        }

        private uint DetermineBetSize(List<uint> visibleCards, uint tableMinBet, uint tableMaxBet)
        {
            if (_isCardCounter)
            {
                var currentCount = _cardCountingRules.CountUsingCardsFromThisHand(visibleCards);
                var recommendedBet = _cardCountingRules.BetSizeBasedOnCount(currentCount, tableMinBet, tableMaxBet);
                if (_bankroll.Amount <= recommendedBet)
                {
                    return recommendedBet;
                }
                else if (_bankroll.Amount <= tableMinBet)
                {
                    return tableMinBet;
                }
                return 0;
            }
            else
            {
                if (_bankroll.Amount >= tableMinBet)
                {
                    return tableMinBet;
                }
                return 0;
            }
        }

        public void SetNewHand(uint[] cards)
        {
            Hands[0] = new HandInformation();
            Hands[1] = new HandInformation();
            Hands[0].Cards.AddRange(cards);
        }

        public void Settle(HandInformation dealerHand)
        {
            foreach (var hand in Hands.Where(h => h.Cards.Any()))
            {
                var bothBlackjack = dealerHand.IsBlackjack && hand.IsBlackjack;
                var playerBlackjack = hand.IsBlackjack;
                var dealerHandGreater = hand.HandValue() < dealerHand.HandValue() && !dealerHand.IsBusted || hand.IsBusted && !dealerHand.IsBusted;
                var playerHandGreater = hand.HandValue() > dealerHand.HandValue() && !hand.IsBusted || dealerHand.IsBusted && !hand.IsBusted;

                if (bothBlackjack)
                {
                    continue;
                }
                else if (playerBlackjack)
                {
                    hand.PayBlackjack(_bankroll);
                }
                else if (playerHandGreater)
                {
                    hand.BetAmountToPlayer(_bankroll);
                }
                else if (dealerHandGreater)
                {
                    hand.BetAmountToDealer(_bankroll);
                }
            }
        }

        private void Initialize(uint startingBankroll, bool isCardCounter)
        {
            _bankroll = new Bankroll();
            _bankroll.Amount = (int)startingBankroll;
            _isCardCounter = isCardCounter;
            Hands = new HandInformation[2];
        }
    }
}
