using Flying.Scripts.Stepable;
using UnityEngine;

namespace Flying.Scripts.Rotate
{
    public class UnityRotateArea : UnityStepableArea
    {
        [SerializeField] private Transform target;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float doneReward;
        [SerializeField] private bool rotationReward;
        public RotateAgent.RotateAgentModel agent { get; private set; }
        private int step;
        private UnityRotateAgent unityAgent;

        public void Awake()
        {
            unityAgent = GetComponentInChildren<UnityRotateAgent>();
            Initialize();
        }

        public override void Step(int maxSteps)
        {
            step += 1;

            var agentBody = unityAgent.body;
            var agentPosition = agentBody.transform.localPosition;
            var agentRotation = agentBody.rotation;
            var agentVelocity = agentBody.angularVelocity;
            var targetPosition = target.localPosition;

            agent = RotateAgent.Update(agent, agentRotation, targetPosition, agentVelocity);
            var rotationToTarget =
                Vector3.Dot(agentBody.transform.forward, (targetPosition - agentPosition).normalized);

            if (IsFacingTarget(rotationToTarget))
                AgentDone();
            else if (step > maxSteps)
                TimeUp();
            else
                AgentStep(maxSteps, rotationToTarget);
        }


        private static bool IsFacingTarget(float rotationToTarget)
        {
            return rotationToTarget > 0.95f;
        }

        private void TimeUp()
        {
            unityAgent.Done();
            Reset();
        }

        private void AgentDone()
        {
            if (unityAgent.IsDone()) return;
            unityAgent.SetReward(doneReward);
            unityAgent.Done();
            Reset(true);
        }

        private void AgentStep(int maxSteps, float rotationToTarget)
        {
            if (!rotationReward) return;
            if (unityAgent.IsDone()) return;
            var stepReward = 1F / rotationToTarget / maxSteps;
            unityAgent.AddReward(stepReward);
        }

        private void Initialize()
        {
            step = 0;
            var targetPosition = target.localPosition;

            agent = RotateAgent.Create(Quaternion.identity, targetPosition, moveSpeed);
            ResetUnityEntities(Quaternion.identity, targetPosition);
        }

        private void Reset(bool changePosition = false)
        {
            step = 0;
            var targetPosition = target.localPosition;
            var initialRotation = Quaternion.identity;
            var initialVelocity = Vector3.zero;

            if (changePosition)
            {
                var randomZ = Random.Range(0, 2);
                if (randomZ == 0)
                {
                    targetPosition.z = 0;
                    var randomX = Random.Range(0, 2);
                    if(randomX == 0)
                        targetPosition.x = -5f;
                    else
                        targetPosition.x = 5f;
                }
                else
                {
                    targetPosition.z = -5f;
                    targetPosition.x = 0;
                }
                    
            }

            agent = RotateAgent.Update(agent, initialRotation, targetPosition, initialVelocity);
            ResetUnityEntities(initialRotation, targetPosition);
        }

        private void ResetUnityEntities(Quaternion initialRotation, Vector3 targetPosition)
        {
            target.localPosition = targetPosition;
            unityAgent.body.angularVelocity = Vector3.zero;
            unityAgent.body.rotation = initialRotation;
        }
    }
}