namespace Blackjack.Models
{
    public class Percent
    {
        public readonly double PercentValue;

        public Percent(int percentAsWholeNumber)
        {
            if (percentAsWholeNumber > 100)
            {
                PercentValue = 1;
            }
            else if (percentAsWholeNumber < 0)
            {
                PercentValue = 0;
            }
            else
            {
                PercentValue = (double) percentAsWholeNumber / 100;
            }
        }
    }
}
