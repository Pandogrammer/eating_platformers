using System;
using MLAgents;
using UnityEngine;

namespace Flying.Scripts.Rotate
{
    public class UnityRotateAgent : Agent
    {
        [SerializeField] public Rigidbody body;
    
        private BehaviorParameters behaviorParameters;
        private UnityRotateArea area;
        public RotateAgent.RotateAgentModel model;
        public override void InitializeAgent()
        {
            behaviorParameters = GetComponent<BehaviorParameters>();
        }

        public override void CollectObservations()
        {
            AddVectorObs(model.rotation);
            AddVectorObs(model.velocity);
            AddVectorObs(model.target);
            AddVectorObs(model.rotationAngle);
            AddVectorObs(model.turnDirection);
        }

        public override void AgentAction(float[] vectorAction, string textAction)
        {
            if (behaviorParameters.brainParameters.vectorActionSpaceType == SpaceType.Discrete)
                DiscreteActions(vectorAction);
            else
                ContinuousActions(vectorAction);
        }

        private void ContinuousActions(float[] act)
        {
            var rotate = Mathf.Clamp(act[0], -1f, 1f);
            Rotate(rotate);
        }


        private void DiscreteActions(float[] act)
        {
            var rotate = Mathf.FloorToInt(act[0]);
            switch (rotate)
            {
                case 0:
                    break;
                case 1:
                    Rotate(-1f);
                    break;
                case 2:
                    Rotate(1f);
                    break;
            }
        }

        private void Rotate(float rotation)
        {
            body.transform.Rotate(body.transform.up, model.rotationAngle * rotation);
        }

        public override float[] Heuristic()
        {
            var action = new float[behaviorParameters.brainParameters.vectorActionSize.Length];
            
            if (Keys.LEFT)
            {
                action[0] = -1f;
            }
            if (Keys.RIGHT)
            {
                action[0] = 1f;
            }

            return action;
        }

        internal static class Keys
        {
            public static bool LEFT => Input.GetKey(KeyCode.A);
            public static bool RIGHT => Input.GetKey(KeyCode.D);
        }
    }
}