using System;
using System.Collections.Generic;
using System.Linq;
using Chefs;
using Eaters;
using food;
using Foods;
using Games.Messages;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Object = UnityEngine.Object;

namespace Games
{
    public class UnityGame : MonoBehaviour
    {
        public Game game { get; private set; }

        
        public Queue<Tuple<GameMsg, GameParameters>> messageQueue;
        
        [SerializeField] private float debugTime;
        private float debugTimer;
        
        private UnityEater[] eaters;
        private UnityChef[] chefs;
        private UnityFood[] food;


        private void Init()
        {
            food = GetComponentsInChildren<UnityFood>();
            food.ToList().ForEach(x => x.Awake());
            
            eaters = GetComponentsInChildren<UnityEater>();
            eaters.ToList().ForEach(x => x.Awake());
            
            chefs = GetComponentsInChildren<UnityChef>();
            chefs.ToList().ForEach(x => x.Awake());
            

            game = new Game(tick: 0,
                food: food.ToDictionary(x => x.GetHashCode(), x => x.food),
                eaters: eaters.ToDictionary(x => x.GetHashCode(), x => x.eater),
                chefs: chefs.ToDictionary(x => x.GetHashCode(), x => x.chef)
            );

            messageQueue = new Queue<Tuple<GameMsg, GameParameters>>();
        }

        public void Start()
        {
            Init();
        }
        public void Restart()
        {
            Init();
        }

        public void Update()
        {
            if (debugTimer >= debugTime)
            {
                game = UpdateGame(game);
                debugTimer = 0;
            }

            debugTimer = IncreaseTimer(debugTimer);
        }


        private Game UpdateGame(Game game)
        {
            game = ProcessAndClearQueue(game);
            game = TickAndUpdateModels(game);
            game = CleanRottenFood(game);
            return game;
        }

        private Game TickAndUpdateModels(Game game)
        {
            var updatedGame = Tick(game);
            food = UpdateModels(food, updatedGame.food);
            eaters = UpdateModels(eaters, updatedGame.eaters);
            chefs = UpdateModels(chefs, updatedGame.chefs);
            return updatedGame;
        }

        private Game ProcessAndClearQueue(Game game)
        {
            var updatedGame = ProcessMessages(game, messageQueue);
            messageQueue.Clear();
            return updatedGame;
        }

        private static Game ProcessMessages(Game game
            , Queue<Tuple<GameMsg, GameParameters>> queue)
        {
            while (queue.Count > 0)
            {
                var msg = queue.Dequeue();
                game = Game.Update(msg.Item1, game, msg.Item2);
            }

            return game;
        }

        private static Game CleanRottenFood(Game game)
        {
            return Game.Update(GameMsg.CleanRottenFood, game);
        }

        private entity[] UpdateModels<entity, model>(entity[] entities, Dictionary<int, model> models) where entity : Object, Modeled<model>
        {
            var updatedEntities = entities.ToDictionary(x => x.GetHashCode(), x => x);

            foreach (var e in updatedEntities)
            {
                updatedEntities[e.Key].UpdateModel(models[e.Key]);
            }

            return updatedEntities.Values.ToArray();
        }

        private float IncreaseTimer(float timer)
        {
            return timer + Time.deltaTime;
        }

        private static Game Tick(Game game)
        {
            return Game.Update(GameMsg.Tick, game);
        }
    }
}