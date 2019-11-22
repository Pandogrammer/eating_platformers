using MLAgents;
using UnityEngine;

namespace Flying.Scripts.Float
{
    public class FloatingArea : Area, CollisionReceiver
    {
        [SerializeField] private Transform respawnPoint;
        [SerializeField] private BoxCollider top;
        [SerializeField] private BoxCollider bottom;
        
        [SerializeField] private FloatingAgent agent;
        
        public void Step(int maxSteps)
        {
            var reward = 1f / maxSteps;
            agent.AddReward(reward);
        }

        public override void ResetArea()
        {
            agent.Done();
            agent.Reset(respawnPoint.position
                , Vector3.zero);
        }

        public void ChangeArea(float impulseSpeed, float maxY, float minY)
        {
            agent.SetParameters(impulseSpeed, maxY, minY);
            
            var topPosition = top.transform.localPosition;
            top.transform.localPosition = new Vector3(topPosition.x, maxY, topPosition.z);
            
            var botPosition = bottom.transform.localPosition;
            bottom.transform.localPosition = new Vector3(botPosition.x, minY, botPosition.z);
        }

        private void ResetIfAgent(GameObject other)
        {
            if (!other.gameObject.CompareTag("agent")) return;

            var reward = -1f;
            agent.SetReward(reward);
            ResetArea();
        }

        public void OnCollisionEnter(Collision other)
        {
            ResetIfAgent(other.gameObject);
        }

        public void OnTriggerEnter(Collider other)
        {
            ResetIfAgent(other.gameObject);
        }

        public bool MustAdvance()
        {
            return agent.GetCumulativeReward() > 0.9f;
        }
    }
}