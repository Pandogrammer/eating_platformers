using System.Collections.Generic;
using System.Linq;
using MLAgents;
using UnityEngine;

namespace Flying.Scripts.Float
{
    public class FloatingAcademy : Academy
    {
        private FloatingArea[] areas;
        private int maxSteps;
        private int step;

        public override void InitializeAcademy()
        {
            areas = FindObjectsOfType<FloatingArea>();
            maxSteps = (int) resetParameters["max_steps"];
        }


        private static List<FloatingArea> ResetAreas(List<FloatingArea> list)
        {
            list.ForEach(x => x.Reset());
            return list;
        }

        private static List<FloatingArea> TryToAdvanceAreas(List<FloatingArea> list)
        {
            if ((float) list.Count(x => x.MustAdvance()) / list.Count < 0.95f) return list;

            var impulse = Random.Range(1f, 2f);
            var maxY = Random.Range(2f, 10f);
            var minY = Random.Range(-10f, -2f);
            list.ForEach(x => x.ChangeArea(impulse, maxY, minY));
            return list;
        }

        public override void AcademyStep()
        {
            step += 1;
            areas.ToList().ForEach(x => x.Step(maxSteps));

            if (step > maxSteps) Reset();
        }

        private void Reset()
        {
            step = 0;
            var list = areas.ToList();
            list = TryToAdvanceAreas(list);
            list = ResetAreas(list);        }
    }
}