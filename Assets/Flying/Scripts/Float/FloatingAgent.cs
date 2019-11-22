using MLAgents;
using UnityEngine;

namespace Flying.Scripts.Float
{
    public class FloatingAgent : Agent
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private float impulseSpeed = Random.Range(1f, 2f);
        [SerializeField] private float maxY = Random.Range(2f, 10f);
        [SerializeField] private float minY = Random.Range(-10f, -2f);

        private BehaviorParameters behaviorParameters;

        public override void CollectObservations()
        {
            VelocityObservation();
            PositionObservation();
            BoundariesObservation();
            ImpulseSpeedObservation();
        }

        private void ImpulseSpeedObservation()
        {            
            AddVectorObs(impulseSpeed);
        }

        private void BoundariesObservation()
        {
            AddVectorObs(maxY);
            AddVectorObs(minY);
        }

        private void PositionObservation()
        {
            AddVectorObs(body.transform.localPosition.y);
        }

        private void VelocityObservation()
        {
            AddVectorObs(body.velocity.y);
        }

        public override void InitializeAgent()
        {
            base.InitializeAgent();
            behaviorParameters = GetComponent<BehaviorParameters>();
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
            var force = ScaleAction(act[0], 0, 1);
            Impulse(impulseSpeed * force);
        }

        private void DiscreteActions(float[] act)
        {
            var impulse = Mathf.FloorToInt(act[0]);
            if (impulse == 1) Impulse(impulseSpeed);
        }

        private void Impulse(float speed)
        {
            body.AddForce(Vector3.up * speed, ForceMode.VelocityChange);
        }


        public override float[] Heuristic()
        {
            var action = new float[behaviorParameters.brainParameters.vectorActionSize.Length];

            if (Keys.UP)
            {
                action[0] = 1f;
            }

            return action;
        }

        internal static class Keys
        {
            public static bool UP => Input.GetKey(KeyCode.Space);
        }

        public void SetParameters(float impulseSpeed, float maxY, float minY)
        {
            this.impulseSpeed = impulseSpeed;
            this.maxY = maxY;
            this.minY = minY;
        }

        public void Reset(Vector3 position, Vector3 velocity)
        {
            transform.position = position;
            GetComponentInChildren<Rigidbody>().velocity = velocity;
        }
    }
}