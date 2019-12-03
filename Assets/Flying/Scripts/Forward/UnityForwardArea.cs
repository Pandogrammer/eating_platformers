using Flying.Scripts.Fly;
using Flying.Scripts.Stepable;
using UnityEngine;

public class UnityForwardArea : UnityStepableArea
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed;
    public ForwardAgentModel agent { get; private set; }
    private int step;
    private UnityForwardAgent unityAgent;

    public void Awake()
    {
        unityAgent = GetComponentInChildren<UnityForwardAgent>();
        Initialize();
    }

    private void Initialize()
    {
        step = 0;
        var initialPosition = spawnPosition.localPosition;
        var targetPosition = target.localPosition;
    }


    public override void Step(int maxSteps)
    {
        step += 1;
        var newPosition = unityAgent.transform.localPosition;
        var newRotation = unityAgent.body.rotation.eulerAngles;
        var newVelocity = unityAgent.body.velocity;
        var newTargetPosition = target.localPosition;
    }
}