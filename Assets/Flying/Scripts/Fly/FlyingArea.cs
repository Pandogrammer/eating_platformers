using UnityEngine;

namespace Flying.Scripts.Fly
{
    public struct FlyingAreaModel
    {
        public readonly FlyingAgentModel agent;
        public readonly Vector3 target;
        public readonly bool done;
        public readonly float distanceReward;
        public readonly float distancePunish;
        public readonly float doneReward;
        public readonly float failReward;
        public readonly float nextReward;

        internal FlyingAreaModel(FlyingAgentModel agent, Vector3 target, bool done, float distanceReward,
            float distancePunish, float doneReward, float failReward, float nextReward)
        {
            this.agent = agent;
            this.target = target;
            this.done = done;
            this.distanceReward = distanceReward;
            this.distancePunish = distancePunish;
            this.doneReward = doneReward;
            this.failReward = failReward;
            this.nextReward = nextReward;
        }
    }

    public static class FlyingArea
    {
        public static FlyingAreaModel Create(
            FlyingAgentModel? agent = null
            , Vector3? target = null
            , bool? done = null
            , float? distanceReward = null
            , float? distancePunish = null
            , float? doneReward = null
            , float? failReward = null
            , float? nextReward = null
        )
        {
            var newAgent = agent.GetValueOrDefault(FlyingAgent.Create());
            var newTarget = target.GetValueOrDefault(Vector3.zero);
            var newDone = done.GetValueOrDefault(false);
            var newDistanceReward = distanceReward.GetValueOrDefault(0.01f);
            var newDistancePunish = distancePunish.GetValueOrDefault(-0.01f);
            var newDoneReward = distancePunish.GetValueOrDefault(1f);
            var newFailReward = distancePunish.GetValueOrDefault(-1f);
            var newNextReward = nextReward.GetValueOrDefault(0f);
            return new FlyingAreaModel(newAgent, newTarget, newDone,
                newDistanceReward, newDistancePunish,
                newDoneReward, newFailReward, newNextReward);
        }

        public static FlyingAreaModel Update(FlyingAreaModel area
            , FlyingAgentModel? agent = null
            , Vector3? target = null
            , bool? done = null
            , float? distanceReward = null
            , float? distancePunish = null
            , float? doneReward = null
            , float? failReward = null
            , float? nextReward = null
        )
        {
            var newAgent = agent.GetValueOrDefault(area.agent);
            var newTarget = target.GetValueOrDefault(area.target);
            var newDone = done.GetValueOrDefault(area.done);
            var newDistanceReward = distanceReward.GetValueOrDefault(area.distanceReward);
            var newDistancePunish = distancePunish.GetValueOrDefault(area.distancePunish);
            var newDoneReward = doneReward.GetValueOrDefault(area.doneReward);
            var newFailReward = failReward.GetValueOrDefault(area.failReward);
            var newNextReward = nextReward.GetValueOrDefault(area.nextReward);
            return new FlyingAreaModel(newAgent, newTarget, newDone,
                newDistanceReward, newDistancePunish,
                newDoneReward, newFailReward, newNextReward);
        }

        public static FlyingAreaModel UpdateNextRewardBasedOnPosition(
            FlyingAreaModel model, Vector3 newPosition)
        {
            var nextReward = CalculateRewardBasedOnDistance(model, newPosition);
            return Update(model, nextReward: nextReward);
        }

        public static FlyingAreaModel UpdateDoneBasedOnValidate(FlyingAreaModel model)
        {
            return Update(model, done: Validate(model));
        }

        private static bool Validate(FlyingAreaModel model)
        {
            return Vector3.Distance(model.agent.position, model.target) < 1f;
        }

        private static float CalculateRewardBasedOnDistance(FlyingAreaModel model, Vector3 newPosition)
        {
            var firstDistance = Vector3.Distance(model.agent.position, model.target);
            var secondDistance = Vector3.Distance(newPosition, model.target);
            if (secondDistance < firstDistance)
                return model.distanceReward;
            else
                return model.distancePunish;
        }
    }
}