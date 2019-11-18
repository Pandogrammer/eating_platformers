using Behaviours;
using food;
using Foods;
using UnityEngine;

namespace Chefs
{
    public class UnityChef : MonoBehaviour, Modeled<Chef>
    {
        [SerializeField] private int startingHappiness;

        private Rigidbody body;
        private Transform bodyTransform;
        private MovementBehaviour movement;
        private FoodDropBehaviour foodDrop;
    
        public Chef chef { get; private set; }
    
        public void Awake()
        {
            chef = new Chef(startingHappiness);
            body = GetComponentInParent<Rigidbody>();
            bodyTransform = GetComponentInParent<Transform>().parent;
            movement = GetComponent<MovementBehaviour>();
            foodDrop = GetComponent<FoodDropBehaviour>();
        }
    
        public void Move(float direction)
        {
            body = movement.Move(body, bodyTransform, direction);
        }

        public void Rotate(float rotateDir)
        {
            bodyTransform = movement.Rotate(bodyTransform, rotateDir);
        }

        public void DropFood()
        {
            var position = bodyTransform.position + bodyTransform.up * 3;
            foodDrop.TryToDrop(position);
        }

        public void UpdateModel(Chef chef)
        {
            this.chef = chef;
        }
    }
}
