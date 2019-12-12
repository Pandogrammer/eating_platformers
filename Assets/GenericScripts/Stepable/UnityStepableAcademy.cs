using System.Collections.Generic;
using System.Linq;
using Flying.Scripts.Stepable;
using MLAgents;

namespace Flying.Scripts.Fly.Unity
{
    public class UnityStepableAcademy : Academy
    {
        private List<UnityStepableArea> areas;
        private int maxSteps;
        public override void InitializeAcademy()
        {
            maxSteps = (int) resetParameters["max_steps"];
            areas = FindObjectsOfType<UnityStepableArea>().ToList();
        }

        public override void AcademyStep()
        {
            areas.ForEach(x => x.Step(maxSteps));
        }
    }
}