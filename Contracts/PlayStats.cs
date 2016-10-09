using System.Collections.Generic;

namespace Blackjack.Contracts
{
    public class PlayStats
    {
        public PlayStats()
        {
            PlayersSnapshot = new List<string>();
        }

        public List<string> PlayersSnapshot { get; }

        public bool WasCancelledByUser { get; private set; }

        public void IsCancelledByUser(bool isUserCancelled)
        {
            WasCancelledByUser = isUserCancelled;
        }
    }
}
