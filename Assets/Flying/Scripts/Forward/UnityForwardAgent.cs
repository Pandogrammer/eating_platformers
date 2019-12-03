using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class UnityForwardAgent : Agent
{

    [SerializeField] public Rigidbody body;
    
    private BehaviorParameters behaviorParameters;
    private UnityForwardArea area;
    private ForwardAgent.ForwardAgentModel model => area.agent;
    public override void InitializeAgent()
    {
        area = GetComponentInParent<UnityForwardArea>();
        behaviorParameters = GetComponent<BehaviorParameters>();
    }
    
    public override void CollectObservations()
        {
            AddVectorObs(model.position);
            AddVectorObs(model.velocity);
            AddVectorObs(model.target);
            AddVectorObs(model.speed);
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
            var impulseForward = Mathf.Clamp(act[0], -1f, 1f);
            ImpulseForward(model.speed * impulseForward);
        }


        private void DiscreteActions(float[] act)
        {
            var impulseForward = Mathf.FloorToInt(act[0]);
            switch (impulseForward)
            {
                case 1:
                    ImpulseForward(model.speed * 1f);
                    break;
                case 2:
                    ImpulseForward(model.speed * -1f);
                    break;
            }
        }

        private void ImpulseForward(float speed)
        {
            body.AddForce(body.transform.forward * speed, ForceMode.VelocityChange);
        }

        public override float[] Heuristic()
        {
            var action = new float[behaviorParameters.brainParameters.vectorActionSize.Length];
            
            if (Keys.FORWARD)
            {
                action[0] = 1f;
            }
            
            if (Keys.BACKWARD)
            {
                action[0] = 2f;
            }

            return action;
        }

        internal static class Keys
        {
            public static bool FORWARD => Input.GetKey(KeyCode.W);
            public static bool BACKWARD => Input.GetKey(KeyCode.S);
        }
}