using Blackjack_v1.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Blackjack_v1
{
    public class Dealer
    {
        private Table table;
        private readonly int numberOfRounds;
        private List<DealtCard> dealersCards;

        public Dealer(List<Player> players, int numberOfRounds, int numberOfDecksToBeUsed, int tableMinBet, Percent whenToShuffle)
        {
            table = new Table(players, numberOfDecksToBeUsed, tableMinBet, whenToShuffle);
            this.numberOfRounds = numberOfRounds;
            dealersCards = new List<DealtCard>();
        }

        public void StartGame()
        {
            table.CardsVisibleThisRound.CollectionChanged += table.DealtCard_CollectionChanged;
            for (var round = 0; round < numberOfRounds; round++)
            {
                if (table.PlayerSlots.Count == 0) break;
                table.CardsVisibleThisRound.Clear();
                ResetStandingFlag();
                dealersCards.Clear();

                SetBets(table.TableMinBet, table.TheCount, false);

                var initialCards = GetIntialRoundOfCards().ToList();
                //GivePlayersTheirInitialCards(ref initialCards);
                GivePlayersTheirInitialCards(initialCards);
                GiveDealerCards(initialCards);
                SetVisbleCards();

                if (DealWithDealerBlackJack()) continue;
               
                PayOutBlackJacks();
                DealAdditionalCardsToPlayers();

                DealerHelper.DealerTakesCards(dealersCards, table);

                SettleBetsWithPlayers();
            }

            foreach (var playerSlot in table.PlayerSlots)
            {
                Console.WriteLine(playerSlot.Player.BankRoll);
                Console.WriteLine(playerSlot.Player.IsCardCounter);
            }
        }

        private void SettleBetsWithPlayers()
        {
            var listOfDealerCardValues = dealersCards.Select(card => (int)card.Value).ToList();
            var dealerTotal = BasicStrategy.DetermineHandValue(listOfDealerCardValues);
            foreach (var playerSeat in table.PlayerSlots)
            {
                var handTotal = BasicStrategy.DetermineHandValue(playerSeat.DealtCards.Select(card => (int)card.Value).ToList());
                AddOrSubtractBets(playerSeat, handTotal, dealerTotal, playerSeat.BetAmount);
                if (playerSeat.Split.Count > 0)
                {
                    var splitHandTotal = BasicStrategy.DetermineHandValue(playerSeat.Split.Select(card => (int)card.Value).ToList());
                    AddOrSubtractBets(playerSeat, splitHandTotal, dealerTotal, (int)playerSeat.BetAmountOnSplit);
                }
                playerSeat.DealtCards.Clear();
                playerSeat.Split.Clear();
            }
        }

        private void AddOrSubtractBets(PlayerSlot playerSeat, int handTotal, int dealerTotal, int betAmount)
        {          
            if (handTotal > 21)
            {
                playerSeat.Player.BankRoll -= betAmount;
            }
            else if (dealerTotal > 21)
            {
                playerSeat.Player.BankRoll += betAmount;
            }
            else
            {
                if (dealerTotal > handTotal)
                {
                    playerSeat.Player.BankRoll -= betAmount;
                }
                else if (dealerTotal < handTotal)
                {
                    playerSeat.Player.BankRoll += betAmount;
                }
            }
        }

        private bool DealWithDealerBlackJack()
        {
            if (DoesDealerHaveBlackjack())
            {
                foreach (var playerSeat in table.PlayerSlots)
                {
                    var card1 = playerSeat.DealtCards[0];
                    var card2 = playerSeat.DealtCards[1];
                    if (!IsBlackJack(card1, card2))
                    {
                        playerSeat.Player.BankRoll -= playerSeat.BetAmount;
                    }

                }
                return true;
            }
            return false;
        }

        private bool IsBlackJack(DealtCard card1, DealtCard card2)
        {
            if ((card1.Value == Enums.Value.Ace && (int)card2.Value >= 10) ||
                (card2.Value == Enums.Value.Ace && (int)card1.Value >= 10))
            {
                return true;
            }
            return false;
        }

        private bool DoesDealerHaveBlackjack()
        {
            if ((int)dealersCards.First(card => card.IsVisibleToPlayers == true).Value >= 10 ||
                dealersCards.First(card => card.IsVisibleToPlayers == true).Value == Enums.Value.Ace)
            {
                var card = dealersCards.FirstOrDefault(c => c.Value != Enums.Value.Ace);
                if (card != null && (int)card.Value >= 10)
                {
                    return true;
                }
            }
            return false;
        }

        private void DealAdditionalCardsToPlayers()
         {
            foreach (var playerSeat in table.PlayerSlots)
            {
                while (!playerSeat.IsStanding || !playerSeat.IsStandingOnSplit)
                {
                    var specialAction = string.Empty;
                    if (playerSeat.Player.IsRequestingCard(playerSeat.DealtCards, 
                        dealersCards.First(card => card.IsVisibleToPlayers == true), out specialAction, true))
                    {
                        if (specialAction != "split")
                        {
                            var card = table.Shoe.GiveMeSomeCards(1).First();
                            table.TotalNumberOfCardsDealt += 1;
                            var dealtCard = new DealtCard { IsVisibleToPlayers = true, Suit = card.Suit, Value = card.Value };
                            table.CardsVisibleThisRound.Add(dealtCard);
                            playerSeat.DealtCards.Add(dealtCard);
                            if (specialAction == "double")
                            {
                                playerSeat.IsStanding = true;
                                playerSeat.BetAmount *= 2;
                            }
                        }
                        else
                        {
                            SetUpSplit(playerSeat);
                            continue;
                        }
                    }
                    else
                    {
                        playerSeat.IsStanding = true;
                    }
                    //if (playerSeat.Split != null)
                    if(playerSeat.Split.Count > 0)
                    {
                        var specialAction2 = string.Empty;
                        if (playerSeat.Player.IsRequestingCard(playerSeat.Split,
                        dealersCards.First(card => card.IsVisibleToPlayers == true), out specialAction2, false))
                        {
                            var card = table.Shoe.GiveMeSomeCards(1).First();
                            table.TotalNumberOfCardsDealt += 1;
                            var dealtCard = new DealtCard { IsVisibleToPlayers = true, Suit = card.Suit, Value = card.Value };
                            table.CardsVisibleThisRound.Add(dealtCard);
                            playerSeat.Split.Add(dealtCard);
                            if (specialAction2 == "double")
                            {
                                playerSeat.IsStandingOnSplit = true;
                                playerSeat.BetAmount *= 2;
                            }
                        }
                        else
                        {
                            playerSeat.IsStandingOnSplit = true;
                        }
                    }
                }             
            }
        }

        private void SetUpSplit(PlayerSlot playerSeat)
        {
            playerSeat.DealtCards.RemoveAt(0);
            playerSeat.Split.Add(playerSeat.DealtCards.First());
            playerSeat.IsStandingOnSplit = false;
            playerSeat.LayDownBet(table.TableMinBet, table.TheCount, table.TotalNumberOfCardsDealt, true);
        }

        private void ResetStandingFlag()
        {
            foreach (var playerSeat in table.PlayerSlots)
            {
                playerSeat.IsStanding = false;

                //only set to false if a split condition happens
                playerSeat.IsStandingOnSplit = true;
            }
        }

        private void PayOutBlackJacks()
        {
            foreach (var playerSeat in table.PlayerSlots)
            {
                if (IsBlackJack(playerSeat.DealtCards[0], playerSeat.DealtCards[1]))
                {
                    var winnings = (decimal)(playerSeat.BetAmount * 1.5);
                    playerSeat.Player.BankRoll += winnings;
                    playerSeat.IsStanding = true;
                }
            }
        }

        private void SetBets(int minimumBet, int theCount, bool isForSplit)
        {
            if (table.PlayerSlots[0].ShouldCountBeReset(table.TotalNumberOfCardsDealt))
            {
                table.TheCount = 0;
            }
            var playersToRemove = new List<PlayerSlot>();
            foreach (var playerSeat in table.PlayerSlots)
            {                
                playerSeat.LayDownBet(minimumBet, theCount, table.TotalNumberOfCardsDealt, isForSplit);
                if (playerSeat.BetAmount == 0)
                {
                    playersToRemove.Add(playerSeat);
                }
            }
            foreach (var p in playersToRemove)
            {
                table.PlayerSlots.Remove(p);
            }
        }

        private void SetVisbleCards()
        {
            foreach (var playerSeat in table.PlayerSlots)
            {
                foreach (var card in playerSeat.DealtCards)
                {
                    table.CardsVisibleThisRound.Add(card);
                }
            }
            table.CardsVisibleThisRound.Add(dealersCards.Where(c => c.IsVisibleToPlayers == true).First());
        }

        private void GiveDealerCards(List<DealtCard> cards)
        {
            cards[0].IsVisibleToPlayers = false;
            dealersCards.AddRange(cards);
        }

        private void GivePlayersTheirInitialCards(List<DealtCard> cards)
        {
            foreach (var playerSpot in table.PlayerSlots)
            {
                var cardsForPlayer = cards.Take(2).ToList();
                foreach (var card in cardsForPlayer)
                {
                    cards.Remove(card);
                }
                playerSpot.DealtCards.AddRange(cardsForPlayer);
            }
        }

        private List<DealtCard> GetIntialRoundOfCards()
        {
            var cards = new List<DealtCard>();
            var numberOfPlayers = table.PlayerSlots.Count;
            
            //two extra cards for the dealer
            var numberOfCardsNeeded = numberOfPlayers * 2 + 2;
            var cardsReturned = table.Shoe.GiveMeSomeCards(numberOfCardsNeeded);
            table.TotalNumberOfCardsDealt += numberOfCardsNeeded;
            foreach (var card in cardsReturned)
            {
                cards.Add(new DealtCard { IsVisibleToPlayers = true, Suit = card.Suit, Value = card.Value});
            }
            return cards;
        }
    }
}
