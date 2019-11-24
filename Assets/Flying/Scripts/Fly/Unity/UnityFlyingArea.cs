using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace Flying.Scripts.Fly.Unity
{
    public class UnityFlyingArea : MonoBehaviour
    {
        [SerializeField] private Transform spawnPosition;
        [SerializeField] private Transform target;
        [SerializeField] private float moveSpeed;
        private int step;
        private int stepsInsideTarget;
        public FlyingAgentModel agent { get; private set; }
        private FlyingAreaModel area;

        private UnityFlyingAgent unityAgent;

        public void Awake()
        {
            unityAgent = GetComponentInChildren<UnityFlyingAgent>();
            Initialize();
        }

        private void Initialize()
        {
            step = 0;
            var initialPosition = spawnPosition.localPosition;
            var targetPosition = target.localPosition;
            agent = FlyingAgent.Create(initialPosition, targetPosition, moveSpeed);
            area = FlyingArea.Create(agent, targetPosition, 
                distanceReward: 0.01f, distancePunish: -0.01f, doneReward: 1f, failReward: -1f);
            ResetUnityAgent(initialPosition);
        }

        public void Step(int maxSteps)
        {
            var newPosition = unityAgent.transform.localPosition;
            var newVelocity = unityAgent.body.velocity;
            var newTargetPosition = target.localPosition;
            agent = FlyingAgent.Update(agent, newPosition, newTargetPosition, velocity: newVelocity);
            area = FlyingArea.Update(area, agent, newTargetPosition);
            area = FlyingArea.UpdateDoneBasedOnValidate(area);
            if (area.done)
            {
                unityAgent.AddReward(2f / maxSteps);
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
            return agent.position.y < -10f || agent.position.y > 30f;
        }

        private void AgentFail()
        {
            unityAgent.SetReward(area.failReward);
            unityAgent.Done();
            Reset();
        }

        private void AgentDone()
        {
            unityAgent.SetReward(area.doneReward);
            unityAgent.Done();
            Reset();
        }

        private void AgentStep(int maxSteps)
        {
            //model = FlyingArea.UpdateNextRewardBasedOnPosition(model, agentModel.position);
            var distanceReward = Vector3.Distance(area.agent.position, area.target);
            unityAgent.AddReward((-1f * distanceReward) / (maxSteps * 100));
            //agent.AddReward(-1f / maxSteps);

            step += 1;
            if (step > maxSteps) Reset();
        }

        private void Reset()
        {
            step = 0;
            var initialPosition = spawnPosition.localPosition;
            var targetPosition = target.localPosition;
            agent = FlyingAgent.Update(agent, initialPosition, targetPosition);
            area = FlyingArea.Update(area, agent, targetPosition);
            ResetUnityAgent(initialPosition);
        }
        
        private void ResetUnityAgent(Vector3 initialPosition)
        {
            unityAgent.transform.localPosition = initialPosition;
            unityAgent.body.velocity = Vector3.zero;
        }
    }
}