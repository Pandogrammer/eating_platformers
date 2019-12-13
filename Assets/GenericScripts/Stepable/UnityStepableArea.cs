using UnityEngine;

namespace GenericScripts.Stepable
{
    public abstract class UnityStepableArea : MonoBehaviour
    {
        public abstract void Step(int maxSteps);
    }
}