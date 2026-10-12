using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementParamsJumpBase", menuName = "ScriptableObject/MovementParam/JumpBase")]
public class MovementParamsJumpBase : ScriptableObject
{
    public float topJumpLength;
    public float fallMultiplier;
    public float lowJumpMultiplier;
    public float jumpForceFromGrab;
    public float jumpForce;
    public float jumpBufferTime;
}
