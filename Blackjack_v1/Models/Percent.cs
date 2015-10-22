namespace Blackjack_v1
{
    public class Percent
    {
        public readonly int PercentValue;

        public Percent(int percent)
        {
            if (percent > 100)
            {
                PercentValue = 100;
            }
            else if (percent < 0)
            {
                PercentValue = 0;
            }
            else
            {
                PercentValue = percent;
            }
        }
    }
}
