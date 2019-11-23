using UnityEngine;

namespace Flying.Scripts.Fly
{
    public class FlyingAgentModel
    {
        public readonly Vector3 position;
        public readonly Vector3 target;
        public readonly float speed;
        public readonly Vector3 velocity;

        protected internal FlyingAgentModel(Vector3 position, Vector3 target, float speed, Vector3 velocity)
        {
            this.position = position;
            this.target = target;
            this.speed = speed;
            this.velocity = velocity;
        }
    }

    public static class FlyingAgent
    {
        public static FlyingAgentModel Create(Vector3 initialPosition, Vector3 target, float moveSpeed)
        {
            return new FlyingAgentModel(initialPosition, target, moveSpeed, Vector3.zero);
        }

        public static FlyingAgentModel Move(FlyingAgentModel agentModel, Vector3 position, Vector3 velocity)
        {
            return new FlyingAgentModel(position, agentModel.target, agentModel.speed, velocity);
        }
    }
}