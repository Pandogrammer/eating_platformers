using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hp;
    [SerializeField] private Collector collector;

    void Update()
    {
        hp.text = collector.hp.ToString("0.00");
    }
}
