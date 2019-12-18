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
        private float collectorInitialHp;

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
            collectorInitialHp = 10F;
            spawnedFood = new List<GameObject>();
            this.agentCollectors = agentCollectors;
            this.foodSpawner = foodSpawner;
        }

        public override void Step(int maxSteps)
        {
            ProcessCollectors(maxSteps);
            StepAdvance(maxSteps);
            TryToSpawnFood();
        }

        private void TryToSpawnFood()
        {
            if (step % 300 == 0)
            {
                spawnedFood.Add(foodSpawner.SpawnFood());
            }
        }

        private void ProcessCollectors(int maxSteps)
        {
            foreach (var agent in agentCollectors)
            {
                StepReward(maxSteps, agent);
                EatReward(agent);
                HpDecay(agent, maxSteps);
                DeathReward(agent);
            }
        }

        private void StepAdvance(int maxSteps)
        {
            step++;
            if (step > maxSteps) Reset();
        }

        private void HpDecay(AgentCollector agent, int maxSteps)
        {
            if (agent.Collector.IsDead) return;
            agent.Collector.hp -= collectorInitialHp * 2 / maxSteps;
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
            SetCollectorsAsDone();
            ResetCollectors();
        }

        private void SetCollectorsAsDone()
        {
            foreach (var collector in agentCollectors)
            {
                collector.Done();
            }
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
                collector.Collector.hp = collectorInitialHp;
            }
        }
    }
}