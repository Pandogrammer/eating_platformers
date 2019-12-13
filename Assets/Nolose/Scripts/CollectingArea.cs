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
            foreach (var agent in agentCollectors.Where(agent => !agent.IsDone()))
            {
                //step
                agent.AddReward(-1F / maxSteps);
                //dead
                if (agent.Collector.IsDead)
                {
                    agent.SetReward(-2F);
                    agent.Done();
                }

                //eaten
                if (agent.Collector.justEeaten)
                {
                    agent.AddReward(0.1F);
                    agent.Collector.justEeaten = false;
                }
            
                agent.Collector.hp -= 0.0002f;
            }
            
            step++;
            if (step > maxSteps) Reset();
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
            foreach (var agent in agentCollectors)
            {
                agent.Done();
                agent.Collector.hp = 10;
                agent.AgentReset();
            }
        }
    }
}