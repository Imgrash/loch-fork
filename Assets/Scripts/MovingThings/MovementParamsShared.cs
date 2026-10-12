using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementParamsShared", menuName = "ScriptableObject/MovementParam/Shared")]
public class MovementParamsShared : ScriptableObject
{
    public LayerMask groundLayerMask;
    public float coyoteTime;
    public float sphereLandCheckRadius;
    public LayerMask interactByMovingLayerMask;
    public float maxSpeed;
}
