using System.Collections.Generic;
using System.Linq;
using MLAgents;
using UnityEngine;

namespace GenericScripts.Stepable
{
    public class UnityStepableAcademy : Academy
    {
        private List<UnityStepableArea> areas;
        private int maxSteps;
        private int agentStep = 0;
        public int step { get; private set; }
        public override void InitializeAcademy()
        {
            maxSteps = (int) resetParameters["max_steps"];
            areas = FindObjectsOfType<UnityStepableArea>().ToList();
        }

        public override void AcademyStep()
        {
            agentStep++;
            if (agentStep % 4 != 0) 
                return;

            agentStep = 0;
            StepAreas();
            step++;
            if (step > maxSteps) Reset();
        }

        private void Reset()
        {
            step = 0;
            ResetAreas();
        }

        private void StepAreas()
        {
            foreach (var area in areas)
            {
                area.Step(step, maxSteps);
            }
        }

        private void ResetAreas()
        {
            foreach (var area in areas)
            {
                area.Reset();
            }
        }

    }
}