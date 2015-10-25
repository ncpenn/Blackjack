using System.Collections.Generic;

namespace Blackjack_v1
{
    class Program
    {
        static void Main(string[] args)
        {
            var player1 = new Player(50000, true, new Percent(100));
            var player2 = new Player(50000, false, new Percent(100));
            var players = new List<Player>
            {
                player1,
                player2
            };

            var dealer = new Dealer(players, 10000, 6, 5, new Percent(25));
            dealer.StartGame();

            
            //add "report" for final numbers of card counting
        }
    }
}
