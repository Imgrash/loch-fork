using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LochParams", menuName = "ScriptableObject/LochParams")]

public class LochParams : ScriptableObject
{
    //[Header("Base")]
    //public Transform Camera;

    //#region Input
    //[Header("Input")]
    //public InputManager inputManager;
    //public Transform respawnPoint;
    //#endregion


    #region moving
    [Header("Moving")]
    public LayerMask interactByMovingLayerMask;

    #region jump
    [Header("Jump")]
    public float jumpForce;
    public float fallMultiplier;
    public float lowJumpMultiplier;

    public float topJumpLength;
    public float jumpForceFromGrab;

    [Header("CoyoteTime")]
    public float coyoteTime;

    [Header("JumpBuffer")]
    public float jumpBufferTime;
    #endregion jump

    #region landing
    [Header("Landing")]
    public float minGroundCheckRadius;
    public float maxGroundCheckRadius;

    public LayerMask groundLayerMask;
    //public Transform TooCloseCheck;

    //public Transform groundChild;
    //public Transform groundChildCheckPosition;
    #endregion landing

    #region ledgegrab
    [Header("LedgeGrab")]
    public float forwardHangRayOffsetForward;
    public float forwardHangRayOffset;
    public float downwardHangRayOffsetForward;
    public float downwardHangRayOffset;

    public float raycastDistanceForwardHang;
    public float raycastDistanceDownwardHang;
    #endregion ledgegrab

    #region walk
    [Header("Walk")]
    public float maxSpeed;
    public float rotationSpeed;
    public float Acceleration;
    public AnimationCurve AccelerationFactorFromDot;
    public AnimationCurve MaxAccelerationForceFromFactorDot;
    public float MaxAccelForce;
    public Vector3 ForceScale;

    public float AirborneAcceleration;

    public float WeightCenterMaxDistance;
    //public Transform floatingRideHeightCheckTransform;
    #endregion walk

    #region holdHeight
    [Header("HoldHeight")]
    public float sphereLandCheckRadius;
    public float minRaycastLandCheckDistanceWhileJump;
    public float minRayCastLandCheckDistanceSetting;
    //public Transform rideHeightTransformPoint;
    //public Transform groundCheckTransformPoint;

    public Vector3 DownDir;
    public float RideSpringDamper;
    public float RideSpringStrength;
    public float RideHeight;
    #endregion holdHeight

    #region IK
    [Header("IK")]
    public float DistanceToGroundIK;
    #endregion IK

    #region checkSlope
    [Header("SlopeSettings")]
    public float startDistanceFromBottom;   // Should probably be higher than skin width
    public float sphereCastRadius;
    public float sphereCastDistance;       // How far spherecast moves down from origin point

    public float raycastLength;
    public Vector3 rayOriginOffset1;
    public Vector3 rayOriginOffset2;
    #endregion checkSlope

    #endregion moving


    //#region interacting
    //[Header("Interacting")]
    //public GameObject Mark;
    //#endregion interacting
}
