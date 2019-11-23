using UnityEngine;

namespace Flying.Scripts.Fly.Unity
{
    public class UnityFlyingArea : MonoBehaviour
    {
        [SerializeField] private Transform spawnPosition;
        [SerializeField] private Transform target;
        [SerializeField] private float moveSpeed;
        private int step;
        private int stepsInsideTarget;
        private FlyingAreaModel model;
        private UnityFlyingAgent agent;
        public FlyingAgentModel agentModel { get; private set; }

        public void Awake()
        {
            agent = GetComponentInChildren<UnityFlyingAgent>();
            Reset();
        }

        public void Step(int maxSteps)
        {
            var newPosition = agent.transform.localPosition;
            var newVelocity = agent.body.velocity;
            agentModel = FlyingAgent.Move(agentModel, newPosition, newVelocity);
            model = FlyingArea.UpdateAgentModel(model, agentModel);
            model = FlyingArea.CheckIfAgentDone(model);
            if (model.done)
            {
                agent.AddReward(2f / maxSteps);
                stepsInsideTarget += 1;
                if (stepsInsideTarget > 150)
                {
                    stepsInsideTarget = 0;
                    AgentDone();
                }
            }
            else if (AgentOutOfBounds())
            {
                AgentFail();
            }
            else
            {
                stepsInsideTarget = 0;
            }
            
            AgentStep(maxSteps);
        }

        private bool AgentOutOfBounds()
        {
            return agentModel.position.y < -10f || agentModel.position.y > 30f;
        }

        private void AgentFail()
        {
            agent.SetReward(-1f);
            agent.Done();
            Reset();
        }

        private void AgentDone()
        {
            agent.SetReward(1f);
            agent.Done();
            Reset();
        }

        private void AgentStep(int maxSteps)
        {
            //model = FlyingArea.UpdateNextRewardBasedOnPosition(model, agentModel.position);
            var distanceReward = Vector3.Distance(model.agent.position, model.target);
            agent.AddReward((-1f * distanceReward) / (maxSteps * 100));
            //agent.AddReward(-1f / maxSteps);

            step += 1;
            if (step > maxSteps) Reset();
        }

        public void Reset()
        {
            step = 0;
            var initialPosition = spawnPosition.localPosition;
            var targetPosition = target.localPosition;
            agent.transform.localPosition = initialPosition;
            agent.body.velocity = Vector3.zero;
            agentModel = FlyingAgent.Create(initialPosition, targetPosition, moveSpeed);
            model = FlyingArea.Create(agentModel, targetPosition, 0.01f, -0.01f);
        }
    }
}