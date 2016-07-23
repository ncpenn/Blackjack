using System.Collections.ObjectModel;

namespace Blackjack.Models
{
    public static class Table
    {
        public static uint DealersUpCard { get; set; }
        public static int TableMinBet { get; set; }
        public static int TableMaxBet { get; set; }
        public static ObservableCollection<uint> VisibleCards { get; }

        static Table()
        {
            VisibleCards = new ObservableCollection<uint>();
        }
    }
}
