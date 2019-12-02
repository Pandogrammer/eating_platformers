using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRotation : MonoBehaviour
{
    [SerializeField] private Transform follow;
    [SerializeField] private bool FreezeX;
    [SerializeField] private bool FreezeY;
    [SerializeField] private bool FreezeZ;

    private Quaternion originalLocalRotation;

    private void Awake() {
        originalLocalRotation = follow.localRotation;
    }

    private void Update()
    {
        if(!FreezeX)
            follow.RotateAround(follow.localPosition, follow.right, -originalLocalRotation.eulerAngles.x);
        if(!FreezeY)
            follow.RotateAround(follow.localPosition, follow.up, -originalLocalRotation.eulerAngles.y);
        if(!FreezeZ)
            follow.RotateAround(follow.localPosition, follow.forward, -originalLocalRotation.eulerAngles.z);
        
        transform.rotation = follow.localRotation;
        follow.localRotation = originalLocalRotation;
    }
    
}
