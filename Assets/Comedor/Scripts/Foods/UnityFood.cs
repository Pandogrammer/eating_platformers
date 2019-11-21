using food;
using UnityEngine;

namespace Foods
{
    public class UnityFood : MonoBehaviour, Modeled<Food>
    {
        [SerializeField] private int initialRot;
        [SerializeField] private int initialRotLimit;
    
        public Food food { get; private set; }
        public void Awake()
        {
            food = new Food(initialRot, initialRotLimit);
        }

        public void UpdateModel(Food food)
        {
            this.food = food;
            if(food.rotten) Destroy(gameObject);
        }
        
    }
}
