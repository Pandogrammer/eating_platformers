using UnityEngine;

namespace Flying.Scripts.Fly
{
    public struct FlyingAgentModel
    {
        public readonly Vector3 position;
        public readonly Vector3 target;
        public readonly float speed;
        public readonly Vector3 velocity;

        internal FlyingAgentModel(Vector3 position, Vector3 target, float speed, 
            Vector3 velocity)
        {
            this.position = position;
            this.target = target;
            this.speed = speed;
            this.velocity = velocity;
        }
    }

    public static class FlyingAgent
    {
        public static FlyingAgentModel Create(
            Vector3? position = null,
            Vector3? target = null,
            float? speed = null,
            Vector3? velocity = null)
        {
            var newPosition = position.GetValueOrDefault(Vector3.zero);
            var newTarget = target.GetValueOrDefault(Vector3.zero);
            var newSpeed = speed.GetValueOrDefault(1);
            var newVelocity = velocity.GetValueOrDefault(Vector3.zero);
            return new FlyingAgentModel(newPosition, newTarget, newSpeed, newVelocity);
        }
        
        public static FlyingAgentModel Update(FlyingAgentModel agent,
            Vector3? position = null,
            Vector3? target = null,
            float? speed = null,
            Vector3? velocity = null)
        {
            var newPosition = position.GetValueOrDefault(agent.position);
            var newTarget = target.GetValueOrDefault(agent.target);
            var newSpeed = speed.GetValueOrDefault(agent.speed);
            var newVelocity = velocity.GetValueOrDefault(agent.velocity);
            return new FlyingAgentModel(newPosition, newTarget, newSpeed, newVelocity);
        }

    }
}