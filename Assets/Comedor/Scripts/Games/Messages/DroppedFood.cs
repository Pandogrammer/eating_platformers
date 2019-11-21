using food;

namespace Games.Messages
{
    public class DroppedFood : GameParameters
    {
        public readonly int foodHash;
        public readonly Food food;

        public DroppedFood(int foodHash, Food food)
        {
            this.foodHash = foodHash;
            this.food = food;
        }
    }
}