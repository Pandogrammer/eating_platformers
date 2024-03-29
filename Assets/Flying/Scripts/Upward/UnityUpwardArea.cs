using System;
using Flying.Scripts.Fly;
using GenericScripts.Stepable;
using UnityEngine;

public class UnityUpwardArea : UnityStepableArea
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float doneReward;
    [SerializeField] private bool distanceReward;
    public UpwardAgent.UpwardAgentModel agent { get; private set; }
    private int step;
    private UnityUpwardAgent unityAgent;

    public void Awake()
    {
        unityAgent = GetComponentInChildren<UnityUpwardAgent>();
        Initialize();
    }

    public void Step(int maxSteps)
    {
        step += 1;

        var position = unityAgent.body.transform.localPosition;
        var targetPosition = target.localPosition;
        var velocity = unityAgent.body.velocity;
        
        agent = UpwardAgent.Update(agent, position, targetPosition, velocity);
        var distanceToTarget = Math.Abs(Vector3.Distance(targetPosition, position));
        
        if (HasReachedTarget(distanceToTarget))
            AgentDone();
        else if (step > maxSteps)
            TimeUp();
        else
            AgentStep(maxSteps, distanceToTarget);
    }


    private static bool HasReachedTarget(float distanceToTarget)
    {
        return distanceToTarget < 0.05f;
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

    private void AgentStep(int maxSteps, float distanceToTarget)
    {
        if (!distanceReward) return;
        if (unityAgent.IsDone()) return;
        var stepReward = 1F / distanceToTarget / maxSteps;
        unityAgent.AddReward(stepReward);
    }

    private void Initialize()
    {
        step = 0;
        var initialPosition = spawnPosition.localPosition;
        var targetPosition = target.localPosition;

        agent = UpwardAgent.Create(initialPosition, targetPosition, moveSpeed);
        ResetUnityEntities(initialPosition, targetPosition);
    }

    private void Reset(bool changePosition = false)
    {
        step = 0;
        var initialPosition = spawnPosition.localPosition;
        var initialVelocity = Vector3.zero; 
        var targetPosition = target.localPosition;
        
        if(changePosition) targetPosition.y *= -1f;
        
        agent = UpwardAgent.Update(agent, initialPosition, targetPosition, initialVelocity);
        ResetUnityEntities(initialPosition, targetPosition);
    }

    private void ResetUnityEntities(Vector3 initialPosition, Vector3 targetPosition)
    {
        target.localPosition = targetPosition;
        unityAgent.body.transform.localPosition = initialPosition;
        unityAgent.body.velocity = Vector3.zero;
        unityAgent.body.rotation = Quaternion.identity;
    }

    public override void Step(int step, int maxSteps)
    {
        throw new NotImplementedException();
    }

    public override void Reset()
    {
        throw new NotImplementedException();
    }
}