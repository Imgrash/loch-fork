using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Scripts.MovingThings
{

    [RequireComponent(typeof(GroundCheckBase), typeof(MovementVariables))]
    public class CheckSlopeBase : MonoBehaviour, ICheckSlope
    {
        [SerializeField]
        private MovementParamsCheckSlopeBase prms;

        private GroundCheckBase groundCheck;
        private MovementVariables movVars;

        private void Start()
        {
            movVars = GetComponent<MovementVariables>();
            groundCheck = GetComponent<GroundCheckBase>();
        }

        private float groundSlopeAngle = 0f;
        private Vector3 groundSlopeDir = Vector3.zero;
        private bool showDebug = false;

        /// <summary>
        /// Checks for ground underneath, to determine some info about it, including the slope angle.
        /// </summary>
        /// <param name="origin">Point to start checking downwards from</param>
        public void CheckSlope()
        {
            Vector3 origin = groundCheck.groundCheckTransformPoint.position;
            // Out hit point from our cast(s)
            RaycastHit hit;

            // SPHERECAST
            // "Casts a sphere along a ray and returns detailed information on what was hit."
            if (Physics.SphereCast(origin, prms.sphereCastRadius, Vector3.down, out hit, prms.sphereCastDistance, movVars.sharedPars.groundLayerMask, QueryTriggerInteraction.Ignore))
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
            if (Physics.Raycast(origin + prms.rayOriginOffset1, Vector3.down, out slopeHit1, prms.raycastLength, movVars.sharedPars.groundLayerMask, QueryTriggerInteraction.Ignore))
            {
                groundCheck.Pit = false;
                // Debug line to first hit point
                if (showDebug) { Debug.DrawLine(origin + prms.rayOriginOffset1, slopeHit1.point, Color.blue); }
                // Get angle of slope on hit normal
                float angleOne = Vector3.Angle(slopeHit1.normal, Vector3.up);

                // 2ND RAYCAST
                if (Physics.Raycast(origin + prms.rayOriginOffset2, Vector3.down, out slopeHit2, prms.raycastLength, movVars.sharedPars.groundLayerMask, QueryTriggerInteraction.Ignore))
                {
                    // Debug line to second hit point
                    if (showDebug) { Debug.DrawLine(origin + prms.rayOriginOffset2, slopeHit2.point, Color.blue); }
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
                    Debug.DrawLine(origin + prms.rayOriginOffset1, origin + prms.rayOriginOffset1 + Vector3.down * prms.raycastLength, Color.red);
                    Debug.DrawLine(origin + prms.rayOriginOffset2, origin + prms.rayOriginOffset2 + Vector3.down * prms.raycastLength, Color.red);
                }
                groundCheck.Pit = true;
            }
        }
    }
}