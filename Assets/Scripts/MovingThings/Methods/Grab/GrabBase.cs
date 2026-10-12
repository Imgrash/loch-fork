using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.MovingThings
{
    [RequireComponent(typeof(BaseVariables), typeof(MovementVariables), typeof(GroundCheckBase))]
    public class GrabBase : MonoBehaviour, IGrab
    {
        [SerializeField]
        private MovementParamsGrabBase prms;

        private BaseVariables baseVars;
        private GroundCheckBase groundCheck;
        private MovementVariables movVars;
        private void Start()
        {
            baseVars = GetComponent<BaseVariables>();
            movVars = GetComponent<MovementVariables>();
            groundCheck = GetComponent<GroundCheckBase>();
        }
        //grabparams
        private Vector3 m_UnitGoalCashed = Vector3.forward;
        private Vector3 hangRayForwardStartPosRelToTransform = Vector3.zero;
        private Vector3 hangRayForwardDirection = Vector3.zero;
        private Vector3 hangRayDownwardStartPosRelToTransform = Vector3.zero;

        public void TryGrab()
        {
            RaycastHit _forwardHangHit, _downwardHangHit;
            if (movVars.m_UnitGoal.magnitude > 0)
                //надо, чтобы стоящий на месте перс тоже мог зацепиться (иначе у него нет вектора направления, куда цепляться)
                m_UnitGoalCashed = movVars.m_UnitGoal;

            hangRayForwardStartPosRelToTransform = prms.forwardHangRayOffsetForward * m_UnitGoalCashed.normalized;
            hangRayForwardStartPosRelToTransform.y += prms.forwardHangRayOffset;

            hangRayDownwardStartPosRelToTransform = m_UnitGoalCashed.normalized * prms.downwardHangRayOffsetForward;
            hangRayDownwardStartPosRelToTransform.y += prms.downwardHangRayOffset;

            hangRayForwardDirection = m_UnitGoalCashed.normalized;
            Debug.DrawRay(transform.position + hangRayForwardStartPosRelToTransform, hangRayForwardDirection * prms.raycastDistanceForwardHang);
            Debug.DrawRay(transform.position + hangRayDownwardStartPosRelToTransform, Vector3.up * -prms.raycastDistanceDownwardHang);
            if (!groundCheck.IsGroundedCloser && baseVars.rb.velocity.y < 0 && !movVars.jump)
            {
                if (Physics.SphereCast(transform.position + hangRayForwardStartPosRelToTransform, 0.01f, hangRayForwardDirection, out _forwardHangHit, prms.raycastDistanceForwardHang, movVars.sharedPars.interactByMovingLayerMask, QueryTriggerInteraction.Ignore))
                {
                    Tags forwardHitTags = _forwardHangHit.transform.gameObject.GetComponent<Tags>();
                    //if (forwardHitTags != null && forwardHitTags.HasTag("Grabbable"))
                    {
                        Debug.DrawLine(transform.position + hangRayForwardStartPosRelToTransform, transform.position + hangRayForwardStartPosRelToTransform + _forwardHangHit.distance * hangRayForwardDirection, Color.red);
                        if (Physics.SphereCast(transform.position + hangRayDownwardStartPosRelToTransform, 0.01f, Vector3.up * -1, out _downwardHangHit, prms.raycastDistanceDownwardHang, movVars.sharedPars.interactByMovingLayerMask, QueryTriggerInteraction.Ignore))
                        {
                            Tags downwardHitTags = _downwardHangHit.transform.gameObject.GetComponent<Tags>();
                            //if (downwardHitTags != null && downwardHitTags.HasTag("Grabbable"))
                            {
                                baseVars.rb.constraints = RigidbodyConstraints.None;
                                Debug.DrawLine(transform.position + hangRayDownwardStartPosRelToTransform, transform.position + hangRayDownwardStartPosRelToTransform + _downwardHangHit.distance * Vector3.up * -1, Color.red);
                                movVars.grab = true;
                                groundCheck.SetGroundChild(_downwardHangHit.transform, _forwardHangHit.point);
                                movVars.GroundChildJoint.connectedBody = baseVars.rb;
                                movVars.TopJumpTimer = 0;
                                baseVars.animator.SetBool("walk", false);
                                movVars.CanJump = true;
                                movVars.isJump = false;
                                movVars.m_GoalVel = Vector3.zero;
                            }
                        }
                    }
                }
            }
        }
    }
}