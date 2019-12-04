using UnityEngine;

public static class ForwardAgent
{
    public struct ForwardAgentModel
    {
        public readonly float speed;
        public readonly Vector3 position;
        public readonly Vector3 velocity;
        public readonly Vector3 target;

        internal ForwardAgentModel(float speed, Vector3 position, Vector3 velocity, Vector3 target)
        {
            this.speed = speed;
            this.position = position;
            this.velocity = velocity;
            this.target = target;
        }
    }
    
    public static ForwardAgentModel Create(Vector3 initialPosition, Vector3 target, float speed)
    {
        return new ForwardAgentModel(speed, initialPosition, Vector3.zero, target);
    }

    public static ForwardAgentModel Update(ForwardAgentModel agent, Vector3 position, 
        Vector3 targetPosition, Vector3 velocity)
    {
        return new ForwardAgentModel(agent.speed, position, velocity, targetPosition);
    }
}