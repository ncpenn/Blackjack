using System;
using System.Linq;
using Blackjack.Interfaces;
using Blackjack.Models;

namespace Blackjack.Actors.Actions
{
    public class Bet
    {
        public Action<PlayCollection, IPlayer> SetMainBet()
        {
            return (collection, player) =>
            {
                uint actualBet = 0;
                if (player.IsCardCounter)
                {
                    if (collection.Table.CurrentCount <= 1 && player.BankRoll >= collection.Table.TableMinBet)
                        actualBet = collection.Table.TableMinBet;
                    var bet = collection.BetHelper.CardCounterFigureBetSize(collection.Table.TableMinBet, collection.Table.TableMaxBet, collection.Table.CurrentCount);
                    if (player.BankRoll >= bet) actualBet = bet;
                    if (player.BankRoll >= collection.Table.TableMinBet) actualBet = collection.Table.TableMinBet;
                }
                if (player.BankRoll >= collection.Table.TableMinBet)
                {
                    actualBet = collection.Table.TableMinBet;
                }
                player.SetMainBet(actualBet);
            };
        }

        public Action<PlayCollection, IPlayer> SetSplitBet()
        {
            return (collection, player) =>
            {
                uint actualBet = 0;
                if (player.SplitHand.Any())
                {
                    if (player.IsCardCounter)
                    {
                        if (collection.Table.CurrentCount <= 1 && player.BankRoll >= collection.Table.TableMinBet + player.MainBet)
                            actualBet = collection.Table.TableMinBet;
                        var bet = collection.BetHelper.CardCounterFigureBetSize(collection.Table.TableMinBet, collection.Table.TableMaxBet, collection.Table.CurrentCount);
                        if (player.BankRoll >= bet + player.MainBet) actualBet = bet;
                        if (player.BankRoll >= collection.Table.TableMinBet + player.MainBet) actualBet = collection.Table.TableMinBet;
                    }
                    if (player.BankRoll >= collection.Table.TableMinBet + player.MainBet)
                    {
                        actualBet = collection.Table.TableMinBet;
                    }
                    player.SetSplitBet(actualBet);
                }
            };
        }

        public Action<PlayCollection, IPlayer> Settle()
        {
            return (collection, player) =>
            {
                var whoWon = collection.PaymentHelper.DetermineWinner(player.GetMainHandTotal(), collection.Dealer);
                collection.PaymentHelper.CreditDebitBets(whoWon, player);
                var splitHand = player.GeSplitHandTotal();
                if (splitHand != null)
                {
                    var splitBet = collection.PaymentHelper.DetermineWinner(splitHand, collection.Dealer);
                    collection.PaymentHelper.CreditDebitBets(splitBet, player);
                }
            };
        }
    }
}