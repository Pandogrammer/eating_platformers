using System;

namespace Chefs
{
    public class Chef
    {
        public readonly int happiness;
        public readonly ChefState state;

        public Chef(int happiness)
        {
            this.happiness = happiness;
            this.state = CalculateState(happiness);
        }

        private ChefState CalculateState(int happiness)
        {
            if (happiness > 0) return ChefState.Happy;
            else return ChefState.Sad;
        }

        public static Chef Update(ChefMsg msg, Chef chef)
        {
            switch (msg)
            {
                case ChefMsg.IncreaseHappiness:
                    return IncreaseHappiness(chef);
                case ChefMsg.DecreaseHappiness:
                    return DecreaseHappiness(chef);
                default:
                    throw new ArgumentOutOfRangeException(nameof(msg), msg, null);
            }
        }

        private static Chef IncreaseHappiness(Chef chef)
        {
            return new Chef(happiness: chef.happiness + 1);
        }

        private static Chef DecreaseHappiness(Chef chef)
        {
            return new Chef(happiness: chef.happiness - 1);
        }
    }
}