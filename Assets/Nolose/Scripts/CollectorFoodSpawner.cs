using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollectorFoodSpawner : MonoBehaviour
{
    [SerializeField] private SphereCollider spawningArea;
    [SerializeField] public float cooldown;
    [SerializeField] private GameObject foodPrefab;
    private Stack<GameObject> foodPool;

    public void Start()
    {
        SpawnInitialFood();
    }

    private void SpawnInitialFood()
    {
        foodPool = new Stack<GameObject>();
        for (int i = 0; i < 50; i++)
        {
            var food = Instantiate(foodPrefab, transform.parent);
            food.SetActive(false);
            foodPool.Push(food);
        }
    }

    public GameObject SpawnFood()
    {
        var pos = GenerateRandomPosition();
        var food = foodPool.Pop();
        food.transform.position = pos;
        food.transform.rotation = Quaternion.identity;
        food.SetActive(true);
        return food;
    }

    private Vector3 GenerateRandomPosition()
    {
        var ang = Random.value * 360;
        var pos = new Vector3();
        var center = spawningArea.transform.position;
        var radius = Random.Range(2, spawningArea.radius);
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        return pos;
    }

    public void UnspawnFood(GameObject food)
    {
        food.SetActive(false);
        foodPool.Push(food);
    }
}