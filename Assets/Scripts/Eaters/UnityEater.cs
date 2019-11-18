using System;
using Behaviours;
using Foods;
using Games;
using Games.Messages;
using UnityEngine;

namespace Eaters
{
    public class UnityEater : MonoBehaviour, Modeled<Eater>
    {
        [SerializeField] private SphereCollider foodDetectionArea;
        [SerializeField] private int startingHunger;
        [SerializeField] private float foodEatingDistance;

        public Rigidbody body;
        public Transform bodyTransform;

        private MovementBehaviour movement;

        private FoodEatingBehaviour foodEating;
        public UnityFood detectedFood { get; private set; }

        public Eater eater { get; private set; }

        public void Awake()
        {
            eater = new Eater(startingHunger);
            body = GetComponentInParent<Rigidbody>();
            bodyTransform = GetComponentInParent<Transform>().parent;
            movement = GetComponent<MovementBehaviour>();
            foodEating = GetComponent<FoodEatingBehaviour>();
        }

        public void Update()
        {
            DetectFood();
        }

        public void UpdateModel(Eater eater)
        {
            this.eater = eater;
        }

        public void Move(float direction)
        {
            body = movement.Move(body, bodyTransform, direction);
        }

        public void Rotate(float rotateDir)
        {
            bodyTransform = movement.Rotate(bodyTransform, rotateDir);
        }

        public void DetectFood()
        {
            detectedFood = foodEating.DetectNearestFood(bodyTransform, foodDetectionArea);

            if (detectedFood != null)
            {
                Debug.DrawLine(bodyTransform.position, detectedFood.transform.position, Color.red);
            }
        }

        public void TryToEatFood()
        {
            if (detectedFood != null)
            {
                var foodPosition = detectedFood.transform.position;
                var position = bodyTransform.position;
                //si esta dentro de la distancia
                if (Vector3.Distance(position, foodPosition) <= foodEatingDistance)
                {
                    Debug.DrawLine(position, foodPosition, Color.green);
                    detectedFood = foodEating.Eat(detectedFood);
                    SendEatenMessage(GetHashCode(), foodEating.satietyValue);
                }
                else
                {
                    Debug.DrawLine(position, foodPosition, Color.red);
                }
            }
        }

        //POLEMICO, no se si esta bueno que el eater le avise al mundo, pero me da paja armar lo de unirx je
        private static void SendEatenMessage(int eaterHash, int value)
        {
            var msg = new Tuple<GameMsg, GameParameters>
            (GameMsg.DecreaseEaterHunger, new DecreaseEaterHunger(eaterHash, value));

            FindObjectOfType<UnityGame>().messageQueue.Enqueue(msg);
        }
    }
}