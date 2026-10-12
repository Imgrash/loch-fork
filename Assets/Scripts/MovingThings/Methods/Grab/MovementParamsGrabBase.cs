using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementParamsGrabBase", menuName = "ScriptableObject/MovementParam/GrabBase")]
public class MovementParamsGrabBase : ScriptableObject
{
    public float forwardHangRayOffsetForward;
    public float forwardHangRayOffset;
    public float downwardHangRayOffsetForward;
    public float downwardHangRayOffset;
    public float raycastDistanceForwardHang;
    public float raycastDistanceDownwardHang;
}
