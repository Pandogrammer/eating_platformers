using System;
using System.Collections.Generic;
using System.Linq;
using Chefs;
using Eaters;
using Eaters.Messages;
using food;
using Games.Messages;

namespace Games
{
    public class Game
    {
        public readonly int tick;
        public readonly Dictionary<int, Food> food;
        public readonly Dictionary<int, Eater> eaters;
        public readonly Dictionary<int, Chef> chefs;

        public Game(int tick
            , Dictionary<int, Food> food
            , Dictionary<int, Eater> eaters
            , Dictionary<int, Chef> chefs)
        {
            this.tick = tick;
            this.food = food;
            this.eaters = eaters;
            this.chefs = chefs;
        }

        public static Game Update(GameMsg gameMsg, Game game, GameParameters parameters = null)
        {
            switch (gameMsg)
            {
                case GameMsg.Tick:
                    return Tick(game);
                case GameMsg.CleanRottenFood:
                    return CleanRottenFood(game);
                case GameMsg.DecreaseEaterHunger:
                    return DecreaseEaterHunger(game, (DecreaseEaterHunger) parameters);
                case GameMsg.DroppedFood:
                    return DroppedFood(game, (DroppedFood) parameters);
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameMsg), gameMsg, null);
            }
        }

        private static Game DroppedFood(Game game, DroppedFood parameters)
        {
            var updatedFood = game.food;
            updatedFood.Add(parameters.foodHash, parameters.food);
            return new Game(game.tick, updatedFood, game.eaters, game.chefs);
        }

        private static Game DecreaseEaterHunger(Game game, DecreaseEaterHunger parameters)
        {
            var eaters = game.eaters;
            var updatedEater = Eater.Update(EaterMsg.DecreaseHunger, 
                eaters[parameters.eaterHash], 
                new DecreaseHunger(parameters.value));

            eaters[parameters.eaterHash] = updatedEater;
            
            return new Game(game.tick, game.food, eaters, game.chefs);
        }

        private static Game CleanRottenFood(Game game)
        {
            var allFood = game.food;
            var allChefs = game.chefs;
            var rottenFood = allFood.Where(x => x.Value.rotten).Select(x => x.Key).ToList();

            rottenFood.ForEach(i =>
            {
                allFood.Remove(i);
                allChefs = ModifyChefsHappiness(allChefs, -1);
            });
            
            
            return new Game(game.tick, allFood, game.eaters, allChefs);
        }

        private static Dictionary<int, Chef> ModifyChefsHappiness(Dictionary<int, Chef> allChefs, int value)
        {
            var updatedChefs = allChefs.ToDictionary(x => x.Key, x => x.Value);
            foreach (var chef in allChefs)
            {
                if(value < 0) updatedChefs[chef.Key] = Chef.Update(ChefMsg.DecreaseHappiness, chef.Value);
                if(value > 0) updatedChefs[chef.Key] = Chef.Update(ChefMsg.IncreaseHappiness, chef.Value);
            }

            return updatedChefs;
        }

        private static Game Tick(Game game)
        {
            var ticks = AddOneTick(game.tick);
            var food = RotFood(game.food);
            var eaters = IncreaseHunger(game.eaters);
            var chefs = ModifyChefsHappiness(eaters, game.chefs);
            
            return new Game(ticks, food, eaters, chefs);
        }

        private static Dictionary<int, Chef> ModifyChefsHappiness(Dictionary<int,Eater> eaters, Dictionary<int, Chef> chefs)
        {
            eaters.Values.ToList()
                .Where(x => x.state == EaterState.Sad)
                .ToList()
                .ForEach(x => chefs = ModifyChefsHappiness(chefs, -1));

            eaters.Values.ToList()
                .Where(x => x.state == EaterState.Happy)
                .ToList()
                .ForEach(x => chefs = ModifyChefsHappiness(chefs, +1));

            return chefs;
        }

        private static Dictionary<int, Eater> IncreaseHunger(Dictionary<int, Eater> eaters)
        {
            var updatedEaters = eaters.ToDictionary(x => x.Key, x => x.Value);
            foreach (var eater in eaters)
            {
                updatedEaters[eater.Key] = Eater.Update(EaterMsg.IncrementHunger, eater.Value);
            }

            return updatedEaters;
        }

        private static Dictionary<int, Food> RotFood(Dictionary<int, Food> allFood)
        {
            var updatedFood = allFood.ToDictionary(x => x.Key, x => x.Value);
            foreach (var food in allFood)
            {
                updatedFood[food.Key] = Food.Update(FoodMsg.Rot, food.Value);
            }

            return updatedFood;
        }

        private static int AddOneTick(int ticks)
        {
            return ticks + 1;
        }
    }
}