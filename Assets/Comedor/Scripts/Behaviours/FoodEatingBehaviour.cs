using System.Collections.Generic;
using food;
using Foods;
using JetBrains.Annotations;
using UnityEngine;

namespace Behaviours
{
    public class FoodEatingBehaviour : MonoBehaviour
    {
        [SerializeField] public int satietyValue;
        
        public UnityFood DetectNearestFood(Transform transform, SphereCollider area)
        {
            Collider[] colliders = new Collider[10];
            int size = Physics.OverlapSphereNonAlloc(transform.position + area.center, area.radius, colliders);
            var closestDistance = area.radius + 1;
            UnityFood selectedFood = null;
            for (int i = 0; i < size; i++)
            {
                //si es comida
                var collider = colliders[i];
                if (collider.CompareTag("food"))
                {
                    //si es la mas cercana
                    var food = collider.GetComponent<UnityFood>();
                    var distance = GetDistance(transform, food);
                    if (distance < closestDistance)
                    {
                        selectedFood = food;
                    }
                }
            }
            
            return selectedFood;
        }

        public UnityFood Eat(UnityFood food)
        {
            Destroy(food.gameObject);
            return null;
        }

        private static float GetDistance(Transform transform, UnityFood food)
        {
            return Vector3.Distance(food.transform.position, transform.position);
        }

    }
}
