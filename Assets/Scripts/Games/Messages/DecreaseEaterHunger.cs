using Eaters;

namespace Games.Messages
{
    public class DecreaseEaterHunger : GameParameters
    {
        public readonly int eaterHash;
        public readonly int value;

        public DecreaseEaterHunger(int eaterHash, int value)
        {
            this.eaterHash = eaterHash;
            this.value = value;
        }
    }
}