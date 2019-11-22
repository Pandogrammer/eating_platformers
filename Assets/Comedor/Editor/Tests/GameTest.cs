using System.Collections.Generic;
using Chefs;
using Eaters;
using food;
using Games;
using Games.Messages;
using NUnit.Framework;

namespace Comedor.Editor.Tests
{
    public class ContextTest
    {
        private Game game;

        [SetUp]
        public void Setup()
        {
            game = new Game(0
                , new Dictionary<int, Food>()
                , new Dictionary<int, Eater>()
                , new Dictionary<int, Chef>());
        }

        [Test]
        public void tick_then_tick_increments()
        {
            var result = Game.Update(GameMsg.Tick, game);

            Assert.AreEqual(1, result.tick);
        }

        [Test]
        public void tick_then_food_rots()
        {
            var food = new Dictionary<int, Food> {{1, new Food(0, 0)}};
            game = new Game(game.tick, food, game.eaters, game.chefs);

            var result = Game.Update(GameMsg.Tick, game);

            Assert.AreEqual(1, result.food[1].rot);
        }

        [Test]
        public void clean_then_rotten_food_disappears()
        {
            var food = new Dictionary<int, Food>
            {
                {1, new Food(10, 1)},
                {2, new Food(0, 1)},
                {3, new Food(10, 1)}
            };
            game = new Game(game.tick, food, game.eaters, game.chefs);

            var result = Game.Update(GameMsg.CleanRottenFood, game);

            Assert.AreEqual(1, result.food.Count);
        }

        [Test]
        public void eat_then_hunger_decreases()
        {
            var eaters = new Dictionary<int, Eater>
            {
                {1, new Eater(10)},
                {2, new Eater(10)},
                {3, new Eater(10)}
            };
            var parameters = new DecreaseEaterHunger(2, 1);
            game = new Game(game.tick, game.food, eaters, game.chefs);

            var result = Game.Update(GameMsg.DecreaseEaterHunger, game, parameters);

            Assert.AreEqual(10, result.eaters[1].hunger);
            Assert.AreEqual(9, result.eaters[2].hunger);
            Assert.AreEqual(10, result.eaters[3].hunger);
        }

        [Test]
        public void dropped_food_then_food_appears()
        {
            var food = new Food(5, 10);
            var parameters = new DroppedFood(food.GetHashCode(), food);

            var result = Game.Update(GameMsg.DroppedFood, game, parameters);

            Assert.AreEqual(food, result.food[food.GetHashCode()]);
        }
    }
}