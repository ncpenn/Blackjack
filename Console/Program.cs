using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackjack.Actors;
using Blackjack.Models;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var players = new List<Player>
            {
                new Player(5000, true),
                new Player(5000, false)
            };
            var dealer = new Dealer(players, 100000, 8, 5, 500, new Percent(25));
            dealer.PlayGame();


            //add "report" for final numbers of card counting
        }
    }
}
