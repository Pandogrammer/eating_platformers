using System;
using MLAgents;
using UnityEngine;

public class FlyingAgent : Agent
{
    [SerializeField] private Rigidbody body;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float impulseSpeed;
    
    private BehaviorParameters behaviorParameters;
    private FlyingBehaviour flying;


    public override void CollectObservations()
    {
        VelocityObservation();
        PositionObservation();
        RotationObservation();
    }

    private void RotationObservation()
    {
        AddVectorObs(body.transform.rotation);
    }

    private void PositionObservation()
    {
        AddVectorObs(body.transform.localPosition);
    }

    private void VelocityObservation()
    {
        AddVectorObs(body.velocity);
    }

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        flying = new FlyingBehaviour();
        behaviorParameters = GetComponent<BehaviorParameters>();
    }


    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (behaviorParameters.brainParameters.vectorActionSpaceType == SpaceType.Discrete)
            DiscreteActions(vectorAction);
    }
    
    private void DiscreteActions(float[] act)
    {
        var tilt = Mathf.FloorToInt(act[0]);
        switch (tilt)
        {
            case 1:
                Move(1f);
                break;
            case 2:
                Move(-1f);
                break;
        }

        var rotate = Mathf.FloorToInt(act[1]);
        switch (rotate)
        {
            case 1:
                Rotate(1f);
                break;
            case 2:
                Rotate(-1f);
                break;
        }

        var impulse = Mathf.FloorToInt(act[2]);
        if (impulse == 1) Impulse();
    }

    private void Impulse()
    {
        flying.Impulse(body, impulseSpeed);
    }

    private void Rotate(float direction)
    {
        flying.Rotate(body.transform, direction, rotationSpeed);
    }

    private void Move(float direction)
    {
        flying.Move(body, direction, moveSpeed);
    }

    public override float[] Heuristic()
    {
        var action = new float[behaviorParameters.brainParameters.vectorActionSize.Length];
        if (Keys.FORWARD)
        {
            action[0] = 1f;
        }

        if (Keys.BACK)
        {
            action[0] = 2f;
        }

        if (Keys.RIGHT)
        {
            action[1] = 1f;
        }

        if (Keys.LEFT)
        {
            action[1] = 2f;
        }

        if (Keys.UP)
        {
            action[2] = 1f;
        }

        return action;
    }
    
    internal static class Keys
    {
        public static bool UP => Input.GetKey(KeyCode.Space);
        public static bool FORWARD => Input.GetKey(KeyCode.W);
        public static bool BACK => Input.GetKey(KeyCode.S);
        public static bool LEFT => Input.GetKey(KeyCode.A);
        public static bool RIGHT => Input.GetKey(KeyCode.D);
    }
}
