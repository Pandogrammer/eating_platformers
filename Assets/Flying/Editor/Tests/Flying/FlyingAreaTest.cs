using Flying.Scripts.Fly;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Flying.Editor.Tests.Flying
{
    public class FlyingAreaTest
    {

        [Test]
        public void agent_done()
        {
            var target = new Vector3(0, 3, 0);
            var agent = FlyingAgent.Create(new Vector3(0, 2.999f, 0), target, 10f);
            var area = FlyingArea.Create(agent, target, 1, 1);

            var result = FlyingArea.CheckIfAgentDone(area);

            Assert.IsTrue(result.done);
        }

        [Test]
        public void distance_reward()
        {
            var distanceReward = 0.1f;
            var target = new Vector3(0, 3, 0);
            var agent = FlyingAgent.Create(new Vector3(0, 0, 0), target, 10f);
            var area = FlyingArea.Create(agent, target, distanceReward, 0);
            var agentStep = FlyingAgent.Move(agent, new Vector3(0, 1, 0), Vector3.zero);

            var result = FlyingArea.UpdateNextRewardBasedOnPosition(
                area,
                agentStep.position);

            Assert.AreEqual(distanceReward, result.nextReward);
        }

        [Test]
        public void distance_punish()
        {
            var distancePunish = -0.1f;
            var target = new Vector3(0, 3, 0);
            var agent = FlyingAgent.Create(new Vector3(0, 1, 0), target, 10f);
            var area = FlyingArea.Create(agent, target, 0, distancePunish);
            var agentStep = FlyingAgent.Move(agent, new Vector3(0, 0, 0), Vector3.zero);

            var result = FlyingArea.UpdateNextRewardBasedOnPosition(
                area,
                agentStep.position);

            Assert.AreEqual(distancePunish, result.nextReward);
        }
    }
}