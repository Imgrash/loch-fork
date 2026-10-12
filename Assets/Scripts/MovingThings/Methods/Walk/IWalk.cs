using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.MovingThings
{
    public interface IWalk
    {
        void SetMovementAxis(Vector3 inputCurInputAxis, float CameraAngle);
        void Walk();
    }
}