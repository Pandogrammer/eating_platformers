using System.Collections.Generic;
using System.Linq;
using MLAgents;

namespace Flying.Scripts.Fly.Unity
{
    public class UnityFlyingAcademy : Academy
    {
        private List<UnityFlyingArea> areas;
        private int maxSteps;
        public override void InitializeAcademy()
        {
            maxSteps = (int) resetParameters["max_steps"];
            areas = FindObjectsOfType<UnityFlyingArea>().ToList();
        }

        public override void AcademyStep()
        {
            areas.ForEach(x => x.Step(maxSteps));
        }
    }
}