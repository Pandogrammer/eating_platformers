using UnityEngine;

public interface CollisionReceiver
{
    void OnCollisionEnter(Collision other);
    void OnTriggerEnter(Collider other);
}