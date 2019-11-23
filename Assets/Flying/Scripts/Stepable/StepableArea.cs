using System.Collections.Generic;
using MLAgents;

namespace Flying.Scripts.Stepable
{
    public interface StepableArea
    {
        void Step();
        void Reset();
        void Setup(int maxSteps);
    }
}