using UnityEngine;

namespace Flying.Scripts.Fly
{
    public static class FlyingArea
    {

        public static FlyingAreaModel Create(FlyingAgentModel agent, Vector3 target,
            float distanceReward, float distancePunish)
        {
            return new FlyingAreaModel(agent, target, distanceReward, distancePunish, 0);
        }

        public static FlyingAreaModel UpdateNextRewardBasedOnPosition(
            FlyingAreaModel model, Vector3 newPosition)
        {
            var nextReward = CalculateRewardBasedOnDistance(model, newPosition);
            return new FlyingAreaModel(model.agent, model.target, 
                model.distanceReward, model.distancePunish, nextReward);
        }

        public static FlyingAreaModel UpdateAgentModel(FlyingAreaModel model, FlyingAgentModel newModel)
        {
            return new FlyingAreaModel(newModel, model.target, model.distanceReward, model.distancePunish,
                model.nextReward);
        }

        public static FlyingAreaModel CheckIfAgentDone(FlyingAreaModel model)
        {
            return new FlyingAreaModel(model.agent, model.target, model.distanceReward, model.distancePunish,
                model.nextReward, Validate(model));
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

    public class FlyingAreaModel
    {
        public readonly FlyingAgentModel agent;
        public readonly Vector3 target;
        public readonly float distanceReward;
        public readonly float distancePunish;
        public readonly float nextReward;
        public readonly bool done;

        protected internal FlyingAreaModel(FlyingAgentModel agent, Vector3 target, float distanceReward,
            float distancePunish, float nextReward, bool done = false)
        {
            this.agent = agent;
            this.target = target;
            this.distanceReward = distanceReward;
            this.distancePunish = distancePunish;
            this.nextReward = nextReward;
            this.done = done;
        }
    }
}