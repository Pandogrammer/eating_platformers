using MLAgents;

namespace Flying.Scripts
{
    public class FlyingAcademy : Academy
    {
        private FlyingArea[] areas;
        private int maxSteps;
        private int step;

        public override void InitializeAcademy()
        {
            areas = FindObjectsOfType<FlyingArea>();
            maxSteps = (int) resetParameters["max_steps"];
        }

        public override void AcademyReset()
        {
            step = 0;
            foreach (var flyingArea in areas)
            {
                flyingArea.ResetArea();
            }
        }

        public override void AcademyStep()
        {
            step += 1;
            foreach (var flyingArea in areas)
            {
                flyingArea.Step(maxSteps);
            }

            if (step > maxSteps)
            {
                AcademyReset();
            }
        }
    }
}
