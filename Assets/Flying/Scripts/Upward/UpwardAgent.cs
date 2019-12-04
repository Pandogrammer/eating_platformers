using UnityEngine;

public static class UpwardAgent
{
    public struct UpwardAgentModel
    {
        public readonly float speed;
        public readonly Vector3 position;
        public readonly Vector3 velocity;
        public readonly Vector3 target;

        internal UpwardAgentModel(float speed, Vector3 position, Vector3 velocity, Vector3 target)
        {
            this.speed = speed;
            this.position = position;
            this.velocity = velocity;
            this.target = target;
        }
    }
    
    public static UpwardAgentModel Create(Vector3 initialPosition, Vector3 target, float speed)
    {
        return new UpwardAgentModel(speed, initialPosition, Vector3.zero, target);
    }

    public static UpwardAgentModel Update(UpwardAgentModel agent, Vector3 position, Vector3 targetPosition, Vector3 velocity)
    {
        return new UpwardAgentModel(agent.speed, position, velocity, targetPosition);
    }
}