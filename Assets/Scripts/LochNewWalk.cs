using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

public class LochNewWalk : MonoBehaviour
{
    private Animator animator;
    public GameObject interactedObject;
    public GameObject Mark;

    public Transform Camera;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    #region moving

    public float sphereLandCheckRadius = 0.01f;
    public float minRaycastLandCheckDistance = 0.28f;
    public float minRayCastLandCheckDistanceSetting = 0.28f;
    public bool _rayDidHitLand;
    public Transform rideHeightTransformPoint;
    public Transform groundCheckTransformPoint;

    public Vector3 DownDir = new Vector3(0,-1,0);
    public float RideSpringDamper = 3000f;
    public float RideSpringStrength = 40000f;
    public float RideHeight = 0.28f;
    RaycastHit _rayHit;

    public bool jump;
    public bool canJump = false;
    public float jumpForce = 600f;
    public LayerMask interactByMovingLayerMask;

    public float fallMultiplier = 3f;
    public float lowJumpMultiplier = 8f;

    public float topJumpLength = 0.4f;
    public float topJumpTimer = 0f;
    public bool isJump = false;
    public float jumpForceFromGrab = 150f;

    public void jumpCut()
    {
        if (isJump && topJumpTimer < topJumpLength)
            topJumpTimer += Time.deltaTime;

        if (rb.velocity.y < 0 && !isGroundedCloser)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && isJump && (!Input.GetButton("Jump") || topJumpTimer > topJumpLength))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            if(groundCheckRadius != maxGroundCheckRadius)
                groundCheckRadius = maxGroundCheckRadius;
        }
    }

    public void startJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            jump = true;
            if(grab)
            {
                grab = false;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                rb.AddForce(Vector3.up * jumpForceFromGrab, ForceMode.Impulse);
            }
        }
    }

    void Update()
    {
        jumpCut();
        startJump();
    }


    void holdHeight()
    {
        if (Physics.SphereCast(isTooClose ? TooCloseCheck.position : floatingRideHeightCheckTransform.position
                , sphereLandCheckRadius
                , -floatingRideHeightCheckTransform.up
                , out _rayHit, minRaycastLandCheckDistance, interactByMovingLayerMask
            )
        )
        {
            _rayDidHitLand = true;
        }
        else
        {
            _rayDidHitLand = false;
        }
        Debug.DrawLine(floatingRideHeightCheckTransform.position, floatingRideHeightCheckTransform.position + _rayHit.distance * Vector3.up * -1, Color.red);

        Vector3 RideHeightTargetGround = floatingRideHeightCheckTransform.position;
        RideHeightTargetGround.y -= RideHeight;
        Debug.DrawLine(floatingRideHeightCheckTransform.position, RideHeightTargetGround, Color.green);

        if (_rayDidHitLand)
        {
            Vector3 vel = rb.velocity;
            Vector3 rayDir = floatingRideHeightCheckTransform.TransformDirection(DownDir);

            Vector3 otherVel = Vector3.zero;
            Rigidbody hitBody = _rayHit.rigidbody;

            if (hitBody != null)
            {
                otherVel = hitBody.velocity;
            }

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, otherVel);

            float relVel = rayDirVel - otherDirVel;

            float x = _rayHit.distance - RideHeight;
            RideHeightTargetGround = floatingRideHeightCheckTransform.position;
            RideHeightTargetGround.y -= _rayHit.distance;
            //Debug.DrawLine(floatingRideHeightCheckTransform.position, RideHeightTargetGround, Color.red);

            float springForce = (x * RideSpringStrength) - (relVel * RideSpringDamper);

            //Debug.DrawLine(floatingRideHeightCheckTransform.position, floatingRideHeightCheckTransform.position + (rayDir * springForce), Color.yellow);
            rb.AddForce(rayDir * springForce);

            if (hitBody != null)
            {
                hitBody.AddForceAtPosition(rayDir * -springForce, _rayHit.point);
            }
        }

    }

    //#region hardStopping
    //public bool wasMoving = false;
    /*public bool isHardStopAllowed = true;
    public float hardStoppingTime = 0.5f;
    public bool hardStopping = false;*/
    //public AnimationCurve hardStoppingCurve;
    /*private void hardStop() => StartCoroutine(hardStopCoroutine());
    IEnumerator hardStopCoroutine()
    {
        isZeroDeadZoneStart = isZeroDeadZoneEnd = false;
        hardStopping = true;
        float elapsedTime = 0;
        animator.SetBool("walk", false);
        animator.SetBool("hardStop", true);
        while (curSpeed > 0)
        {
            elapsedTime += Time.fixedDeltaTime;
            elapsedTime = Mathf.Clamp(elapsedTime, 0, hardStoppingTime);
            curSpeed = Mathf.Clamp(Mathf.Lerp(maxSpeed, 0, hardStoppingCurve.Evaluate(elapsedTime / hardStoppingTime)), 0, maxSpeed);
            yield return null;
        }
        animator.SetBool("hardStop", false);
        wasMoving = false;
        hardStopping = false;
    }*/
    //#endregion hardStopping
    /*
    #region zeroDeadZoning
    public float zeroDeadZoneTimer = 0.05f;
    public bool isZeroDeadZoneStart = false;
    public bool isZeroDeadZoneEnd = false;
    private void zeroDeadZoning() => StartCoroutine(zeroDeadZoningCoroutine());
    IEnumerator zeroDeadZoningCoroutine()
    {
        isZeroDeadZoneStart = true;
        float elapsedTime = 0;
        while (elapsedTime < zeroDeadZoneTimer)
        {
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }
        isZeroDeadZoneEnd = true;
    }
    #endregion zeroDeadZoning
    */

    private float sv;
    private Vector3 curInputAxis;
    /// <summary>
    /// Целевая скорость
    /// </summary>
    Vector3 m_GoalVel = Vector3.zero;
    /// <summary>
    /// вектор направления перса, без скорости и прочих характеристик
    /// </summary>
    Vector3 m_UnitGoal = Vector3.zero;
    float movementAngle = 0;
    private void rotateToMatchMovement() => transform.rotation =
            curInputAxis.magnitude > 0f ?
            Quaternion.Euler(0f
                                , Mathf.SmoothDampAngle(transform.eulerAngles.y, movementAngle, ref sv, 1 / rotationSpeed)
                                , 0f
            )                           :
            transform.rotation;

    public float maxSpeed = 5f;
    public float rotationSpeed = 10f;
    public float Acceleration = 200f;
    public AnimationCurve AccelerationFactorFromDot;
    public AnimationCurve MaxAccelerationForceFromFactorDot;
    public float MaxAccelForce = 150f;
    public Vector3 ForceScale = new Vector3(1, 0, 1);
    public float AirborneAcceleration = 20f;

    public float forwardHangRayOffsetForward = 0.17f
        , forwardHangRayOffset = 1.44f;
    public float downwardHangRayOffsetForward = 0.36f
        , downwardHangRayOffset = 1.6f;
    public Vector3 hangRayForwardStartPosRelToTransform = Vector3.zero;
    public Vector3 hangRayForwardDirection = Vector3.zero;

    public Vector3 hangRayDownwardStartPosRelToTransform = Vector3.zero;
    RaycastHit _forwardHangHit, _downwardHangHit;
    public float raycastDistanceForwardHang = 0.33f;
    public float raycastDistanceDownwardHang = 0.24f;
    public bool grab = false;
    public void hangthere()
    {
        if (m_UnitGoal.magnitude > 0 && !grab)
        {
            m_UnitGoalCashed = m_UnitGoal;
        }
            hangRayForwardStartPosRelToTransform = forwardHangRayOffsetForward * m_UnitGoalCashed.normalized;
            hangRayForwardStartPosRelToTransform.y += forwardHangRayOffset;

            hangRayDownwardStartPosRelToTransform = m_UnitGoalCashed.normalized * downwardHangRayOffsetForward;
            hangRayDownwardStartPosRelToTransform.y += downwardHangRayOffset;

            hangRayForwardDirection = m_UnitGoalCashed.normalized;
        Debug.DrawRay(transform.position + hangRayForwardStartPosRelToTransform, hangRayForwardDirection * raycastDistanceForwardHang);
        Debug.DrawRay(transform.position + hangRayDownwardStartPosRelToTransform, Vector3.up * -raycastDistanceDownwardHang);
        if (!isGroundedCloser && rb.velocity.y < 0 && !jump)
        {

            if (Physics.SphereCast(transform.position + hangRayForwardStartPosRelToTransform, 0.01f, hangRayForwardDirection, out _forwardHangHit, raycastDistanceForwardHang, interactByMovingLayerMask))
            {
                Tags forwardHitTags = _forwardHangHit.transform.gameObject.GetComponent<Tags>();
                //if (forwardHitTags != null && forwardHitTags.HasTag("Grabbable"))
                {
                    Debug.DrawLine(transform.position + hangRayForwardStartPosRelToTransform, transform.position + hangRayForwardStartPosRelToTransform + _forwardHangHit.distance * hangRayForwardDirection, Color.red);
                    if (Physics.SphereCast(transform.position + hangRayDownwardStartPosRelToTransform, 0.01f, Vector3.up * -1, out _downwardHangHit, raycastDistanceDownwardHang, interactByMovingLayerMask))
                    {
                        Tags downwardHitTags = _downwardHangHit.transform.gameObject.GetComponent<Tags>();
                        //if (downwardHitTags != null && downwardHitTags.HasTag("Grabbable"))
                        {
                            Debug.DrawLine(transform.position + hangRayDownwardStartPosRelToTransform, transform.position + hangRayDownwardStartPosRelToTransform + _downwardHangHit.distance * Vector3.up * -1, Color.red);
                            grab = true;
                            topJumpTimer = 0;
                            animator.SetBool("walk", false);
                            canJump = true; 
                            isJump = false;
                            rb.velocity = Vector3.zero;
                            m_GoalVel = Vector3.zero;
                            rb.constraints = RigidbodyConstraints.FreezeAll;
                        }
                    }
                }
            }
            if (Physics.SphereCast(transform.position + hangRayDownwardStartPosRelToTransform, 0.01f, Vector3.up * -1, out _downwardHangHit, raycastDistanceDownwardHang, interactByMovingLayerMask))
            {
                Tags downwardHitTags = _downwardHangHit.transform.gameObject.GetComponent<Tags>();
                //if (downwardHitTags != null && downwardHitTags.HasTag("Grabbable"))
                {
                    Debug.DrawLine(transform.position + hangRayDownwardStartPosRelToTransform, transform.position + hangRayDownwardStartPosRelToTransform + _downwardHangHit.distance * Vector3.up * -1, Color.red);
                }
            }
        }
    }
    public Vector3 m_UnitGoalCashed = Vector3.forward;
    private void moving()
    {
        rotateToMatchMovement();

        Vector3 unitVel = m_GoalVel.normalized;
        float velDot = Vector3.Dot(m_UnitGoal, unitVel);
        float accel = (isGroundedCloser && !justjumped ? Acceleration : AirborneAcceleration) * AccelerationFactorFromDot.Evaluate(velDot);
        Vector3 goalVel = m_UnitGoal * maxSpeed;

        m_GoalVel = Vector3.MoveTowards(m_GoalVel,
            (goalVel) ,//+ groundVel
            accel * Time.fixedDeltaTime
            );
        m_GoalVel = Vector3.ClampMagnitude(m_GoalVel, maxSpeed);

        if(m_GoalVel.magnitude > 0)
            animator.SetBool("walk", true);
        else
            animator.SetBool("walk", false);
        float maxAccel = MaxAccelForce * MaxAccelerationForceFromFactorDot.Evaluate(velDot);
        Vector3 neededAcel = (m_GoalVel - rb.velocity) / Time.fixedDeltaTime;
        neededAcel = Vector3.ClampMagnitude(neededAcel, maxAccel);
        Vector3 Force = neededAcel * rb.mass;
        rb.AddForce(Vector3.Scale(Force, ForceScale));
    }
    #endregion moving

    private void getInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        curInputAxis = new Vector2(horizontal, vertical).normalized;
        if (curInputAxis.magnitude > 0f)
        {
            movementAngle = Mathf.Atan2(curInputAxis.x, curInputAxis.y) * Mathf.Rad2Deg + Camera.eulerAngles.y;
            m_UnitGoal = Quaternion.Euler(0f, movementAngle, 0f) * Vector3.forward;
            m_UnitGoal.Normalize();
        }
        else 
        {
            m_UnitGoal = Vector3.zero;
        }
    }
    bool justjumped = false;
    private void FixedUpdate()
    {
        getInput();
        hangthere();
        if (!grab)
        {
            if (justjumped)
            {
                justjumped = false;
            }
            GroundCheck();
            if (jump)
            {
                minRaycastLandCheckDistance = 0.1f;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                isJump = true;
                jump = false;
            }
            moving();
            holdHeight();
            interacting();
        }
        moveWeightCenter();
    }
    #region interacting
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
    private void interacting()
    {/*
        if (Input.GetKey(KeyCode.F) && interactedObject != null)
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

    public float DistanceToGroundIK = 0.06f;
    private void IKFoot(AvatarIKGoal foot)
    {
        animator.SetIKPositionWeight(foot, 1f);// animator.GetFloat("IKLeftFootWeight"));
        animator.SetIKRotationWeight(foot, 1f);//, animator.GetFloat("IKLeftFootWeight"));
                                                                // Left Foot
        RaycastHit hit;
        // We cast our ray from above the foot in case the current terrain/floor is above the foot position.
        Ray ray = new Ray(animator.GetIKPosition(foot) + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out hit, DistanceToGroundIK + 1f, interactByMovingLayerMask))
        {
            // We're only concerned with objects that are tagged as "Walkable"
            //if (hit.transform.tag == "Walkable")
            {
                Vector3 footPosition = hit.point; // The target foot position is where the raycast hit a walkable object...
                footPosition.y += DistanceToGroundIK; // ... taking account the distance to the ground we added above.
                animator.SetIKPosition(foot, footPosition);
                animator.SetIKRotation(foot, Quaternion.LookRotation(transform.forward, hit.normal));

            }

        }
    }
    public float WeightCenterMaxDistance = 0.2f;
    public Transform floatingRideHeightCheckTransform;
    private void moveWeightCenter()
    {
        Vector2 rbVel2D = new Vector2(rb.velocity.x, rb.velocity.z);
        float distanceFromRootRideHeightCheck = WeightCenterMaxDistance * rbVel2D.magnitude / maxSpeed;

        Vector3 offset = new Vector3(rbVel2D.normalized.x,0f,rbVel2D.normalized.y) * distanceFromRootRideHeightCheck;
        floatingRideHeightCheckTransform.position = rideHeightTransformPoint.position + offset;
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            IKFoot(AvatarIKGoal.LeftFoot);
            IKFoot(AvatarIKGoal.RightFoot);
        }
    }

    public bool isGroundedCloser = false;
    public float groundCheckRadius = 0.08f;
    public float minGroundCheckRadius = 0.08f;
    public float maxGroundCheckRadius = 0.12f;

    public LayerMask groundLayerMask;
    public Transform TooCloseCheck;
    public bool isTooClose = false;
    public void GroundCheck()
    {
        isGroundedCloser = Physics.CheckSphere(groundCheckTransformPoint.position, groundCheckRadius, groundLayerMask);
        isTooClose = Physics.CheckSphere(TooCloseCheck.position, groundCheckRadius, groundLayerMask);

        if (isGroundedCloser)
        {
            minRaycastLandCheckDistance = minRayCastLandCheckDistanceSetting;
            if (isJump)
            {
                isJump = false;
                Debug.Log("Landed");
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                groundCheckRadius = minGroundCheckRadius;
                topJumpTimer = 0;
            }
            canJump = true;
        }
        else
            canJump = false;
    }
   
}
