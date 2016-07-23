namespace Blackjack.Models
{
    public class HandValue
    {
        public uint Value { get; set; } = 0;
        public bool IsSplit { get; set; } = false;
        public bool IsSoft { get; set; } = false;
    }
}
