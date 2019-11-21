namespace Eaters.Messages
{
    public class DecreaseHunger : EaterParameters
    {
        public readonly int value;
        public DecreaseHunger(int value)
        {
            this.value = value;
        }
    }
}