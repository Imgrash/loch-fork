using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementParamsHoldHeightBase", menuName = "ScriptableObject/MovementParam/HoldHeightBase")]
public class MovementParamsHoldHeightBase : ScriptableObject
{
    public float RideHeight;
    public Vector3 DownDir;
    public float RideSpringDamper;
    public float RideSpringStrength;
    public float WeightCenterMaxDistance;
}
