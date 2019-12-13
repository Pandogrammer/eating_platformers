using UnityEngine;

public class CollectorFoodSpawner : MonoBehaviour
{
    [SerializeField] private SphereCollider spawningArea;
    [SerializeField] public float cooldown;
    [SerializeField] private GameObject foodPrefab;

    public void Setup(SphereCollider spawningArea, float cooldown, GameObject foodPrefab)
    {
        this.spawningArea = spawningArea;
        this.cooldown = cooldown;
        this.foodPrefab = foodPrefab;
    }

    public GameObject SpawnFood()
    {
        var ang = Random.value * 360;
        var pos = new Vector3();
        var center = spawningArea.transform.position;
        var radius = Random.Range(2, spawningArea.radius);
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        return Instantiate(foodPrefab, pos, Quaternion.identity, transform.parent);
    }
}