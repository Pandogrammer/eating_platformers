using UnityEngine;

namespace GenericScripts.Stepable
{
    public abstract class UnityStepableArea : MonoBehaviour
    {
        public abstract void Step(int step, int maxSteps);

        public abstract void Reset();
    }
}