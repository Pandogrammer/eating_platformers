using System.Runtime.CompilerServices;
using UnityEngine;

namespace Flying.Scripts.Fly
{
    public struct FlyingAreaModel
    {
        public readonly FlyingAgentModel agent;
        public readonly Vector3 target;
        public readonly bool isInsideArea;
        public readonly float doneReward;
        public readonly float failReward;

        internal FlyingAreaModel(FlyingAgentModel agent, Vector3 target, bool isInsideArea,
            float doneReward, float failReward)
        {
            this.agent = agent;
            this.target = target;
            this.isInsideArea = isInsideArea;
            this.doneReward = doneReward;
            this.failReward = failReward;
        }
    }

    public static class FlyingArea
    {
        public static FlyingAreaModel Create(FlyingAgentModel? agent = null
            , Vector3? target = null
            , bool? isInsideArea = null
            , float? doneReward = null
            , float? failReward = null
)
        {
            var newAgent = agent.GetValueOrDefault(FlyingAgent.Create());
            var newTarget = target.GetValueOrDefault(Vector3.zero);
            var newDone = isInsideArea.GetValueOrDefault(false);
            var newDoneReward = doneReward.GetValueOrDefault(1f);
            var newFailReward = failReward.GetValueOrDefault(-1f);
            return new FlyingAreaModel(newAgent, newTarget, newDone, newDoneReward, newFailReward);
        }

        public static FlyingAreaModel Update(FlyingAreaModel area
            , FlyingAgentModel? agent = null
            , Vector3? target = null
            , bool? isInsideArea = null
            , float? doneReward = null
            , float? failReward = null)
        {
            var newAgent = agent.GetValueOrDefault(area.agent);
            var newTarget = target.GetValueOrDefault(area.target);
            var newDone = isInsideArea.GetValueOrDefault(area.isInsideArea);
            var newDoneReward = doneReward.GetValueOrDefault(area.doneReward);
            var newFailReward = failReward.GetValueOrDefault(area.failReward);
            return new FlyingAreaModel(newAgent, newTarget, newDone, newDoneReward, newFailReward);
        }

        public static bool IsInsideArea(FlyingAreaModel model)
        {
            return Vector3.Distance(model.agent.position, model.target) < 1f;
        }

    }
}