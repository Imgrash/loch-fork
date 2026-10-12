using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.MovingThings
{

    [RequireComponent(typeof(GroundCheckBase))]
    public class MovementVariables : MonoBehaviour
    {
        /// <summary>
        /// Целевая скорость
        /// </summary>
        [HideInInspector]
        public Vector3 m_GoalVel = Vector3.zero;
        /// <summary>
        /// вектор направления перса, без скорости и прочих характеристик
        /// </summary>
        [HideInInspector]
        public Vector3 m_UnitGoal = Vector3.zero;
        [HideInInspector]
        public bool jump;
        [HideInInspector]
        public bool grab = false;
        [HideInInspector]
        public FixedJoint GroundChildJoint;
        [HideInInspector]
        public bool isJump = false;
        [HideInInspector]
        public float TopJumpTimer;
        [HideInInspector]
        public bool CanJump;
        public MovementParamsShared sharedPars;
        [HideInInspector]
        private GroundCheckBase groundCheck;
        [HideInInspector]
        public Transform floatingRideHeightCheckTransform;
        public void Start()
        {
            groundCheck = GetComponent<GroundCheckBase>();
            GroundChildJoint = groundCheck.GroundChild.GetComponent<FixedJoint>();
            CanJump = false;
            TopJumpTimer = 0f;
        }
    }
}
