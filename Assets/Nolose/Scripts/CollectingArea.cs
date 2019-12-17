using System.Collections.Generic;
using System.Linq;
using GenericScripts.Stepable;
using UnityEngine;

namespace Nolose.Scripts
{
    public class CollectingArea : UnityStepableArea
    {
        private List<AgentCollector> agentCollectors;
        private CollectorFoodSpawner foodSpawner;
        private List<GameObject> spawnedFood;

        public int step { get; private set; }

        private void Start()
        {
            var agentCollectors = FindObjectsOfType<AgentCollector>().ToList();
            var foodSpawner = GetComponentInChildren<CollectorFoodSpawner>();
            Setup(agentCollectors, foodSpawner);
        }

        public void Setup(List<AgentCollector> agentCollectors, CollectorFoodSpawner foodSpawner)
        {
            step = 0;
            spawnedFood = new List<GameObject>();
            this.agentCollectors = agentCollectors;
            this.foodSpawner = foodSpawner;
        }

        public override void Step(int maxSteps)
        {
            ProcessCollectors(maxSteps);
            StepAdvance(maxSteps);
        }

        private void ProcessCollectors(int maxSteps)
        {
            foreach (var agent in agentCollectors)
            {
                StepReward(maxSteps, agent);
                EatReward(agent);
                HpDecay(agent);
                DeathReward(agent);
            }
        }

        private void StepAdvance(int maxSteps)
        {
            step++;
            if (step > maxSteps) Reset();
        }

        private static void HpDecay(AgentCollector agent)
        {
            if (agent.Collector.IsDead) return;
            agent.Collector.hp -= 0.0002f;
        }

        private static void EatReward(AgentCollector agent)
        {
            if (!agent.Collector.justEeaten) return;
            agent.AddReward(0.1F);
            agent.Collector.justEeaten = false;
        }

        private static void DeathReward(AgentCollector agent)
        {
            if (!agent.Collector.IsDead) return;
            agent.SetReward(-2F);
        }

        private static void StepReward(int maxSteps, AgentCollector agent)
        {
            if (agent.Collector.IsDead) return;
            agent.AddReward(-1F / maxSteps);
        }

        private void Reset()
        {
            step = 0;
            DestroyFood();
            ResetCollectors();
        }

        private void DestroyFood()
        {
            foreach (var food in spawnedFood)
            {
                Destroy(food);
            }
        }

        private void ResetCollectors()
        {
            foreach (var collector in agentCollectors)
            {
                collector.Done();
                collector.Collector.hp = 10;
            }
        }
    }
}