﻿using UnityEngine;

namespace Behaviours
{
    public class MovementBehaviour : MonoBehaviour
    {
        [SerializeField] private float upImpulse;
        [SerializeField] private float upLimit;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationAngle;
    
        public Rigidbody Move(Rigidbody rigidbody, Transform parentTransform, float direction)
        {
            var up = rigidbody.velocity.y < upLimit ? upImpulse * parentTransform.up : Vector3.zero;
            var forward = direction * moveSpeed * parentTransform.forward;
            
            rigidbody.AddForce(up + forward, ForceMode.VelocityChange);
            return rigidbody;
        }

        public Transform Rotate(Transform transform, float rotateDir)
        {
            transform.Rotate(transform.up * rotateDir, rotationAngle);
            return transform;
        }
    }
}
