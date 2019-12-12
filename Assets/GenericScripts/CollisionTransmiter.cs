using UnityEngine;

namespace Flying.Scripts
{
    public class CollisionTransmiter : MonoBehaviour
    {
        [SerializeField] private GameObject receiver;

        private void OnTriggerEnter(Collider other)
        {
            receiver.GetComponent<CollisionReceiver>().OnTriggerEnter(other);
        }

        private void OnCollisionEnter(Collision other)
        {
            receiver.GetComponent<CollisionReceiver>().OnCollisionEnter(other);
        }
    }
}