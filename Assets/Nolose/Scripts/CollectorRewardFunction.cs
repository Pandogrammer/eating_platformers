namespace Nolose.Scripts
{
    public class CollectorRewardFunction
    {
        public static float Calculate(CollectorAgent collectorAgent, int maxSteps)
        {
            return (DeathReward(collectorAgent)
                    + StepReward(collectorAgent)
                    + HpReward(collectorAgent)
                   ) / maxSteps;
        }

        private static float DeathReward(CollectorAgent collectorAgent)
        {
            if (!collectorAgent.Collector.IsDead)
                return 0f;

            return -5F;
        }

        private static float StepReward(CollectorAgent collectorAgent)
        {
            if (collectorAgent.Collector.IsDead)
                return 0f;

            return -1F;
        }

        private static float HpReward(CollectorAgent collectorAgent)
        {
            if (collectorAgent.Collector.IsDead)
                return 0f;

            var hp = collectorAgent.Collector.hp;
            return hp / 10;
        }
    }
}