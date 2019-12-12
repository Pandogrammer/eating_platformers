using System.Collections.Generic;
using System.Linq;
using Flying.Scripts.Stepable;

public class CollectingArea : UnityStepableArea
{
    private List<AgentCollector> agentCollectors;

    private void Start()
    {
        agentCollectors = FindObjectsOfType<AgentCollector>().ToList();
    }

    public override void Step(int maxSteps)
    {
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
    }
}
