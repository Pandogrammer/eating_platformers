using System;
using Behaviours;
using MLAgents;
using Platformer;
using UnityEngine;

public class PlatformerAgent : Agent
{
    private BehaviorParameters behaviorParameters;
    private RayPerception perception;
    private MovementBehaviour movement;
    private EatingBehaviour eating;

    private Rigidbody parentBody;
    private Transform parentTransform;

    public int totalFoodEaten { get; private set; }

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        perception = GetComponent<RayPerception>();
        behaviorParameters = GetComponent<BehaviorParameters>();
        parentTransform = transform.parent;
        parentBody = GetComponentInParent<Rigidbody>();
        totalFoodEaten = 0;

        movement = GetComponent<MovementBehaviour>();
        eating = GetComponent<EatingBehaviour>();
    }

    public override void CollectObservations()
    {
        VisionPerception(5, 2f, 40f, 90f, 5f);
        MomentumPerception();
        TotalFoodEatenPerception();
        PositionPerception();
        RotationPerception();
        AddVectorObs(GetStepCount() / (float) agentParameters.maxStep);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (behaviorParameters.brainParameters.vectorActionSpaceType == SpaceType.Discrete)
            DiscreteActions(vectorAction);
        else
            ContinuousActions(vectorAction);

        AddReward(-1f / agentParameters.maxStep);
    }

    private void ContinuousActions(float[] vectorAction)
    {
        var movement = Mathf.Clamp(vectorAction[0], -1f, 1f);
        if (movement < -0.5f || movement > 0.5f) Move(movement);

        var rotation = Mathf.Clamp(vectorAction[1], -1f, 1f);
        if (rotation < -0.5f || rotation > 0.5f) Rotate(rotation);

        var eat = Mathf.Clamp(vectorAction[2], 0, 1f);
        if (eat > 0.5f) TryToEat();
    }

    public void Move(float direction)
    {
        parentBody = movement.Move(parentBody, parentTransform, direction);
    }

    public void Rotate(float rotateDir)
    {
        parentTransform = movement.Rotate(parentTransform, rotateDir);
    }

    private void DiscreteActions(float[] act)
    {
        var move = Mathf.FloorToInt(act[0]);
        switch (move)
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

        var eat = Mathf.FloorToInt(act[2]);
        if (eat == 1) TryToEat();
    }

    public override void AgentReset()
    {
        totalFoodEaten = 0;
        parentTransform.parent.GetComponent<PlatformerArea>().ResetArea();
    }

    private void TryToEat()
    {
        var foodEaten = eating.TryToEat();
        if (foodEaten <= 0)
        {
            AddReward(-0.001f);
            return;
        }

        totalFoodEaten += foodEaten;
        AddReward(foodEaten * 0.1f);
        if (totalFoodEaten > 10)
        {
            SetReward(2f);
            Done();
        }
    }

    public override float[] Heuristic()
    {
        var action = new float[behaviorParameters.brainParameters.vectorActionSize.Length];
        if (Keys.UP)
        {
            action[0] = 1f;
        }

        if (Keys.DOWN)
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

        if (Keys.EAT)
        {
            action[2] = 1f;
        }

        return action;
    }


    private void PositionPerception()
    {
        AddVectorObs(parentTransform.localPosition);
    }

    private void VisionPerception(int rays, float distanceModifier, float distance, float rayAngle, float angleModifier)
    {
        string[] detectableObjects = {"food", "wall", "agent"};
        for (int i = 0; i < rays; i++)
        {
            float rayDistance = distance - distanceModifier * i;
            float[] rayAngles = i == 0
                ? new[] {rayAngle}
                : new[] {rayAngle - angleModifier * i, rayAngle + angleModifier * i};
            AddVectorObs(perception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        }
    }

    private void RotationPerception()
    {
        AddVectorObs(parentTransform.rotation);
    }

    private void TotalFoodEatenPerception()
    {
        AddVectorObs(totalFoodEaten);
    }

    private void MomentumPerception()
    {
        AddVectorObs(parentBody.velocity);
    }
}

internal static class Keys
{
    public static bool EAT => Input.GetKey(KeyCode.Space);
    public static bool UP => Input.GetKey(KeyCode.W);
    public static bool DOWN => Input.GetKey(KeyCode.S);
    public static bool LEFT => Input.GetKey(KeyCode.A);
    public static bool RIGHT => Input.GetKey(KeyCode.D);
}