using System;
using System.Linq;
using Blackjack.Interfaces;
using Blackjack.Models;

namespace Blackjack.Actors.Actions
{
    public class Turn
    {
        public Action<PlayCollection, IPlayer> Play()
        {
            return (collection, player) =>
            {
                if (!player.GetMainHandTotal().IsBlackJack)
                {
                    var playAction = collection.BasicStrategu.DeterminePlayerNextPlay(player.MainHand, collection.Dealer.DealerUpCard,
                            !player.SplitHand.Any());
                    if (playAction == Enums.PlayAction.Split)
                    {
                        var card = player.MainHand[0];
                        player.SplitHand.Add(card);
                        player.MainHand.RemoveAt(0);
                        playAction = Enums.PlayAction.Hit;
                    }
                    while (playAction != Enums.PlayAction.Stand)
                    {
                        var card = collection.Shoe.GiveMeSomeCards(1)[0];
                        player.MainHand.Add(card);
                        playAction = collection.BasicStrategu.DeterminePlayerNextPlay(player.MainHand, collection.Dealer.DealerUpCard,
                            !player.SplitHand.Any());
                    }
                    playAction = Enums.PlayAction.Hit;
                    while (playAction != Enums.PlayAction.Stand)
                    {
                        var card = collection.Shoe.GiveMeSomeCards(1)[0];
                        player.SplitHand.Add(card);
                        playAction = collection.BasicStrategu.DeterminePlayerNextPlay(player.SplitHand, collection.Dealer.DealerUpCard,
                            !player.SplitHand.Any());
                    }
                }
            };
        }
    }
}
