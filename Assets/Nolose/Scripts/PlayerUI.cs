using System.Collections;
using System.Collections.Generic;
using GenericScripts.Stepable;
using Nolose.Scripts;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hp;
    [SerializeField] private TextMeshProUGUI step;
    [SerializeField] private Collector collector;
    [SerializeField] private CollectingArea area;

    void Update()
    {
        hp.text = "HP: "+collector.hp.ToString("0.0000");
        step.text = "STEP: "+area.step;
    }
}
