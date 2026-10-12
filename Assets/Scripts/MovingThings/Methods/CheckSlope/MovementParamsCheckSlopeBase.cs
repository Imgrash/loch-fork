using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementParamsCheckSlopeBase", menuName = "ScriptableObject/MovementParam/CheckSlopeBase")]
public class MovementParamsCheckSlopeBase : ScriptableObject
{
    public float sphereCastRadius;
    public float sphereCastDistance;
    public Vector3 rayOriginOffset1;
    public Vector3 rayOriginOffset2;
    public float raycastLength;
}
