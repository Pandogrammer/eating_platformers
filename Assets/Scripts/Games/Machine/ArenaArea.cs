using System;
using System.Collections.Generic;
using System.Linq;
using Foods;
using MLAgents;
using UnityEngine;

namespace Games.Machine
{
    public class ArenaArea : Area
    {
        private UnityGame game;
        private FoodSpawner spawner;

        public void Awake()
        {
            game = GetComponentInChildren<UnityGame>();
            spawner = GetComponentInChildren<FoodSpawner>();
        }

        public void Reset()
        {
            var agents = GetComponentsInChildren<Agent>().ToList();
            agents.ForEach(x =>
            {
                x.Done();
                x.transform.parent.transform.localPosition = new Vector3(0, 3, 0);
            });

            var food = GetComponentsInChildren<UnityFood>().ToList();
            food.ForEach(x => Destroy(x.gameObject));
            
            game.Restart();
        }

        public override void ResetArea()
        {
            Reset();
        }

        public void Update()
        {
            if (game.game.tick > 10)
            {
                ResetArea();
            }
        }

        public void Setup(float areaSize, float spawnerCooldown)
        {
            spawner.Setup(areaSize, spawnerCooldown);
        }
    }

    public class Rewards
    {
        private static int limit = 10;
        private static float littleReward = 0.0001f;
        private static float bigReward = 1f;

        public static void RewardByHappiness(Agent agent, int happiness)
        {
            agent.AddReward(-0.00001f);
            var multiplier = happiness > 0 ? 1 : -1;
            if (Math.Abs(happiness) < 10)
                agent.AddReward(littleReward * multiplier);
            else
            {
                agent.SetReward(bigReward * multiplier);
                agent.Done();
            }
        }
    }
}