using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ManualSpawner : MonoBehaviour
{
    [SerializeField] private Collider collider;
    [SerializeField] private float cooldown;
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private Camera camera;

    private float cooldownTimer = 0f;
    
    void Update()
    {
        TryToSpawnFood();
    }

    private void TryToSpawnFood()
    {
        if (OnCooldown()) return;

        if(Input.GetMouseButton(0))
        {
            if (!camera.gameObject.activeSelf) return;
            
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out hit)) return;
            if (hit.collider != collider) return;

            var position = hit.point;
            DropFood(position);
        }
    }

    private void DropFood(Vector3 position)
    {
        Instantiate(foodPrefab, position, Quaternion.identity, transform.parent);
    }

    private bool OnCooldown()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer < cooldown)
            return true;
        cooldownTimer = 0;
        return false;
    }
}
