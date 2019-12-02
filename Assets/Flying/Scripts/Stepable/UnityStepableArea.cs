using System.Collections.Generic;
using MLAgents;
using UnityEngine;

namespace Flying.Scripts.Stepable
{
    public abstract class UnityStepableArea : MonoBehaviour
    {
        public abstract void Step(int maxSteps);
    }
}