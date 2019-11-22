using MLAgents;
using UnityEngine;

namespace Flying.Scripts
{
    public class FlyingArea : Area, CollisionReceiver
    {
        [SerializeField] private Transform respawnPoint;
        private FlyingAgent flyingAgent;

        private void Awake()
        {
            flyingAgent = GetComponentInChildren<FlyingAgent>();
        }


        public void Step(int maxSteps)
        {
            flyingAgent.AddReward(1f / maxSteps);
        }

        public override void ResetArea()
        {
            flyingAgent.Done();
            flyingAgent.transform.position = respawnPoint.position;
            flyingAgent.transform.rotation = Quaternion.identity;
            flyingAgent.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        }


        public void OnCollisionEnter(Collision other)
        {
            ResetIfAgent(other.gameObject);
        }

        public void OnTriggerEnter(Collider other)
        {
            ResetIfAgent(other.gameObject);
        }

        private void ResetIfAgent(GameObject other)
        {
            if (!other.gameObject.CompareTag("agent")) return;

            flyingAgent.SetReward(-1f);
            ResetArea();
        }
    }
}