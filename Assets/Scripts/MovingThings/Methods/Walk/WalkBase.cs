using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.MovingThings
{
    [RequireComponent(typeof(BaseVariables), typeof(MovementVariables), typeof(GroundCheckBase))]
    public class WalkBase : MonoBehaviour, IWalk
    {
        [SerializeField]
        private MovementParamsWalkBase prms;

        private BaseVariables baseVars;
        private GroundCheckBase groundCheck;
        private MovementVariables movVars;
        private void Start()
        {
            baseVars = GetComponent<BaseVariables>();
            movVars = GetComponent<MovementVariables>();
            groundCheck = GetComponent<GroundCheckBase>(); 
            rbPlatformWas = groundCheck.GroundChild.position;
        }

        private Vector3 curInputAxis;
        private float movementAngle = 0;
        private bool wasPlatformSet = false;
        private Vector3 rbPlatformWas = Vector3.zero;
        public void SetMovementAxis(Vector3 inputCurInputAxis, float CameraAngle)
        {
            curInputAxis = inputCurInputAxis;
            if (curInputAxis.magnitude > 0f)
            {
                movementAngle = Mathf.Atan2(curInputAxis.x, curInputAxis.y) * Mathf.Rad2Deg + CameraAngle;
                movVars.m_UnitGoal = Quaternion.Euler(0f, movementAngle, 0f) * Vector3.forward;
                movVars.m_UnitGoal.Normalize();
            }
            else
                movVars.m_UnitGoal = Vector3.zero;
        }
        private void rotateToMatchMovement()
        {
            float sv = 0;
            if (curInputAxis.magnitude > 0f)
                baseVars.rb.MoveRotation(Quaternion.Euler(0f
                    , Mathf.SmoothDampAngle(transform.eulerAngles.y, movementAngle, ref sv, 1 / prms.rotationSpeed)
                    , 0f)
                    );
        }
        public void Walk()
        {
            Vector3 platformRelativeVelocity = Vector3.zero;
            rotateToMatchMovement();
            if (groundCheck.IsGroundChildSet)
            {
                if (wasPlatformSet)
                    platformRelativeVelocity = (groundCheck.GroundChild.position - rbPlatformWas - groundCheck.GroundChildDifPosition) / Time.fixedDeltaTime;
                rbPlatformWas = groundCheck.GroundChild.position;
                wasPlatformSet = true;
            }

            Vector3 unitVel = movVars.m_GoalVel.normalized;
            float velDot = Vector3.Dot(movVars.m_UnitGoal, unitVel);
            float accel = (groundCheck.IsGroundedCloser ? prms.Acceleration : prms.AirborneAcceleration) * prms.AccelerationFactorFromDot.Evaluate(velDot);
            Vector3 goalVel = movVars.m_UnitGoal * movVars.sharedPars.maxSpeed;

            movVars.m_GoalVel = Vector3.MoveTowards(movVars.m_GoalVel, goalVel, accel * Time.fixedDeltaTime);
            movVars.m_GoalVel = Vector3.ClampMagnitude(movVars.m_GoalVel, movVars.sharedPars.maxSpeed);

            if (movVars.m_GoalVel.magnitude > 0)
                baseVars.animator.SetBool("walk", true);
            else
                baseVars.animator.SetBool("walk", false);
            float maxAccel = prms.MaxAccelForce * prms.MaxAccelerationForceFromFactorDot.Evaluate(velDot);
            Vector3 neededAcel = (movVars.m_GoalVel + platformRelativeVelocity - baseVars.rb.velocity) / Time.fixedDeltaTime;
            neededAcel = Vector3.ClampMagnitude(neededAcel, maxAccel);

            Vector3 Force = neededAcel * baseVars.rb.mass;

            baseVars.rb.AddForce(Vector3.Scale(Force, prms.ForceScale));
        }
    }
}