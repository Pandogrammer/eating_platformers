using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class UnityUpwardAgent : Agent
{

    [SerializeField] public Rigidbody body;
    
    private BehaviorParameters behaviorParameters;
    private UnityUpwardArea area;
    private UpwardAgent.UpwardAgentModel model => area.agent;
    public override void InitializeAgent()
    {
        area = GetComponentInParent<UnityUpwardArea>();
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
            var impulseUpward = Mathf.Clamp(act[0], 0, 1f);
            ImpulseUpward(model.speed * impulseUpward);
        }


        private void DiscreteActions(float[] act)
        {
            var impulseUpward = Mathf.FloorToInt(act[0]);
            switch (impulseUpward)
            {
                case 1:
                    ImpulseUpward(model.speed * 1f);
                    break;
            }
        }

        private void ImpulseUpward(float speed)
        {
            body.AddForce(body.transform.up * speed, ForceMode.VelocityChange);
        }

        public override float[] Heuristic()
        {
            var action = new float[behaviorParameters.brainParameters.vectorActionSize.Length];
            
            if (Keys.UPWARD)
            {
                action[0] = 1f;
            }

            return action;
        }

        internal static class Keys
        {
            public static bool UPWARD => Input.GetKey(KeyCode.Space);
        }
}