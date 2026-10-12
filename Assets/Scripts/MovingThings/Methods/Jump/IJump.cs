using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.MovingThings
{
    public interface IJump
    {
        void defineJumpInput(bool JumpPressed, bool JumpDown);
        void JumpCut();
        void startJump();
        void ProlongJump();

        Coroutine startJumpBufferTime();
        void jumpBufferTimeStoppedEffect();
        void stopJumpBufferTime();
        IEnumerator jumpBufferTimeCoroutine();
    }
}