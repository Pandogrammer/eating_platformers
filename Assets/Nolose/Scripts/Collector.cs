using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private float speed;
    [SerializeField] private float rotationAngle;
    [SerializeField] private EatingBehaviour eating;
    [SerializeField] public float hp;
    public bool IsDead => hp <= 0;
    public Rigidbody Body => body;

    public void TurnLeft()
    {
        if (IsDead) return;
        body.transform.Rotate(body.transform.up, -rotationAngle);
    }

    public void TurnRight()
    {
        if (IsDead) return;
        body.transform.Rotate(body.transform.up, rotationAngle);
    }

    public void GoBackward()
    {
        if (IsDead) return;
        body.AddForce(-body.transform.forward * speed, ForceMode.VelocityChange);
    }

    public void GoForward()
    {        
        if (IsDead) return;
        body.AddForce(body.transform.forward * speed, ForceMode.VelocityChange);
    }

    public void Eat()
    {
        if (IsDead) return;
        hp += eating.TryToEat();
    }
}