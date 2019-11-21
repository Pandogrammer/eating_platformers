using System;
using food;
using Foods;
using Games;
using Games.Messages;
using UnityEngine;

namespace Behaviours
{
    public class FoodDropBehaviour : MonoBehaviour
    {

        [SerializeField] private UnityFood foodPrefab;
        [SerializeField] private float cooldown;
        [SerializeField] private UnityGame unityGame;

        private float cooldownTimer = 0;
        private void Update()
        {
            if (cooldownTimer > 0) cooldownTimer -= Time.fixedDeltaTime;
        }

        public void TryToDrop(Vector3 position, Transform parent = null)
        {
            if (cooldownTimer > 0) return;
            
            var unityFood = Instantiate(foodPrefab, position, Quaternion.identity, parent);
            cooldownTimer = cooldown;
            SendDroppedFoodMessage(unityFood.GetHashCode(), unityFood.food);
        }
        
        private void SendDroppedFoodMessage(int foodHash, Food food)
        {
            var msg = new Tuple<GameMsg, GameParameters>
                (GameMsg.DroppedFood, new DroppedFood(foodHash, food));

            unityGame.messageQueue.Enqueue(msg);
        }
    }
}
