using System.Collections.Generic;
using System.Linq;
using Games.Machine;
using MLAgents;
using UnityEngine;

public class ArenaAcademy : Academy
{
    private List<ArenaArea> areas;
    public override void AcademyReset()
    {
        var areaSize = resetParameters["area_size"];
        var spawnerCooldown = resetParameters["spawner_cooldown"];
        areas = FindObjectsOfType<ArenaArea>().ToList();
        areas.ForEach(x =>
        {
            x.Setup(areaSize, spawnerCooldown);
            x.Reset();
        });
    }

}