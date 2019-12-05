using Flying.Scripts.Stepable;
using UnityEngine;

namespace Flying.Scripts.Rotate
{
    public class UnityRotateArea : UnityStepableArea
    {
        [SerializeField] private Transform target;
        [SerializeField] private float rotationAngle;
        [SerializeField] private float doneReward;
        [SerializeField] private bool giveRotationReward;
        [SerializeField] private bool giveDoneReward;
        [SerializeField] private bool giveFailReward;
        [SerializeField] private int mustFaceTargetSteps = 0;
        private int stepsFacing = 0; 
        [SerializeField] private float facingDistance = 0.95f;
        [SerializeField] private float facingRewardDistance = 0.99f;
        public RotateAgent.RotateAgentModel agent { get; private set; }
        private int step;
        private int initialTurnDirection;
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
            var turnDirection = CalculateTurnDirection(agentBody, targetPosition, agentPosition);

            agent = RotateAgent.Update(agent, agentRotation, targetPosition, agentVelocity, turnDirection);

            if (IsFacingTarget(agentBody, targetPosition, agentPosition))
            {
                if (stepsFacing >= mustFaceTargetSteps)
                    AgentDone();
                else
                    FacingReward(agentBody, targetPosition, agentPosition, maxSteps);
            }
            else if (GoingWrongDirection(turnDirection))
                AgentFail();
            else if (step > maxSteps)
                TimeUp();
            else
                AgentStep(maxSteps, turnDirection);
        }

        private void FacingReward(Rigidbody agentBody, Vector3 targetPosition, Vector3 agentPosition, int maxSteps)
        {
            var facing = Vector3.Dot(agentBody.transform.forward, (targetPosition - agentPosition).normalized);
            if(facing > facingRewardDistance)
                unityAgent.AddReward(facing / maxSteps);
        }

        private static int CalculateTurnDirection(Rigidbody agentBody, Vector3 targetPosition, Vector3 agentPosition)
        {
            var rightAngle = Vector3.Angle(agentBody.transform.right, targetPosition - agentPosition);
            var leftAngle = Vector3.Angle(-agentBody.transform.right, targetPosition - agentPosition);
            return rightAngle < leftAngle ? 1 : -1;
        }


        private bool GoingWrongDirection(int turnDirection)
        {
            return turnDirection != initialTurnDirection;
        }


        private bool IsFacingTarget(Rigidbody agentBody, Vector3 targetPosition, Vector3 agentPosition)
        {
            var rotationToTarget =
                Vector3.Dot(agentBody.transform.forward, (targetPosition - agentPosition).normalized);
            var isFacing = rotationToTarget > facingDistance;
            if (!isFacing) stepsFacing = 0;
            else stepsFacing += 1;
            return isFacing;
        }

        private void TimeUp()
        {
            unityAgent.Done();
            Reset();
        }

        private void AgentDone()
        {
            if (!giveDoneReward) return;
            if (unityAgent.IsDone()) return;
            unityAgent.SetReward(doneReward);
            unityAgent.Done();
            Reset(true);
        }

        private void AgentFail()
        {
            if (!giveFailReward) return;
            if (unityAgent.IsDone()) return;
            unityAgent.SetReward(-doneReward);
            unityAgent.Done();
            Reset();
        }

        private void AgentStep(int maxSteps, float rotationToTarget)
        {
            if (!giveRotationReward) return;
            if (rotationToTarget == 0) return;
            if (unityAgent.IsDone()) return;
            var proportionalRotation = 1 - rotationToTarget;
            var stepReward = -5F * proportionalRotation / maxSteps;
            unityAgent.AddReward(stepReward);
        }

        private void Initialize()
        {
            step = 0;
            var targetPosition = target.localPosition;

            agent = RotateAgent.Create(Quaternion.identity, targetPosition, rotationAngle);
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
                    if (randomX == 0)
                        targetPosition.x = -5f;
                    else
                        targetPosition.x = 5f;
                }
                else
                {
                    targetPosition.z = -5f;
                    var randomX = Random.Range(-5f, 5f);
                    targetPosition.x = randomX;
                }
            }

            ResetUnityEntities(initialRotation, targetPosition);

            agent = RotateAgent.Update(agent, initialRotation, targetPosition, initialVelocity, initialTurnDirection);
        }

        private void ResetUnityEntities(Quaternion initialRotation, Vector3 targetPosition)
        {
            target.localPosition = targetPosition;
            unityAgent.body.angularVelocity = Vector3.zero;
            unityAgent.body.rotation = initialRotation;
            initialTurnDirection =
                CalculateTurnDirection(unityAgent.body, targetPosition, unityAgent.body.transform.localPosition);
        }
    }
}