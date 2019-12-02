using MLAgents;
using UnityEngine;

namespace Flying.Scripts.Fly.Unity
{
    public class UnityFlyingAgent : Agent
    {
        [SerializeField] public Rigidbody body;

        private BehaviorParameters behaviorParameters;
        private UnityFlyingArea area;
        private FlyingAgentModel model => area.agent;
        public override void InitializeAgent()
        {
            area = GetComponentInParent<UnityFlyingArea>();
            behaviorParameters = GetComponent<BehaviorParameters>();
        }

        public override void CollectObservations()
        {
            AddVectorObs(model.position);
            AddVectorObs(model.rotation);
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
            var impulseUp = ScaleAction(act[0], 0, 1);
            ImpulseUp(model.speed * impulseUp);
            
            var impulseForward = Mathf.Clamp(act[1], -1f, 1f);
            ImpulseForward(model.speed * impulseForward);
            
            var rotate = Mathf.Clamp(act[2], -1f, 1f);
            Rotate(model.speed * 4 * rotate);
        }


        private void DiscreteActions(float[] act)
        {
            var impulseUp = Mathf.FloorToInt(act[0]);
            if (impulseUp == 1) ImpulseUp(model.speed);
            
            var impulseForward = Mathf.FloorToInt(act[1]);
            switch (impulseForward)
            {
                case 1:
                    ImpulseForward(model.speed * 1f);
                    break;
                case 2:
                    ImpulseForward(model.speed * -1f);
                    break;
            }
            
            var rotate = Mathf.FloorToInt(act[2]);
            switch (rotate)
            {
                case 1:
                    Rotate(model.speed * -1f);
                    break;
                case 2:
                    Rotate(model.speed * 1f);
                    break;
            }
        }

        private void ImpulseForward(float speed)
        {
            body.AddForce(body.transform.forward * speed, ForceMode.VelocityChange);
        }

        private void ImpulseUp(float speed)
        {
            body.AddForce(body.transform.up * speed, ForceMode.VelocityChange);
        }

        private void Rotate(float rotation)
        {
            body.transform.Rotate(body.transform.up, rotation);
        }

        public override float[] Heuristic()
        {
            var action = new float[behaviorParameters.brainParameters.vectorActionSize.Length];

            if (Keys.UP)
            {
                action[0] = 1f;
            }
            
            if (Keys.FORWARD)
            {
                action[1] = 1f;
            }
            
            if (Keys.BACKWARD)
            {
                action[1] = 2f;
            }
            
            if (Keys.LEFT)
            {
                action[2] = 1f;
            }
            
            if (Keys.RIGHT)
            {
                action[2] = 2f;
            }

            return action;
        }

        internal static class Keys
        {
            public static bool UP => Input.GetKey(KeyCode.Space);
            public static bool FORWARD => Input.GetKey(KeyCode.W);
            public static bool BACKWARD => Input.GetKey(KeyCode.S);
            public static bool LEFT => Input.GetKey(KeyCode.A);
            public static bool RIGHT => Input.GetKey(KeyCode.D);
        }
        
    }
}
