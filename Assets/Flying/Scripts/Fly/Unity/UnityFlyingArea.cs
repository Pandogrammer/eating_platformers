using GenericScripts.Stepable;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace Flying.Scripts.Fly.Unity
{
    public class UnityFlyingArea : UnityStepableArea
    {
        [SerializeField] private Transform spawnPosition;
        [SerializeField] private Transform target;
        [SerializeField] private float moveSpeed;
        private int step;
        private int stepsInsideTarget;
        private LevelParameters levelParameters;
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
            stepsInsideTarget = 0;
            levelParameters = LevelParameters.Create(100, 30f, 1f);
            
            var initialPosition = spawnPosition.localPosition;
            var targetPosition = target.localPosition;
            agent = FlyingAgent.Create(initialPosition, target: targetPosition, speed: moveSpeed);
            area = FlyingArea.Create(agent, targetPosition, doneReward: 2f, failReward: -2f);
            ResetUnityEntities(initialPosition, targetPosition);
        }

        public void Step(int maxSteps)
        {
            step += 1;
            var newPosition = unityAgent.transform.localPosition;
            var newRotation = unityAgent.body.rotation.eulerAngles;
            var newVelocity = unityAgent.body.velocity;
            var newTargetPosition = target.localPosition;
            var isInsideArea = FlyingArea.IsInsideArea(area);
            agent = FlyingAgent.Update(agent, newPosition, newTargetPosition, newRotation, velocity: newVelocity,
                speed: moveSpeed);
            area = FlyingArea.Update(area, agent, newTargetPosition, isInsideArea);

            if (!isInsideArea)
                AgentOutsideArea(maxSteps);
            if (isInsideArea)
                AgentInsideArea(maxSteps);
            if (AgentOutOfBounds())
                AgentFail();
            if (step > maxSteps)
                TimeUp();

            AgentStep(maxSteps);
        }


        private void AgentOutsideArea(int maxSteps)
        {
            if (unityAgent.IsDone()) return;
            stepsInsideTarget = 0;
            var proportionalDistance = Vector3.Distance(area.agent.position, area.target) / levelParameters.maxDistance;
            var rewardModifier = area.failReward / 2 / maxSteps;
            var outsideAreaReward = proportionalDistance * rewardModifier;
            unityAgent.AddReward(outsideAreaReward);
        }

        private void AgentInsideArea(int maxSteps)
        {
            if (unityAgent.IsDone()) return;
            stepsInsideTarget += 1;

            var insideAreaReward = area.doneReward / maxSteps;
            unityAgent.AddReward(insideAreaReward);

            if (stepsInsideTarget > 100) AgentDone();
        }

        private bool AgentOutOfBounds()
        {
            var targetPosition = area.target;
            var agentPosition = area.agent.position;
            return Vector3.Distance(targetPosition, agentPosition) > levelParameters.maxDistance;
        }

        private void TimeUp()
        {
            unityAgent.Done();
            Reset();
        }
        
        private void AgentFail()
        {
            if (unityAgent.IsDone()) return;
            unityAgent.SetReward(area.failReward);
            unityAgent.Done();
            Reset();
        }

        private void AgentDone()
        {
            if (unityAgent.IsDone()) return;
            unityAgent.SetReward(area.doneReward);
            unityAgent.Done();
            levelParameters = levelParameters.AdvanceLevel();
            Reset(true);
        }

        private void AgentStep(int maxSteps)
        {
            if (unityAgent.IsDone()) return;
            var stepReward = area.failReward / 2 / maxSteps;
            unityAgent.AddReward(stepReward);
        }

        private void Reset(bool changeTargetPosition = false)
        {
            step = 0;
            stepsInsideTarget = 0;
            var initialPosition = spawnPosition.localPosition;
            var targetLocalPosition = target.localPosition;
            var targetPosition = changeTargetPosition
                ? GenerateNewTargetPosition(initialPosition)
                : targetLocalPosition;

            agent = FlyingAgent.Update(agent, initialPosition, targetPosition);
            area = FlyingArea.Update(area, agent, targetPosition);
            ResetUnityEntities(initialPosition, targetPosition);
        }

        private Vector3 GenerateNewTargetPosition(Vector3 initialPosition)
        { 
            var newPosition = new Vector3(
                (initialPosition.x + randomDistance),
                (initialPosition.y + randomDistance),
                (initialPosition.z + randomDistance));

            return newPosition;
        }

        private float randomDistance => Random.Range(-levelParameters.targetDistanceFromSpawn, levelParameters.targetDistanceFromSpawn);

        private void ResetUnityEntities(Vector3 initialPosition, Vector3 targetPosition)
        {
            target.localPosition = targetPosition;
            unityAgent.transform.localPosition = initialPosition;
            unityAgent.body.velocity = Vector3.zero;
            unityAgent.body.rotation = Quaternion.identity;
        }

        public override void Step(int step, int maxSteps)
        {
            throw new System.NotImplementedException();
        }

        public override void Reset()
        {
            throw new System.NotImplementedException();
        }
    }

    internal struct LevelParameters
    {
        public readonly int stepsInsideTarget;
        public readonly float maxDistance;
        public readonly float targetDistanceFromSpawn;

        private LevelParameters(
            int stepsInsideTarget,
            float maxDistance,
            float targetDistanceFromSpawn)
        {
            this.stepsInsideTarget = stepsInsideTarget;
            this.maxDistance = maxDistance;
            this.targetDistanceFromSpawn = targetDistanceFromSpawn;
        }

        public static LevelParameters Create(int initialStepsInsideTarget, float initialMaxDistance, float initialTargetDistanceFromSpawn)
        {
            return new LevelParameters(initialStepsInsideTarget, 
                initialMaxDistance, 
                initialTargetDistanceFromSpawn);
        }

        public LevelParameters AdvanceLevel()
        {
            var stepsInsideTarget = this.stepsInsideTarget + 5;
            var maxDistance = this.maxDistance + 0.1f;
            var targetDistanceFromSpawn = this.targetDistanceFromSpawn + 0.1f;
            return new LevelParameters(stepsInsideTarget, maxDistance, targetDistanceFromSpawn);
        }
    }
}