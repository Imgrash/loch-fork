using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementParamsGroundCheckBase", menuName = "ScriptableObject/MovementParam/GroundCheckBase")]
public class MovementParamsGroundCheckBase : ScriptableObject
{
    public float minRayCastLandCheckDistanceSetting;
    public float minRaycastLandCheckDistanceWhileJump;
    public float minGroundCheckRadius;
    public float maxGroundCheckRadius;
}
