using MLAgents;
using Nolose.Scripts;
using UnityEngine;

public class CollectorAgent : Agent
{
    [SerializeField] private Collector collector;
    [SerializeField] private BehaviorParameters behaviorParameters;
    [SerializeField] private CollectingArea area;
    private int step;

    public Collector Collector => collector;

    public override void InitializeAgent()
    {
        step = 0;
    }

    public override void CollectObservations()
    {
        AddVectorObs(collector.Body.velocity);
        AddVectorObs(collector.hp);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (behaviorParameters.brainParameters.vectorActionSpaceType == SpaceType.Discrete)
            DiscreteActions(vectorAction);
        
        var reward = CollectorRewardFunction.Calculate(this, agentParameters.maxStep);
        AddReward(reward);
        Step();
    }

    private void Step()
    {
        var maxStep = agentParameters.maxStep;
        area.Step(step, maxStep);
        step++;
        CheckReset(maxStep);
    }

    private void CheckReset(int maxStep)
    {
        if (step <= maxStep) return;
        step = 0;
        Done();
        area.Reset();
    }


    private void DiscreteActions(float[] act)
    {
        var move = Mathf.FloorToInt(act[0]);
        switch (move)
        {
            case 1:
                collector.GoForward();
                break;
            case 2:
                collector.GoBackward();
                break;
        }

        var rotate = Mathf.FloorToInt(act[1]);
        switch (rotate)
        {
            case 1:
                collector.TurnRight();
                break;
            case 2:
                collector.TurnLeft();
                break;
        }

        var eat = Mathf.FloorToInt(act[2]);
        if (eat == 1) collector.Eat();
    }

    public override float[] Heuristic()
    {
        var action = new float[behaviorParameters.brainParameters.vectorActionSize.Length];
        
        if (Keys.FORWARD)
        {
            action[0] = 1f;
        }

        if (Keys.BACKWARD)
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

    internal static class Keys
    {
        public static bool EAT => Input.GetKey(KeyCode.Space);
        public static bool FORWARD => Input.GetKey(KeyCode.W);
        public static bool BACKWARD => Input.GetKey(KeyCode.S);
        public static bool LEFT => Input.GetKey(KeyCode.A);
        public static bool RIGHT => Input.GetKey(KeyCode.D);
    }

}