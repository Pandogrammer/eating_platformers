using System;
using System.Collections.Generic;
using System.Linq;
using MLAgents;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Platformer
{
    public class PlatformerArea : Area
    {
        private PlatformerAgent[] agents;
        private Vector3[] initialPositions;
        private void Awake()
        {
            agents = GetComponentsInChildren<PlatformerAgent>().ToArray();
            initialPositions = agents.Select(x => x.transform.parent.transform.localPosition).ToArray();
        }

        public override void ResetArea()
        {
            DestroyFood();
            ResetAgentPosition();
        }

        private void DestroyFood()
        {
            var objects = GetComponentsInChildren<Transform>()
                .Where(x => x.CompareTag("food"))
                .Select(x => x.gameObject);
            foreach (var food in objects)
            {
                Destroy(food);
            }
        }

        private void ResetAgentPosition()
        {

            var randomPos = Random.Range(0, initialPositions.Length);
            for (var i = 0; i < agents.Length; i++)
            {
                agents[i].transform.parent.transform.localPosition = initialPositions[(i + randomPos) % initialPositions.Length];
                agents[i].transform.parent.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
            }
        }
    }
}