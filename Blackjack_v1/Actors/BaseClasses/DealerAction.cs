using System;
using Blackjack.Interfaces;
using Blackjack.Models;

namespace Blackjack.Actors.BaseClasses
{
    public class DealerAction
    {
        public void Do(Action<PlayCollection, IPlayer> func, PlayCollection collection)
        {
            foreach (var player in collection.Players)
            {
                func.Invoke(collection, player);
            }
        }
    }
}
