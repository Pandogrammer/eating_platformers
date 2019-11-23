using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowChildPosition : MonoBehaviour
{
    [SerializeField] private Transform follow;
    [SerializeField] private bool FreezeX;
    [SerializeField] private bool FreezeY;
    [SerializeField] private bool FreezeZ;
    private Vector3 originalLocalPosition;

    private void Awake()
    {
        originalLocalPosition = follow.localPosition;
    }

    private void Update()
    {
//        var followLocalPosition = follow.localPosition;
//        var x = FollowAxis(followLocalPosition.x, originalLocalPosition.x, FreezeX);
//        var y = FollowAxis(followLocalPosition.y, originalLocalPosition.y, FreezeY);
//        var z = FollowAxis(followLocalPosition.z, originalLocalPosition.z, FreezeZ);
//        
//        follow.localPosition = new Vector3(x[0], y[0], z[0]);
//        originalLocalPosition = new Vector3(x[1], y[1], z[1]);

        if (follow.localPosition == originalLocalPosition) return;
        
        transform.localPosition += follow.localPosition - originalLocalPosition;
        follow.localPosition = originalLocalPosition;
    }

    private float[] FollowAxis(float followPosition, float originalPosition, bool Freeze)
    {
        if (Mathf.Abs(followPosition - originalPosition) < 0.01f) return FollowAxisData(followPosition, originalPosition);
        var position = followPosition;
        
        position += -originalPosition;

        return FollowAxisData(originalPosition, position);
    }

    float[] FollowAxisData(float newFollowPosition, float newLocalPosition) => new float[2]
        {newFollowPosition, newLocalPosition};
}