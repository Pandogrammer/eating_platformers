using food;
using NUnit.Framework;

namespace Editor.Tests
{
    public class FoodTest
    {
        private Food food;
        
        [Test]
        public void food_rots_then_rot_increases()
        {
            food = new Food(0, 10);

            var result = Food.Update(FoodMsg.Rot, food);

            Assert.AreEqual(1, result.rot);
            Assert.AreEqual(10, result.rotLimit);
        }

        [Test]
        public void rot_reaches_limit_then_food_rottens()
        {
            food = new Food(0, 1);

            var result = Food.Update(FoodMsg.Rot, food);

            Assert.IsTrue(result.rotten);
        }
    }
}