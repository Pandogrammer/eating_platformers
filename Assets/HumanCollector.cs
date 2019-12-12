using UnityEngine;

public class HumanCollector : MonoBehaviour
{
    [SerializeField] private Collector collector;
    void Update()
    {
        if (Keys.FORWARD) collector.GoForward();
        if (Keys.BACKWARD) collector.GoBackward();
        if (Keys.RIGHT) collector.TurnRight();
        if (Keys.LEFT) collector.TurnLeft();
        if (Keys.EAT) collector.Eat();
    }

    internal static class Keys
    {
        public static bool EAT => Input.GetKey(KeyCode.Space);
        public static bool FORWARD => Input.GetKey(KeyCode.W);
        public static bool BACKWARD => Input.GetKey(KeyCode.S);
        public static bool LEFT => Input.GetKey(KeyCode.A);
        public static bool RIGHT => Input.GetKey(KeyCode.D);
    }
}