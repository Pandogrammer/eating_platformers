using UnityEngine;

namespace Flying.Scripts.Rotate
{
    public static class RotateAgent
    {
        public struct RotateAgentModel
        {
            public readonly float speed;
            public readonly Quaternion rotation;
            public readonly Vector3 velocity;
            public readonly Vector3 target;

            internal RotateAgentModel(float speed, Quaternion rotation, Vector3 velocity, Vector3 target)
            {
                this.speed = speed;
                this.rotation = rotation;
                this.velocity = velocity;
                this.target = target;
            }
        }
    
        public static RotateAgentModel Create(Quaternion initialRotation, Vector3 target, float speed)
        {
            return new RotateAgentModel(speed, initialRotation, Vector3.zero, target);
        }

        public static RotateAgentModel Update(RotateAgentModel agent, Quaternion rotation, Vector3 targetPosition, Vector3 velocity)
        {
            return new RotateAgentModel(agent.speed, rotation, velocity, targetPosition);
        }
    }
}