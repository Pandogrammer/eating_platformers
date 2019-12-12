using System.Collections.Generic;
using System.Linq;
using Flying.Scripts.Stepable;
using UnityEngine;

public class CollectingArea : UnityStepableArea
{
    private List<AgentCollector> agentCollectors;
    private int step;

    private void Start()
    {
        agentCollectors = FindObjectsOfType<AgentCollector>().ToList();
    }

    public override void Step(int maxSteps)
    {
        step++;
        foreach (var agent in agentCollectors.Where(agent => !agent.IsDone()))
        {
            //step
            agent.AddReward(-1F / maxSteps);
            //dead
            if (agent.Collector.IsDead)
            {
                agent.SetReward(-2F);
                agent.Done();
            }

            //eaten
            if (agent.Collector.justEeaten)
            {
                agent.AddReward(0.1F);
                agent.Collector.justEeaten = false;
            }
        }

        if (step > maxSteps) Reset();
    }

    private void Reset()
    {
        var objects = GetComponentsInChildren<Transform>()
            .Where(x => x.CompareTag("food"))
            .Select(x => x.gameObject);
        foreach (var food in objects)
        {
            Destroy(food);
        }

        foreach (var agent in agentCollectors)
        {
            agent.Collector.hp = 10;
            agent.AgentReset();
        }
    }
}