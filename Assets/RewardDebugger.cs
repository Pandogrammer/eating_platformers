using System.Collections;
using System.Collections.Generic;
using Flying.Scripts.Fly.Unity;
using UnityEngine;

public class RewardDebugger : MonoBehaviour
{
    [SerializeField] private UnityFlyingAgent[] agents;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < agents.Length; i++)
        {
            if(agents[i] != null)
                Debug.Log("["+i+"]: "+agents[i].GetCumulativeReward());
        }
    }
}
