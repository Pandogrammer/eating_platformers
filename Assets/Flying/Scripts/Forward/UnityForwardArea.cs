using Flying.Scripts.Stepable;
using UnityEngine;

public class UnityForwardArea : UnityStepableArea
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed;
    public ForwardAgentModel agent { get; private set; }

    public override void Step(int maxSteps)
    {
        throw new System.NotImplementedException();
    }
}