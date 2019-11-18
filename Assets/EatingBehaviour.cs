using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingBehaviour : MonoBehaviour
{
    [SerializeField] private Collider eatingArea;
    
    public int TryToEat()
    {
        Collider[] colliders = new Collider[10];
        var bounds = eatingArea.bounds;
        int size = Physics.OverlapBoxNonAlloc(eatingArea.transform.localPosition + bounds.center, 
            bounds.extents, colliders);
        var foodEaten = 0;
        for (int i = 0; i < size; i++)
        {
            //si es comida
            var collider = colliders[i];
            if (collider.CompareTag("food"))
            {
                foodEaten++;
                Destroy(collider.gameObject);
            }
        }

        return foodEaten;
    }

}
