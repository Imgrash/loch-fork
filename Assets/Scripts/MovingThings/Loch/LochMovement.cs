using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Animator))]
public class LochMovement : MonoBehaviour
{
    [SerializeField]
    protected bool ShowDebugVariables = false;
    private Animator animator;

    [Header("Base")]
    public LochParams lochParams;
    public Transform Camera;
    private Rigidbody rb;

    private void SetInputSubscription()
    {
        inputManager.setInputAxisEvent              += SetMovementAxis;
        inputManager.defineJumpInputEvent           += defineJumpInput;
        inputManager.defineIsInteractingInputEvent  += defineIsInteracting;
        inputManager.defineIsRespawningInputEvent   += defineIsRespawning;
    }

    private void UnSetInputSubscription()
    {
        inputManager.setInputAxisEvent              -= SetMovementAxis;
        inputManager.defineJumpInputEvent           -= defineJumpInput;
        inputManager.defineIsInteractingInputEvent  -= defineIsInteracting;
        inputManager.defineIsRespawningInputEvent   -= defineIsRespawning;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rbPlatformWas = groundChild.position;
        GroundChildJoint = groundChild.GetComponent<FixedJoint>();
        SetInputSubscription();
        minRaycastLandCheckDistance = lochParams.minRayCastLandCheckDistanceSetting;
        groundCheckRadius = lochParams.minGroundCheckRadius;
    }

    #region Input
    [Header("Input")]
    [ContextMenuItem("Reset input subscription", "ResetInputSubscription")]
    public InputManager inputManager;
    public void ResetInputSubscription()
    {
        UnSetInputSubscription();
        SetInputSubscription();
    }
    private void SetMovementAxis()
    {
        curInputAxis = inputManager.CurInputAxis;
        if (curInputAxis.magnitude > 0f)
        {
            movementAngle = Mathf.Atan2(curInputAxis.x, curInputAxis.y) * Mathf.Rad2Deg + Camera.eulerAngles.y;
            m_UnitGoal = Quaternion.Euler(0f, movementAngle, 0f) * Vector3.forward;
            m_UnitGoal.Normalize();
        }
        else
            m_UnitGoal = Vector3.zero;
    }
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private bool jumpPressed = false;
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private bool jumpDown = false;
    private void defineJumpInput()
    {
        jumpPressed = inputManager.JumpPressed;
        if (inputManager.JumpDown)
        {
            stopJumpBufferTime();
            jumpDown = true;
            //readJumpInput = false;
            startJump();
        }
    }
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField] 
    private bool isInteracting = false;
    public void defineIsInteracting() => isInteracting = inputManager.IsInteracting;
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private bool isRespawning = false;
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    public Transform respawnPoint;
    public void defineIsRespawning()
    {
        isRespawning = inputManager.IsRespawning;
        if (isRespawning)
            transform.position = respawnPoint.position;
    }
    #endregion


    #region moving
    private void FixedUpdate()
    {
        if (!grab)
            TryGrab();
        if (!grab)
        {
            GroundCheck();
            if (jump)
                ProlongJump();
            JumpCut();

            Walk();
            CheckSlope();
            HoldHeight();

            Interacting();
        }
        moveWeightCenter();
    }

    [Header("Moving")]
    #region jump
    [Header("Jump")]
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private bool jump;
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private bool canJump = false;
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private float topJumpTimer = 0f;

    public bool isJump = false;

    public void JumpCut()
    {
        if (isJump && topJumpTimer < lochParams.topJumpLength)
            topJumpTimer += Time.fixedDeltaTime;

        if (rb.velocity.y < 0 && !isGroundedCloser)
            rb.velocity += Vector3.up * Physics.gravity.y * (lochParams.fallMultiplier - 1) * Time.fixedDeltaTime;
        else if (rb.velocity.y > 0 && isJump && (!jumpPressed || topJumpTimer > lochParams.topJumpLength))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lochParams.lowJumpMultiplier - 1) * Time.fixedDeltaTime;
            if (groundCheckRadius != lochParams.maxGroundCheckRadius)
                groundCheckRadius = lochParams.maxGroundCheckRadius;
        }
    }
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    //private bool readJumpInput = true;


    public void startJump()
    {
        if (jumpDown)
        {
            if (canJump && !isJump)
            {
                stopJumpBufferTime();
                jump = true;
                if (grab)
                {
                    grab = false;
                    isGroundChildSet = false;
                    GroundChildJoint.connectedBody = null;
                    rb.constraints = RigidbodyConstraints.FreezeRotation;
                    rb.AddForce(Vector3.up * lochParams.jumpForceFromGrab, ForceMode.Impulse);
                }
            }
            else if (jumpBufferTimeTimer == 0 && !canJump)
                currentCoroutineJumpBuffer = startJumpBufferTime();
        }
    }
    public void ProlongJump()
    {
        minRaycastLandCheckDistance = lochParams.minRaycastLandCheckDistanceWhileJump;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * lochParams.jumpForce, ForceMode.Impulse);

        isJump = true;
        jump = false;
    }

    [Header("CoyoteTime")]
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private float coyoteTimeTimer = 0f;

    private Coroutine currentCoroutineCoyoteTime;
    private Coroutine startCoyoteTime() => StartCoroutine(coyoteTimeCoroutine());
    private void coyoteTimeStoppedEffect()
    {
        coyoteTimeTimer = 0;
        rb.useGravity = true;
    }
    private void stopCoyoteTime()
    {
        if (coyoteTimeTimer > 0 && coyoteTimeTimer < lochParams.coyoteTime)
            StopCoroutine(currentCoroutineCoyoteTime);
        coyoteTimeStoppedEffect();
    }
    IEnumerator coyoteTimeCoroutine()
    {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        while (coyoteTimeTimer < lochParams.coyoteTime && !isJump && !isGroundedCloser)
        {
            coyoteTimeTimer += Time.deltaTime;
            yield return null;
        }
        canJump = false;
        coyoteTimeStoppedEffect();
    }

    [Header("JumpBuffer")]
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private float jumpBufferTimeTimer = 0f;

    private Coroutine currentCoroutineJumpBuffer;
    private Coroutine startJumpBufferTime() => StartCoroutine(jumpBufferTimeCoroutine());
    private void jumpBufferTimeStoppedEffect()
    {
        jumpDown = false;
        //readJumpInput = true;
        jumpBufferTimeTimer = 0;
    }
    private void stopJumpBufferTime()
    {
        if (jumpBufferTimeTimer > 0)
            StopCoroutine(currentCoroutineJumpBuffer);
        jumpBufferTimeStoppedEffect();
    }
    IEnumerator jumpBufferTimeCoroutine()
    {
        while (jumpBufferTimeTimer < lochParams.jumpBufferTime)
        {
            jumpBufferTimeTimer += Time.deltaTime;
            yield return null;
        }
        jumpBufferTimeStoppedEffect();
    }
    #endregion jump

    #region landing
    [Header("Landing")]
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private bool isGroundedCloser = false;
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private float groundCheckRadius = 0.08f;

    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    public Transform TooCloseCheck;
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private bool isTooClose = false;

    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private bool IsGroundedCloserCash = false;

    public Transform groundChild;
    public Transform groundChildCheckPosition;
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private Vector3 groundChildDifPosition = Vector3.zero;
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private bool isGroundChildSet = false;
    public void SetGroundChild(Transform parent, Vector3 whereToPut)
    {
        groundChild.SetParent(parent);
        groundChildDifPosition = whereToPut - groundChild.position;
        groundChild.position = whereToPut;
        isGroundChildSet = true;
    }

    public void GroundCheck()
    {
        isGroundChildSet = false;
        Collider[] hitColliders = Physics.OverlapSphere(groundChildCheckPosition.position, groundCheckRadius, lochParams.groundLayerMask);
        if (hitColliders.Length > 0)
            SetGroundChild(hitColliders[0].transform, groundChildCheckPosition.position);

        isGroundedCloser = Physics.CheckCapsule(groundCheckTransformPoint.position, groundCheckTransformPoint.position - new Vector3(0, -groundCheckRadius, 0), lochParams.sphereLandCheckRadius, lochParams.groundLayerMask);
        isTooClose = Physics.CheckSphere(TooCloseCheck.position, groundCheckRadius, lochParams.groundLayerMask);

        if (isGroundedCloser)
        {
            stopCoyoteTime();
            minRaycastLandCheckDistance = lochParams.minRayCastLandCheckDistanceSetting;
            if (isJump)
            {
                isJump = false;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                groundCheckRadius = lochParams.minGroundCheckRadius;
                topJumpTimer = 0;
            }
            canJump = true;
        }
        else if (coyoteTimeTimer == 0 && !isJump && Pit && IsGroundedCloserCash)
        {
            currentCoroutineCoyoteTime = startCoyoteTime();
            IsGroundedCloserCash = false;
            Pit = false;
        }
        else if (isJump)
            canJump = false;
        if (isGroundedCloser)
            IsGroundedCloserCash = isGroundedCloser;
    }
    #endregion landing

    #region ledgegrab
    [Header("LedgeGrab")]
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private Vector3 hangRayForwardStartPosRelToTransform = Vector3.zero;
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private Vector3 hangRayForwardDirection = Vector3.zero;

    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private Vector3 hangRayDownwardStartPosRelToTransform = Vector3.zero;
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private bool grab = false;
    private FixedJoint GroundChildJoint;
    public void TryGrab()
    {
        RaycastHit _forwardHangHit, _downwardHangHit;
        if (m_UnitGoal.magnitude > 0)
            //надо, чтобы стоящий на месте перс тоже мог зацепиться (иначе у него нет вектора направления, куда цепляться)
            m_UnitGoalCashed = m_UnitGoal;

        hangRayForwardStartPosRelToTransform = lochParams.forwardHangRayOffsetForward * m_UnitGoalCashed.normalized;
        hangRayForwardStartPosRelToTransform.y += lochParams.forwardHangRayOffset;

        hangRayDownwardStartPosRelToTransform = m_UnitGoalCashed.normalized * lochParams.downwardHangRayOffsetForward;
        hangRayDownwardStartPosRelToTransform.y += lochParams.downwardHangRayOffset;

        hangRayForwardDirection = m_UnitGoalCashed.normalized;
        Debug.DrawRay(transform.position + hangRayForwardStartPosRelToTransform, hangRayForwardDirection * lochParams.raycastDistanceForwardHang);
        Debug.DrawRay(transform.position + hangRayDownwardStartPosRelToTransform, Vector3.up * -lochParams.raycastDistanceDownwardHang);
        if (!isGroundedCloser && rb.velocity.y < 0 && !jump)
        {
            if (Physics.SphereCast(transform.position + hangRayForwardStartPosRelToTransform, 0.01f, hangRayForwardDirection, out _forwardHangHit, lochParams.raycastDistanceForwardHang, lochParams.interactByMovingLayerMask))
            {
                Tags forwardHitTags = _forwardHangHit.transform.gameObject.GetComponent<Tags>();
                //if (forwardHitTags != null && forwardHitTags.HasTag("Grabbable"))
                {
                    Debug.DrawLine(transform.position + hangRayForwardStartPosRelToTransform, transform.position + hangRayForwardStartPosRelToTransform + _forwardHangHit.distance * hangRayForwardDirection, Color.red);
                    if (Physics.SphereCast(transform.position + hangRayDownwardStartPosRelToTransform, 0.01f, Vector3.up * -1, out _downwardHangHit, lochParams.raycastDistanceDownwardHang, lochParams.interactByMovingLayerMask))
                    {
                        Tags downwardHitTags = _downwardHangHit.transform.gameObject.GetComponent<Tags>();
                        //if (downwardHitTags != null && downwardHitTags.HasTag("Grabbable"))
                        {
                            rb.constraints = RigidbodyConstraints.None;
                            Debug.DrawLine(transform.position + hangRayDownwardStartPosRelToTransform, transform.position + hangRayDownwardStartPosRelToTransform + _downwardHangHit.distance * Vector3.up * -1, Color.red);
                            grab = true;
                            isGroundChildSet = false;
                            SetGroundChild(_downwardHangHit.transform, _forwardHangHit.point);
                            GroundChildJoint.connectedBody = rb;
                            topJumpTimer = 0;
                            animator.SetBool("walk", false);
                            canJump = true;
                            isJump = false;
                            //rb.velocity = Vector3.zero;
                            m_GoalVel = Vector3.zero;
                        }
                    }
                }
            }
        }
    }
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private Vector3 m_UnitGoalCashed = Vector3.forward;
    #endregion ledgegrab

    #region walk
    private bool wasPlatformSet = false;
    [Header("Walk")]
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private Vector3 rbPlatformWas = Vector3.zero;

    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private Vector3 curInputAxis;
    /// <summary>
    /// Целевая скорость
    /// </summary>
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private Vector3 m_GoalVel = Vector3.zero;
    /// <summary>
    /// вектор направления перса, без скорости и прочих характеристик
    /// </summary>
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private Vector3 m_UnitGoal = Vector3.zero;
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    public float movementAngle = 0;
    private void rotateToMatchMovement()
    {
        float sv = 0;
        if (curInputAxis.magnitude > 0f)
            rb.MoveRotation(Quaternion.Euler(0f
                , Mathf.SmoothDampAngle(transform.eulerAngles.y, movementAngle, ref sv, 1 / lochParams.rotationSpeed )
                , 0f)
                );
    }

    private void Walk()
    {
        Vector3 platformRelativeVelocity = Vector3.zero;
        rotateToMatchMovement();
        if (isGroundChildSet)
        {
            if (wasPlatformSet)
                platformRelativeVelocity = (groundChild.position - rbPlatformWas - groundChildDifPosition) / Time.fixedDeltaTime;
            rbPlatformWas = groundChild.position;
            wasPlatformSet = true;
        }

        Vector3 unitVel = m_GoalVel.normalized;
        float velDot = Vector3.Dot(m_UnitGoal, unitVel);
        float accel = (isGroundedCloser ? lochParams.Acceleration : lochParams.AirborneAcceleration) * lochParams.AccelerationFactorFromDot.Evaluate(velDot);
        Vector3 goalVel = m_UnitGoal * lochParams.maxSpeed;

        m_GoalVel = Vector3.MoveTowards(m_GoalVel, goalVel, accel * Time.fixedDeltaTime);
        m_GoalVel = Vector3.ClampMagnitude(m_GoalVel, lochParams.maxSpeed);

        if (m_GoalVel.magnitude > 0)
            animator.SetBool("walk", true);
        else
            animator.SetBool("walk", false);
        float maxAccel = lochParams.MaxAccelForce * lochParams.MaxAccelerationForceFromFactorDot.Evaluate(velDot);
        Vector3 neededAcel = (m_GoalVel + platformRelativeVelocity - rb.velocity) / Time.fixedDeltaTime;
        neededAcel = Vector3.ClampMagnitude(neededAcel, maxAccel);

        Vector3 Force = neededAcel * rb.mass;

        rb.AddForce(Vector3.Scale(Force, lochParams.ForceScale));
    }

    public Transform floatingRideHeightCheckTransform;
    private void moveWeightCenter()
    {
        Vector2 rbVel2D = new Vector2(rb.velocity.x, rb.velocity.z);
        float distanceFromRootRideHeightCheck = lochParams.WeightCenterMaxDistance * rbVel2D.magnitude / lochParams.maxSpeed;

        Vector3 offset = new Vector3(rbVel2D.normalized.x, 0f, rbVel2D.normalized.y) * distanceFromRootRideHeightCheck;
        floatingRideHeightCheckTransform.position = rideHeightTransformPoint.position + offset;
    }
    #endregion walk

    #region holdHeight
    [Header("HoldHeight")]
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private float minRaycastLandCheckDistance = 0.28f;
    public Transform rideHeightTransformPoint;
    public Transform groundCheckTransformPoint;
    void HoldHeight()
    {
        RaycastHit _rayHit;
        bool _rayDidHitLand =
            Physics.SphereCast(isTooClose ? TooCloseCheck.position : floatingRideHeightCheckTransform.position
                    , lochParams.sphereLandCheckRadius
                    , isTooClose ? -TooCloseCheck.up : -floatingRideHeightCheckTransform.up
                    , out _rayHit, minRaycastLandCheckDistance, lochParams.interactByMovingLayerMask
                );

        Debug.DrawLine(floatingRideHeightCheckTransform.position, floatingRideHeightCheckTransform.position + _rayHit.distance * Vector3.up * -1, Color.red);

        Vector3 RideHeightTargetGround = floatingRideHeightCheckTransform.position;
        RideHeightTargetGround.y -= lochParams.RideHeight;
        Debug.DrawLine(floatingRideHeightCheckTransform.position, RideHeightTargetGround, Color.green);

        if (_rayDidHitLand)
        {
            Vector3 vel = rb.velocity;
            Vector3 rayDir = floatingRideHeightCheckTransform.TransformDirection(lochParams.DownDir);

            Vector3 otherVel = Vector3.zero;
            Rigidbody hitBody = _rayHit.rigidbody;

            if (hitBody != null)
                otherVel = hitBody.velocity;

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, otherVel);

            float relVel = rayDirVel - otherDirVel;

            float x = _rayHit.distance - lochParams.RideHeight;
            RideHeightTargetGround = floatingRideHeightCheckTransform.position;
            RideHeightTargetGround.y -= _rayHit.distance;

            float springForce = (x * lochParams.RideSpringStrength) - (relVel * lochParams.RideSpringDamper);

            rb.AddForce(rayDir * springForce);

            if (hitBody != null)
                hitBody.AddForceAtPosition(rayDir * -springForce, _rayHit.point);
        }

    }
    #endregion holdHeight

    #region IK

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            IKFoot(AvatarIKGoal.LeftFoot);
            IKFoot(AvatarIKGoal.RightFoot);
        }
    }
    private void IKFoot(AvatarIKGoal foot)
    {
        animator.SetIKPositionWeight(foot, 1f);// animator.GetFloat("IKLeftFootWeight"));
        animator.SetIKRotationWeight(foot, 1f);//, animator.GetFloat("IKLeftFootWeight"));
                                               // Left Foot
        RaycastHit hit;
        // We cast our ray from above the foot in case the current terrain/floor is above the foot position.
        Ray ray = new Ray(animator.GetIKPosition(foot) + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out hit, lochParams.DistanceToGroundIK + 1f, lochParams.interactByMovingLayerMask))
        {
            // We're only concerned with objects that are tagged as "Walkable"
            //if (hit.transform.tag == "Walkable")
            {
                Vector3 footPosition = hit.point; // The target foot position is where the raycast hit a walkable object...
                footPosition.y += lochParams.DistanceToGroundIK; // ... taking account the distance to the ground we added above.
                animator.SetIKPosition(foot, footPosition);
                animator.SetIKRotation(foot, Quaternion.LookRotation(transform.forward, hit.normal));

            }

        }
    }
    #endregion IK

    #region checkSlope
    [Header("SlopeResults")]
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private float groundSlopeAngle = 0f;            // Angle of the slope in degrees
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private Vector3 groundSlopeDir = Vector3.zero;  // The calculated slope as a vector

    [Header("SlopeSettings")]
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private bool showDebug = false;                  // Show debug gizmos and lines
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private bool pit = false;
    public bool Pit
    {
        get => pit;
        set
        {
            if (coyoteTimeTimer == 0 && value && IsGroundedCloserCash)
                pit = value;
            if (!value)
                pit = value;
        }
    }
    /// <summary>
    /// Checks for ground underneath, to determine some info about it, including the slope angle.
    /// </summary>
    /// <param name="origin">Point to start checking downwards from</param>
    public void CheckSlope()
    {
        Vector3 origin = groundCheckTransformPoint.position;
        // Out hit point from our cast(s)
        RaycastHit hit;

        // SPHERECAST
        // "Casts a sphere along a ray and returns detailed information on what was hit."
        if (Physics.SphereCast(origin, lochParams.sphereCastRadius, Vector3.down, out hit, lochParams.sphereCastDistance, lochParams.groundLayerMask))
        {
            // Angle of our slope (between these two vectors). 
            // A hit normal is at a 90 degree angle from the surface that is collided with (at the point of collision).
            // e.g. On a flat surface, both vectors are facing straight up, so the angle is 0.
            groundSlopeAngle = Vector3.Angle(hit.normal, Vector3.up);

            // Find the vector that represents our slope as well. 
            //  temp: basically, finds vector moving across hit surface 
            Vector3 temp = Vector3.Cross(hit.normal, Vector3.down);
            //  Now use this vector and the hit normal, to find the other vector moving up and down the hit surface
            groundSlopeDir = Vector3.Cross(temp, hit.normal);
        }

        // Now that's all fine and dandy, but on edges, corners, etc, we get angle values that we don't want.
        // To correct for this, let's do some raycasts. You could do more raycasts, and check for more
        // edge cases here. There are lots of situations that could pop up, so test and see what gives you trouble.
        RaycastHit slopeHit1;
        RaycastHit slopeHit2;

        // FIRST RAYCAST
        if (Physics.Raycast(origin + lochParams.rayOriginOffset1, Vector3.down, out slopeHit1, lochParams.raycastLength))
        {
            Pit = false;
            // Debug line to first hit point
            if (showDebug) { Debug.DrawLine(origin + lochParams.rayOriginOffset1, slopeHit1.point, Color.blue); }
            // Get angle of slope on hit normal
            float angleOne = Vector3.Angle(slopeHit1.normal, Vector3.up);

            // 2ND RAYCAST
            if (Physics.Raycast(origin + lochParams.rayOriginOffset2, Vector3.down, out slopeHit2, lochParams.raycastLength))
            {
                // Debug line to second hit point
                if (showDebug) { Debug.DrawLine(origin + lochParams.rayOriginOffset2, slopeHit2.point, Color.blue); }
                // Get angle of slope of these two hit points.
                float angleTwo = Vector3.Angle(slopeHit2.normal, Vector3.up);
                // 3 collision points: Take the MEDIAN by sorting array and grabbing middle.
                float[] tempArray = new float[] { groundSlopeAngle, angleOne, angleTwo };
                Array.Sort(tempArray);
                groundSlopeAngle = tempArray[1];
            }
            else
            {
                // 2 collision points (sphere and first raycast): AVERAGE the two
                float average = (groundSlopeAngle + angleOne) / 2;
                groundSlopeAngle = average;
            }
        }
        else
        {
            if (showDebug)
            {
                Debug.DrawLine(origin + lochParams.rayOriginOffset1, origin + lochParams.rayOriginOffset1 + Vector3.down * lochParams.raycastLength, Color.red);
                Debug.DrawLine(origin + lochParams.rayOriginOffset2, origin + lochParams.rayOriginOffset2 + Vector3.down * lochParams.raycastLength, Color.red);
            }
            Pit = true;
        }
    }
    #endregion checkSlope

    #endregion moving


    #region interacting
    [Header("Interacting")]
    public GameObject Mark;
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private GameObject interactedObject;
    void OnTriggerEnter(Collider c)
    {
        interactedObject = c.gameObject;
        Mark.SetActive(true);
        Debug.Log("F");

    }
    void OnTriggerExit()
    {
        interactedObject = null;
        Mark.SetActive(false);
    }
    private void Interacting()
    {/*
        if (isInteracting && interactedObject != null)
        {
            if (interactedObject.CompareTag("SceneScript"))
            {
                interactedObject.GetComponent<ScenesManager>().LoadScene();
                Debug.Log("загрузка сцены");
                interactedObject = null;
                Mark.SetActive(false);
            }
            else
            {
                interactedObject.GetComponent<IngredientScript>().WhenPressed();
                Debug.Log("взаимодействие");
                interactedObject = null;
                Mark.SetActive(false);
            }
        }*/
    }
    #endregion interacting
}
