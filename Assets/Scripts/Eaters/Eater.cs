using System;
using Eaters.Messages;

namespace Eaters
{
    public class Eater
    {
        public readonly int hunger;
        public readonly EaterState state;
        public int happiness => -hunger;

        public Eater(int hunger)
        {
            this.hunger = hunger;
            this.state = CalculateState(hunger);
        }

        private EaterState CalculateState(int hunger)
        {
            if (hunger > 0) return EaterState.Sad;
            else return EaterState.Happy;
        }

        public static Eater Update(EaterMsg msg, Eater eater, EaterParameters parameters = null)
        {
            switch (msg)
            {
                case EaterMsg.IncrementHunger:
                    return IncrementHunger(eater);
                case EaterMsg.DecreaseHunger:
                    return DecreaseHunger(eater, (DecreaseHunger) parameters);
                default:
                    throw new ArgumentOutOfRangeException(nameof(msg), msg, null);
            }
        }

        private static Eater DecreaseHunger(Eater eater, DecreaseHunger parameters)
        {
            return new Eater(hunger: eater.hunger - parameters.value);
        }

        private static Eater IncrementHunger(Eater eater)
        {
            return new Eater(hunger: eater.hunger + 1);
        }
        
    }
}