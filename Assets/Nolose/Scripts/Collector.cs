using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private float speed;
    [SerializeField] private float rotationAngle;
    [SerializeField] private EatingBehaviour eating;
    [SerializeField] public float hp;

    public bool justEeaten;
    public bool IsDead => hp <= 0;
    public Rigidbody Body => body;

    public void TurnLeft()
    {
        body.transform.Rotate(body.transform.up, -rotationAngle);
    }

    public void TurnRight()
    {
        body.transform.Rotate(body.transform.up, rotationAngle);
    }

    public void GoBackward()
    {
        body.AddForce(-body.transform.forward * speed, ForceMode.VelocityChange);
    }

    public void GoForward()
    {
        body.AddForce(body.transform.forward * speed, ForceMode.VelocityChange);
    }

    public void Eat()
    {
        var foodEaten = eating.TryToEat();
        hp += foodEaten;
        if (foodEaten > 0)
            justEeaten = true;
    }
}