using UnityEngine;

namespace Flying.Scripts
{
    public interface CollisionReceiver
    {
        void OnCollisionEnter(Collision other);
        void OnTriggerEnter(Collider other);
    }
}