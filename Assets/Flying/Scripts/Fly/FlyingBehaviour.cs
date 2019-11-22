using UnityEngine;

namespace Flying.Scripts
{
    public class FlyingBehaviour
    {
        public Rigidbody Move(Rigidbody objectBody, float direction, float moveSpeed)
        {
            objectBody.AddForce(moveSpeed * direction * objectBody.transform.right, ForceMode.VelocityChange);
            return objectBody;
        }

        public Transform Rotate(Transform objectTransform, float direction, float rotationSpeed)
        {
            objectTransform.Rotate(Vector3.up * direction, rotationSpeed);
            return objectTransform;
        }

        public Rigidbody Impulse(Rigidbody objectBody, float impulseSpeed)
        {
            objectBody.AddForce(Vector3.up * impulseSpeed, ForceMode.VelocityChange);
            return objectBody;
        }
    }
}