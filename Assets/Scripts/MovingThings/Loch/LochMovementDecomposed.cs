using Scripts.MovingThings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IWalk), typeof(IGrab), typeof(IHoldHeight)), RequireComponent(typeof(IGroundCheck), typeof(IJump), typeof(ICheckSlope))
    , RequireComponent(typeof(BaseVariables)), RequireComponent(typeof(MovementVariables))]
public class LochMovementDecomposed : MonoBehaviour
{
    public Transform Camera;
    private IWalk walkClass;
    private IGrab grabClass;
    private IHoldHeight holdHeightClass;
    private IGroundCheck groundCheckClass;
    private IJump jumpClass;
    private ICheckSlope checkSlopeClass;
    private IThrow throwShakerClass;
    private IMix mixClass;
    private InteractionsDispatcher interactionsDispatcher;
    private BaseVariables vars;

    private MovementVariables movVars;
    private void SetInputSubscription()
    {
        inputManager.setInputAxisEvent += SetMovementAxis;
        inputManager.defineJumpInputEvent += defineJumpInput;
        inputManager.defineIsInteractingInputEvent += defineIsInteracting;
        inputManager.defineIsRespawningInputEvent += defineIsRespawning;
        inputManager.defineIsThrowInputEvent += defineIsThrow;
        inputManager.defineIsMixInputEvent += defineIsMix;
    }

    private void UnSetInputSubscription()
    {
        inputManager.setInputAxisEvent -= SetMovementAxis;
        inputManager.defineJumpInputEvent -= defineJumpInput;
        inputManager.defineIsInteractingInputEvent -= defineIsInteracting;
        inputManager.defineIsRespawningInputEvent -= defineIsRespawning;
        inputManager.defineIsThrowInputEvent -= defineIsThrow;
        inputManager.defineIsMixInputEvent -= defineIsMix;
    }
    void SetComponents()
    {
        vars = GetComponent<BaseVariables>();
        walkClass = GetComponent<IWalk>();
        grabClass = GetComponent<IGrab>();
        holdHeightClass = GetComponent<IHoldHeight>();
        groundCheckClass = GetComponent<IGroundCheck>();
        jumpClass = GetComponent<IJump>();
        checkSlopeClass = GetComponent<ICheckSlope>();
        throwShakerClass = GetComponent<IThrow>();
        mixClass = GetComponent<IMix>();

        movVars = GetComponent<MovementVariables>();
        interactionsDispatcher = GetComponent<InteractionsDispatcher>();
        ResetInputSubscription();// SetInputSubscription();
    }
    void Start()
    {
        SetComponents();
        startUnfreeze();
    }
    private Coroutine startUnfreeze() => StartCoroutine(unfreeze());
    IEnumerator unfreeze()
    {
        float timeFreeze = 0.1f;
        float timer = 0; ;
        while(timer < timeFreeze)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        vars.rb.isKinematic = false;
        vars.rb.useGravity = true;
    }

    //[ContextMenuItem("Reset input subscription", "ResetInputSubscription")]

    [ContextMenuItem("Reset components", "SetComponents")]
    public InputManager inputManager;
    public void ResetInputSubscription()
    {
        UnSetInputSubscription();
        SetInputSubscription();
    }
    private void SetMovementAxis()
    {
        walkClass.SetMovementAxis(inputManager.CurInputAxis, Camera.eulerAngles.y);
    }
    private void defineJumpInput()
    {
        jumpClass.defineJumpInput(inputManager.JumpPressed, inputManager.JumpDown);
    }
    public void defineIsInteracting()
    {
        interactionsDispatcher.defineInteractionInput(inputManager.IsInteracting);
    }
    public void defineIsThrow()
    {
        throwShakerClass.defineIsThrow(inputManager.IsThrow);
    }
    public void defineIsMix()
    {
        mixClass.defineIsMix(inputManager.IsMix);
    }
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    private bool isRespawning = false;
    //[NaughtyAttributes.ShowIf("ShowDebugVariables"), SerializeField]
    public Transform respawnPoint;
    public void defineIsRespawning()
    {
        isRespawning = inputManager.IsRespawning;
        if (isRespawning && respawnPoint != null)
            transform.position = respawnPoint.position;
    }

    private void FixedUpdate()
    {
        if (!movVars.grab)
            grabClass.TryGrab();
        if (!movVars.grab)
        {
            groundCheckClass.GroundCheck();
            if (movVars.jump)
                jumpClass.ProlongJump();
            jumpClass.JumpCut();

            walkClass.Walk();
            checkSlopeClass.CheckSlope();
            holdHeightClass.HoldHeight();
        }
        holdHeightClass.moveWeightCenter();
    }
}