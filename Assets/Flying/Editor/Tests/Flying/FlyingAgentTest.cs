using Flying.Scripts.Fly;
using NUnit.Framework;
using UnityEngine;

namespace Flying.Editor.Tests.Flying
{
    public class FlyingAgentTest
    {
        [Test]
        public void agent_creation()
        {
            var initialPosition = new Vector3(0, 0, 0);
            var moveSpeed = 1f;

            var agent = FlyingAgent.Create(initialPosition, initialPosition, moveSpeed);

            Assert.AreEqual(initialPosition, agent.position);
            Assert.AreEqual(moveSpeed, agent.speed);
        }

        [Test]
        public void agent_step()
        {
            var initialPosition = new Vector3(0, 0, 0);
            var moveSpeed = 1f;
            var agent = FlyingAgent.Create(initialPosition, initialPosition, moveSpeed);

            var result = FlyingAgent.Move(agent, initialPosition + Vector3.up, Vector3.zero);

            Assert.AreEqual(new Vector3(0, 1, 0), result.position);
        }
    }
}