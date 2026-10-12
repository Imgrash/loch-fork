using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Scripts.MovingThings
{
    [RequireComponent(typeof(BaseVariables), typeof(MovementVariables))]
    public class GroundCheckBase : MonoBehaviour, IGroundCheck
    {
        [SerializeField]
        private MovementParamsGroundCheckBase prms;

        private BaseVariables baseVars;
        private MovementVariables movVars;
        private void Start()
        {
            baseVars = GetComponent<BaseVariables>();
            movVars = GetComponent<MovementVariables>();
            SetLandCheckDistanceSetting();
            GroundCheckRadiusSetMinRadius();
            CoyoteTimeTimer = 0f;
            IsGroundedCloser = false;
            IsTooClose = false;
            IsGroundedCloserCash = false;
            GroundChildDifPosition = Vector3.zero;
        }

        public Transform GroundChild;
        public GameObject GroundChildPrefab;
        public Vector3 GroundChildDifPosition { get; private set; }
        public float GroundCheckRadius { get ; private set; }
        public void GroundCheckRadiusSetMinRadius() => GroundCheckRadius = prms.minGroundCheckRadius;
        public void GroundCheckRadiusSetMaxRadius() => GroundCheckRadius = prms.maxGroundCheckRadius;
        public bool IsGroundedCloser;
        public Transform groundCheckTransformPoint;

        public bool IsTooClose { get; private set; }
        public Transform TooCloseCheck;
        private Coroutine currentCoroutineCoyoteTime;
        public float minRaycastLandCheckDistance { get; private set; }
        public void SetLandCheckDistanceSetting() => minRaycastLandCheckDistance = prms.minRayCastLandCheckDistanceSetting;
        public void SetLandCheckDistanceWhileJump() => minRaycastLandCheckDistance = prms.minRaycastLandCheckDistanceWhileJump;

        public float CoyoteTimeTimer { get; private set; }
        public bool IsGroundedCloserCash { get; private set; }

        private bool pit = false;
        public bool Pit
        {
            get => pit;
            set
            {
                if (CoyoteTimeTimer == 0 && value && IsGroundedCloserCash)
                    pit = value;
                if (!value)
                    pit = value;
            }
        }
        private void stopCoyoteTime()
        {
            if (CoyoteTimeTimer > 0 && CoyoteTimeTimer < movVars.sharedPars.coyoteTime)
                StopCoroutine(currentCoroutineCoyoteTime);
            coyoteTimeStoppedEffect();
        }
        private void coyoteTimeStoppedEffect()
        {
            CoyoteTimeTimer = 0;
            baseVars.rb.useGravity = true;
        }
        public Transform groundChildCheckPosition;

        private bool isGroundChildSet = false;
        public bool IsGroundChildSet { get => isGroundChildSet; set => isGroundChildSet = value; }
        public void SetGroundChild(Transform parent, Vector3 whereToPut)
        {
            if(GroundChild == null)
            {
                GroundChild = Instantiate(GroundChildPrefab).transform;
                movVars.GroundChildJoint = GroundChild.GetComponent<FixedJoint>();
            }
            GroundChild.SetParent(parent);
            GroundChildDifPosition = whereToPut - GroundChild.position;
            GroundChild.position = whereToPut;
            isGroundChildSet = true;
            
        }
        private Coroutine startCoyoteTime() => StartCoroutine(coyoteTimeCoroutine());
        IEnumerator coyoteTimeCoroutine()
        {
            baseVars.rb.useGravity = false;
            baseVars.rb.velocity = new Vector3(baseVars.rb.velocity.x, 0f, baseVars.rb.velocity.z);
            while (CoyoteTimeTimer < movVars.sharedPars.coyoteTime && !movVars.isJump && !IsGroundedCloser)
            {
                CoyoteTimeTimer += Time.deltaTime;
                yield return null;
            }
            movVars.CanJump = false;
            coyoteTimeStoppedEffect();
        }
        public void GroundCheck()
        {
            isGroundChildSet = false;
            Collider[] hitColliders = Physics.OverlapSphere(groundChildCheckPosition.position, GroundCheckRadius, movVars.sharedPars.groundLayerMask, QueryTriggerInteraction.Ignore);
            if (hitColliders.Length > 0)
                SetGroundChild(hitColliders[0].transform, groundChildCheckPosition.position);

            IsGroundedCloser = Physics.CheckCapsule(
                groundCheckTransformPoint.position
                , groundCheckTransformPoint.position - new Vector3(0, -GroundCheckRadius, 0)
                , movVars.sharedPars.sphereLandCheckRadius
                , movVars.sharedPars.groundLayerMask, QueryTriggerInteraction.Ignore
            );
            IsTooClose = Physics.CheckSphere(TooCloseCheck.position, GroundCheckRadius, movVars.sharedPars.groundLayerMask, QueryTriggerInteraction.Ignore);

            if(IsGroundedCloser)
            {
                stopCoyoteTime();
                SetLandCheckDistanceSetting();
                if (movVars.isJump)
                {
                    movVars.isJump = false;
                    baseVars.rb.velocity = new Vector3(baseVars.rb.velocity.x, 0, baseVars.rb.velocity.z);
                    GroundCheckRadiusSetMinRadius();
                    movVars.TopJumpTimer = 0;
                }
                movVars.CanJump = true;
            }
            else if (CoyoteTimeTimer == 0 && !movVars.isJump && Pit && IsGroundedCloserCash)
            {
                currentCoroutineCoyoteTime = startCoyoteTime();
                IsGroundedCloserCash = false;
                Pit = false;
            }
            else if (movVars.isJump)
                movVars.CanJump = false;
            if (IsGroundedCloser)
                IsGroundedCloserCash = IsGroundedCloser;
        }
    }
}