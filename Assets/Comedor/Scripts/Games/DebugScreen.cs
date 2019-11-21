using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chefs;
using Eaters;
using Games;
using TMPro;
using UnityEngine;

public class DebugScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameState;
    private List<UnityEater> eaters;
    private List<UnityChef> chefs;
    private UnityGame game;

    private void Start()
    {
        eaters = FindObjectsOfType<UnityEater>().ToList();
        chefs = FindObjectsOfType<UnityChef>().ToList();
        game = FindObjectOfType<UnityGame>();
    }

    private void Update()
    {
        var text = new StringBuilder();

        text = AddTickText(text, game.game.tick);
        text = AddEatersState(text, eaters);
        text = AddChefsState(text, chefs);

        gameState.text = text.ToString();
    }

    private static StringBuilder AddChefsState(StringBuilder message, List<UnityChef> chefs)
    {
        chefs.Select(x => "C: " + x.chef.state + " - Happiness: " + x.chef.happiness)
            .ToList()
            .ForEach(x => message.AppendLine(x));

        return message;
    }

    private static StringBuilder AddTickText(StringBuilder message, int tick)
    {
        return message.AppendLine("Tick: " + tick);
    }

    private static StringBuilder AddEatersState(StringBuilder message, List<UnityEater> eaters)
    {
        eaters.Select(x => "E: " + x.eater.state + " - Happiness: " + x.eater.happiness)
            .ToList()
            .ForEach(x => message.AppendLine(x));

        return message;
    }
}