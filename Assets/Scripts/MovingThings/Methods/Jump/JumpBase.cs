using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.MovingThings
{
    [RequireComponent(typeof(BaseVariables), typeof(MovementVariables), typeof(GroundCheckBase))]
    public class JumpBase : MonoBehaviour, IJump
    {
        [SerializeField]
        private MovementParamsJumpBase prms;

        private BaseVariables baseVars;
        private GroundCheckBase groundCheck;
        private MovementVariables movVars;
        private void Start()
        {
            baseVars = GetComponent<BaseVariables>();
            movVars = GetComponent<MovementVariables>();
            groundCheck = GetComponent<GroundCheckBase>();
        }

        private bool jumpPressed = false;
        private bool jumpDown = false;

        public void defineJumpInput(bool JumpPressed, bool JumpDown)
        {
            jumpPressed = JumpPressed;
            if (JumpDown)
            {
                stopJumpBufferTime();
                jumpDown = true;
                startJump();
            }
        }
        public void JumpCut()
        {
            if (movVars.isJump && movVars.TopJumpTimer < prms.topJumpLength)
                movVars.TopJumpTimer += Time.fixedDeltaTime;

            if (baseVars.rb.velocity.y < 0 && !groundCheck.IsGroundedCloser)
                baseVars.rb.velocity += Vector3.up * Physics.gravity.y * (prms.fallMultiplier - 1) * Time.fixedDeltaTime;
            else if (baseVars.rb.velocity.y > 0 && movVars.isJump && (!jumpPressed || movVars.TopJumpTimer > prms.topJumpLength))
            {
                baseVars.rb.velocity += Vector3.up * Physics.gravity.y * (prms.lowJumpMultiplier - 1) * Time.fixedDeltaTime;
                groundCheck.GroundCheckRadiusSetMaxRadius();
            }
        }
        public void startJump()
        {
            if (jumpDown)
            {
                if (movVars.CanJump && !movVars.isJump)
                {
                    stopJumpBufferTime();
                    movVars.jump = true;
                    if (movVars.grab)
                    {
                        movVars.grab = false;
                        groundCheck.IsGroundChildSet = false;
                        movVars.GroundChildJoint.connectedBody = null;
                        baseVars.rb.constraints = RigidbodyConstraints.FreezeRotation;
                        baseVars.rb.AddForce(Vector3.up * prms.jumpForceFromGrab, ForceMode.Impulse);
                    }
                }
                else if (jumpBufferTimeTimer == 0 && !movVars.CanJump)
                    currentCoroutineJumpBuffer = startJumpBufferTime();
            }
        }
        public void ProlongJump()
        {
            groundCheck.SetLandCheckDistanceWhileJump();
            baseVars.rb.velocity = new Vector3(baseVars.rb.velocity.x, 0f, baseVars.rb.velocity.z);
            baseVars.rb.AddForce(Vector3.up * prms.jumpForce, ForceMode.Impulse);

            movVars.isJump = true;
            movVars.jump = false;
        }

        private float jumpBufferTimeTimer = 0f;
        private Coroutine currentCoroutineJumpBuffer;
        public Coroutine startJumpBufferTime() => StartCoroutine(jumpBufferTimeCoroutine());
        public void jumpBufferTimeStoppedEffect()
        {
            jumpDown = false;
            jumpBufferTimeTimer = 0;
        }
        public void stopJumpBufferTime()
        {
            if (jumpBufferTimeTimer > 0)
                StopCoroutine(currentCoroutineJumpBuffer);
            jumpBufferTimeStoppedEffect();
        }
        public IEnumerator jumpBufferTimeCoroutine()
        {
            while (jumpBufferTimeTimer < prms.jumpBufferTime)
            {
                jumpBufferTimeTimer += Time.deltaTime;
                yield return null;
            }
            jumpBufferTimeStoppedEffect();
        }
    }
}