using Scripts.MovingThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseVariables), typeof(MovementVariables), typeof(Animator))]
public class IKBase : MonoBehaviour
{
    [SerializeField]
    private MovementParamsIKBase prms;

    private BaseVariables baseVars;
    private MovementVariables movVars;
    private void Start()
    {
        baseVars = GetComponent<BaseVariables>();
        movVars = GetComponent<MovementVariables>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (baseVars.animator)
        {
            IKFoot(AvatarIKGoal.LeftFoot);
            IKFoot(AvatarIKGoal.RightFoot);
        }
    }
    private void IKFoot(AvatarIKGoal foot)
    {
        baseVars.animator.SetIKPositionWeight(foot, 1f);// animator.GetFloat("IKLeftFootWeight"));
        baseVars.animator.SetIKRotationWeight(foot, 1f);//, animator.GetFloat("IKLeftFootWeight"));
                                                // Left Foot
        RaycastHit hit;
        // We cast our ray from above the foot in case the current terrain/floor is above the foot position.
        Ray ray = new Ray(baseVars.animator.GetIKPosition(foot) + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out hit, prms.DistanceToGroundIK + 1f, movVars.sharedPars.interactByMovingLayerMask, QueryTriggerInteraction.Ignore))
        {
            // We're only concerned with objects that are tagged as "Walkable"
            //if (hit.transform.tag == "Walkable")
            {
                Vector3 footPosition = hit.point; // The target foot position is where the raycast hit a walkable object...
                footPosition.y += prms.DistanceToGroundIK; // ... taking account the distance to the ground we added above.
                baseVars.animator.SetIKPosition(foot, footPosition);
                baseVars.animator.SetIKRotation(foot, Quaternion.LookRotation(transform.forward, hit.normal));

            }

        }
    }
}
