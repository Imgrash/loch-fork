using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementParamsWalkBase", menuName = "ScriptableObject/MovementParam/WalkBase")]
public class MovementParamsWalkBase : ScriptableObject
{
    public float rotationSpeed;
    public float Acceleration;
    public float AirborneAcceleration;
    public AnimationCurve AccelerationFactorFromDot;
    public float MaxAccelForce;
    public AnimationCurve MaxAccelerationForceFromFactorDot;
    public Vector3 ForceScale;
}
