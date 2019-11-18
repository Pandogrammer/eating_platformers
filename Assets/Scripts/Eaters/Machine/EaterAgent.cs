using Games.Machine;
using MLAgents;
using UnityEngine;

namespace Eaters.Machine
{
    public class EaterAgent : Agent
    {
        private UnityEater eaterBody;

        public override void InitializeAgent()
        {
            eaterBody = gameObject.GetComponent<UnityEater>();
        }

        public override void AgentReset()
        {
            transform.parent.gameObject.transform.parent.GetComponent<ArenaArea>().ResetArea();
        }
        
        public override void AgentAction(float[] vectorAction, string textAction)
        {
            DiscreteActions(vectorAction);

            var happiness = eaterBody.eater.happiness;
            Rewards.RewardByHappiness(this, happiness);
        }


        public override void CollectObservations()
        {
            Happiness();
            InverseTransformDirection();
            FoodDetected();
            DetectedFoodPosition();
            DetectedFoodDistance();
            AgentPosition();
            AgentRotation();
            AgentVelocity();
        }

        private void AgentVelocity()
        {
            AddVectorObs(eaterBody.body.velocity);
        }

        private void AgentRotation()
        {
            AddVectorObs(eaterBody.bodyTransform.rotation);
        }

        private void AgentPosition()
        {
            AddVectorObs(eaterBody.bodyTransform.position);
        }

        private void FoodDetected()
        {
            AddVectorObs(eaterBody.detectedFood != null);
        }

        private void DetectedFoodDistance()
        {
            var distance = -1f;
            if (eaterBody.detectedFood != null)
            {
                var agentPosition = eaterBody.bodyTransform.position;
                var foodPosition = eaterBody.detectedFood.transform.position;
                distance = Vector3.Distance(agentPosition, foodPosition);
            }

            AddVectorObs(distance);
        }

        private void DetectedFoodPosition()
        {
            var position = Vector3.zero;
            if (eaterBody.detectedFood != null)
                position = eaterBody.detectedFood.transform.position;
            
            AddVectorObs(position);
        }

        private void InverseTransformDirection()
        {
            AddVectorObs(eaterBody.bodyTransform.InverseTransformDirection(eaterBody.body.velocity));
        }

        private void Happiness()
        {
            AddVectorObs(eaterBody.eater.happiness);
        }

        private void DiscreteActions(float[] act)
        {
            var action = Mathf.FloorToInt(act[0]);
            switch (action)
            {
                case 1:
                    eaterBody.Move(1f * 2f);
                    break;
                case 2:
                    eaterBody.Move(-1f * 2f);
                    break;
                case 3:
                    eaterBody.Rotate(1f);
                    break;
                case 4:
                    eaterBody.Rotate(-1f);
                    break;
                case 5:
                    eaterBody.TryToEatFood();
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

            if (Keys.EAT)
            {
                action[0] = 5f;
            }

            return action;
        }
    }

    internal static class Keys
    {
        public static bool UP => Input.GetKey(KeyCode.W);
        public static bool DOWN => Input.GetKey(KeyCode.S);
        public static bool LEFT => Input.GetKey(KeyCode.A);
        public static bool RIGHT => Input.GetKey(KeyCode.D);
        public static bool EAT => Input.GetKey(KeyCode.Space);
    }
}