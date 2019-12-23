using System;
using System.Collections.Generic;
using System.Linq;
using GenericScripts.Stepable;
using UnityEngine;

namespace Nolose.Scripts
{
    public class CollectingArea : UnityStepableArea
    {
        private List<CollectorAgent> agentCollectors;
        private CollectorFoodSpawner foodSpawner;
        private List<GameObject> spawnedFood;
        private float collectorInitialHp;

        private void Start()
        {
            var agentCollectors = GetComponentsInChildren<CollectorAgent>().ToList();
            var foodSpawner = GetComponentInChildren<CollectorFoodSpawner>();
            Setup(agentCollectors, foodSpawner);
        }

        public void Setup(List<CollectorAgent> agentCollectors, CollectorFoodSpawner foodSpawner)
        {
            collectorInitialHp = 10F;
            spawnedFood = new List<GameObject>();
            this.agentCollectors = agentCollectors;
            this.foodSpawner = foodSpawner;
        }

        public override void Step(int step, int maxSteps)
        {
            ProcessCollectors(maxSteps);
            TryToSpawnFood(step);
        }

        public override void Reset()
        {
            DestroyFood();
            SetCollectorsAsDone();
            ResetCollectors();
        }

        private void TryToSpawnFood(int step)
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
//                if (agent.IsDone()) continue;
//                var reward = CollectorRewardFunction.Calculate(agent, maxSteps);
//                if(Math.Abs(reward) > 0.0001f) agent.AddReward(reward);
                HpDecay(agent, maxSteps);
            }
        }

        public static void ProcessMe(CollectorAgent agent)
        {
            var reward = CollectorRewardFunction.Calculate(agent, 3000);
            if(Math.Abs(reward) > 0.0001f) agent.AddReward(reward);
        }

        private void HpDecay(CollectorAgent collectorAgent, int maxSteps)
        {
            if (collectorAgent.Collector.IsDead) 
                return;
            
            collectorAgent.Collector.hp -= collectorInitialHp * 2 / maxSteps;
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
                foodSpawner.UnspawnFood(food);
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