using System;
using Flying.Scripts.Fly;
using Flying.Scripts.Stepable;
using UnityEngine;

public class UnityForwardArea : UnityStepableArea
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed;

    [SerializeField] private float failReward;
    [SerializeField] private float doneReward;
    public ForwardAgent.ForwardAgentModel agent { get; private set; }
    private int step;
    private UnityForwardAgent unityAgent;

    public void Awake()
    {
        unityAgent = GetComponentInChildren<UnityForwardAgent>();
        Initialize();
    }

    public override void Step(int maxSteps)
    {
        step += 1;
        
        agent = ForwardAgent.Update(agent, unityAgent.body.transform.localPosition, target.localPosition);

        if (HasReachedTarget())
            AgentDone();
        else if (step > maxSteps)
            TimeUp();
        else
            AgentStep(maxSteps);
    }


    private bool HasReachedTarget()
    {
        return Math.Abs(Vector3.Distance(target.localPosition, agent.position)) < 0.05f;
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
        Reset();
    }

    private void AgentStep(int maxSteps)
    {
        if (unityAgent.IsDone()) return;
        var stepReward = -1F / maxSteps;
        unityAgent.AddReward(stepReward);
    }

    private void Initialize()
    {
        step = 0;
        var initialPosition = spawnPosition.localPosition;
        var targetPosition = target.localPosition;

        agent = ForwardAgent.Create(initialPosition, targetPosition, moveSpeed);
        ResetUnityEntities(initialPosition, targetPosition);
    }

    private void Reset()
    {
        step = 0;
        var initialPosition = spawnPosition.localPosition;
        var targetPosition = target.localPosition;
        targetPosition.z *= -1f;
        
        agent = ForwardAgent.Update(agent, initialPosition, targetPosition);
        ResetUnityEntities(initialPosition, targetPosition);
    }

    private void ResetUnityEntities(Vector3 initialPosition, Vector3 targetPosition)
    {
        target.localPosition = targetPosition;
        unityAgent.body.transform.localPosition = initialPosition;
        unityAgent.body.velocity = Vector3.zero;
        unityAgent.body.rotation = Quaternion.identity;
    }
}