using System.Linq;
using Eaters;
using Games;
using Games.Machine;
using MLAgents;
using UnityEngine;

namespace Chefs.Machine
{
    public class ChefAgent : Agent
    {
        private UnityChef chefBody;

        public override void InitializeAgent()
        {
            chefBody = gameObject.GetComponent<UnityChef>();
        }


        public override void AgentAction(float[] vectorAction, string textAction)
        {
            DiscreteActions(vectorAction);

            var happiness = chefBody.chef.happiness / 2;
            Rewards.RewardByHappiness(this, happiness);
        }

        public override void CollectObservations()
        {
            Happiness();
            InverseTransformDirection();
            FiveClosestEatersHappinessAndPosition();
            ChefPosition();
        }

        private void ChefPosition()
        {
            AddVectorObs(chefBody.transform.position);
        }

        private void FiveClosestEatersHappinessAndPosition()
        {
            int amount = 5;
            var eaters = transform.parent.gameObject.transform.parent
                .GetComponentsInChildren<UnityEater>()
                .OrderBy(x => DistanceToChef(x.transform.position))
                .ToArray();
            
            var eatersHappiness = new int[amount];
            var eatersPositions = new Vector3[amount];
            
            for (var i = 0; i < eaters.Length && i < amount; i++)
            {
                eatersHappiness[i] = eaters[i].eater.happiness;
                eatersPositions[i] = eaters[i].transform.position;
            }

            for (int i = 0; i < amount; i++)
            {
                AddVectorObs(eatersHappiness[i]);
                AddVectorObs(eatersPositions[i]);
            }
        }

        private float DistanceToChef(Vector3 eaterPosition)
        {
            return Vector3.Distance(chefBody.transform.position, eaterPosition);
        }

        private void InverseTransformDirection()
        {
            AddVectorObs(transform.InverseTransformDirection(chefBody.GetComponentInParent<Rigidbody>().velocity));
        }

        private void Happiness()
        {
            AddVectorObs(chefBody.chef.happiness);
        }

        private void DiscreteActions(float[] act)
        {
            var action = Mathf.FloorToInt(act[0]);
            switch (action)
            {
                case 1:
                    chefBody.Move(1f * 2f);
                    break;
                case 2:
                    chefBody.Move(-1f * 2f);
                    break;
                case 3:
                    chefBody.Rotate(1f);
                    break;
                case 4:
                    chefBody.Rotate(-1f);
                    break;
                case 5:
                    chefBody.DropFood();
                    break;
            }
        }
        public override float[] Heuristic()
        {
            var action = new float[1];
            if (Keys.UP)
            {
                action[0] = 1f;
            }

            if (Keys.DOWN)
            {
                action[0] = 2f;
            }

            if (Keys.RIGHT)
            {
                action[0] = 3f;
            }

            if (Keys.LEFT)
            {
                action[0] = 4f;
            }

            if (Keys.DROP_FOOD)
            {
                action[0] = 5f;
            }
            
            return action;
        }
        
        
    }

    internal static class Keys
    {
        public static bool UP => Input.GetKey(KeyCode.UpArrow);
        public static bool DOWN => Input.GetKey(KeyCode.DownArrow);
        public static bool LEFT => Input.GetKey(KeyCode.LeftArrow);
        public static bool RIGHT => Input.GetKey(KeyCode.RightArrow);
        public static bool DROP_FOOD => Input.GetKey(KeyCode.Keypad0);
    }
}