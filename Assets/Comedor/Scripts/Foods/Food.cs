using System;

namespace food
{
    public class Food
    {
        public readonly int rot;
        public readonly int rotLimit;
        public bool rotten => rot >= rotLimit;

        public Food(int rot, int rotLimit)
        {
            this.rot = rot;
            this.rotLimit = rotLimit;
        }

        public static Food Update(FoodMsg foodMsg, Food food)
        {
            switch (foodMsg)
            {
                case FoodMsg.Rot:
                    return Rot(food);
                default:
                    throw new ArgumentOutOfRangeException(nameof(foodMsg), foodMsg, null);
            }
        }
        
        private static Food Rot(Food food)
        {
            var rot = food.rot + 1;
            
            return new Food(rot, food.rotLimit);
        }
    }

    public enum FoodMsg { Rot }
}