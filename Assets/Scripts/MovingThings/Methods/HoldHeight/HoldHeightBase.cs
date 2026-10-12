using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.MovingThings
{
    [RequireComponent(typeof(BaseVariables), typeof(MovementVariables), typeof(GroundCheckBase))]
    public class HoldHeightBase : MonoBehaviour, IHoldHeight
    {
        [SerializeField]
        private MovementParamsHoldHeightBase prms;

        private BaseVariables baseVars;
        private GroundCheckBase groundCheck;
        private MovementVariables movVars;
        public Transform rideHeightTransformPoint;
        private void Start()
        {
            baseVars = GetComponent<BaseVariables>();
            groundCheck = GetComponent<GroundCheckBase>();
            movVars = GetComponent<MovementVariables>();
        }

        public Transform floatingRideHeightCheckTransform;

        public void HoldHeight()
        {
            RaycastHit _rayHit;
            bool _rayDidHitLand =
                Physics.SphereCast(groundCheck.IsTooClose ? groundCheck.TooCloseCheck.position : floatingRideHeightCheckTransform.position
                        , movVars.sharedPars.sphereLandCheckRadius
                        , groundCheck.IsTooClose ? -groundCheck.TooCloseCheck.up : -floatingRideHeightCheckTransform.up
                        , out _rayHit, groundCheck.minRaycastLandCheckDistance, movVars.sharedPars.interactByMovingLayerMask, QueryTriggerInteraction.Ignore
                    );

            Debug.DrawLine(floatingRideHeightCheckTransform.position, floatingRideHeightCheckTransform.position + _rayHit.distance * Vector3.up * -1, Color.red);

            Vector3 RideHeightTargetGround = floatingRideHeightCheckTransform.position;
            RideHeightTargetGround.y -= prms.RideHeight;
            Debug.DrawLine(floatingRideHeightCheckTransform.position, RideHeightTargetGround, Color.green);

            if (_rayDidHitLand)
            {
                Vector3 vel = baseVars.rb.velocity;
                Vector3 rayDir = floatingRideHeightCheckTransform.TransformDirection(prms.DownDir);

                Vector3 otherVel = Vector3.zero;
                Rigidbody hitBody = _rayHit.rigidbody;

                if (hitBody != null)
                    otherVel = hitBody.velocity;

                float rayDirVel = Vector3.Dot(rayDir, vel);
                float otherDirVel = Vector3.Dot(rayDir, otherVel);

                float relVel = rayDirVel - otherDirVel;

                float x = _rayHit.distance - prms.RideHeight;
                RideHeightTargetGround = floatingRideHeightCheckTransform.position;
                RideHeightTargetGround.y -= _rayHit.distance;

                float springForce = (x * prms.RideSpringStrength) - (relVel * prms.RideSpringDamper);

                baseVars.rb.AddForce(rayDir * springForce);

                if (hitBody != null)
                    hitBody.AddForceAtPosition(rayDir * -springForce, _rayHit.point);
            }

        }
        public void moveWeightCenter()
        {
            Vector2 rbVel2D = new Vector2(baseVars.rb.velocity.x, baseVars.rb.velocity.z);
            float distanceFromRootRideHeightCheck = prms.WeightCenterMaxDistance * rbVel2D.magnitude / movVars.sharedPars.maxSpeed;

            Vector3 offset = new Vector3(rbVel2D.normalized.x, 0f, rbVel2D.normalized.y) * distanceFromRootRideHeightCheck;
            floatingRideHeightCheckTransform.position = rideHeightTransformPoint.position + offset;
            //Debug.Log($"distanceFromRootRideHeightCheck {distanceFromRootRideHeightCheck}, rideHeightTransformPoint {rideHeightTransformPoint.position}");
        }
    }
}