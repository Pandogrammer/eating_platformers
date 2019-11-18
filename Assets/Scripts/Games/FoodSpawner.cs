using System;
using System.Collections;
using System.Collections.Generic;
using Behaviours;
using Games.Messages;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] private SphereCollider spawningArea;
    [SerializeField] public float cooldown;
    [SerializeField] private GameObject foodPrefab;

    private float cooldownTimer = 0f;
    
    void Update()
    {
        TryToSpawnFood();
    }

    private void TryToSpawnFood()
    {
        if (OnCooldown()) return;

        var ang = Random.value * 360;
        var pos = new Vector3();
        var center = spawningArea.transform.position;
        var radius = Random.Range(2, spawningArea.radius);
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        DropFood(pos);
    }

    private void DropFood(Vector3 position)
    {
        Instantiate(foodPrefab, position, Quaternion.identity, transform.parent);
    }

    private bool OnCooldown()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer < cooldown || Math.Abs(cooldown) < 0.01f)
            return true;
        cooldownTimer = 0;
        return false;
    }

    public void Setup(float areaSize, float spawnerCooldown)
    {
        cooldown = spawnerCooldown;
        spawningArea.radius = areaSize;
    }
}