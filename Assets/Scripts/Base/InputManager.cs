using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using static UnityEngine.PlayerLoop.PostLateUpdate;

public class InputManager : MonoBehaviour
{
    public static InputManager instance = null;
    public InputAction playerControls;
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance = this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    #region Axis
    public Vector3 CurInputAxis { get; private set; }
    public event Action setInputAxisEvent;
    public void SetInputAxis()
    {
        CurInputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        setInputAxisEvent?.Invoke();
    }
    #endregion

    #region Jump
    private bool jumpPressed;
    public bool JumpPressed { get => jumpPressed; private set => jumpPressed = value; }
    private bool jumpDown;
    public bool JumpDown { get => jumpDown; private set => jumpDown = value; }
    public event Action defineJumpInputEvent;
    private void defineJumpInput()
    {
        JumpPressed = Input.GetButton("Jump");
        JumpDown = Input.GetButtonDown("Jump");
        defineJumpInputEvent?.Invoke();
    }
    #endregion

    public void defineByGetKey(ref bool IsGetKey, KeyCode keyCode, Action InputEvent)
    {
        IsGetKey = Input.GetKey(keyCode);
        InputEvent?.Invoke();
    }

    #region Interacting
    private bool isInteracting;
    public bool IsInteracting { get => isInteracting; private set => isInteracting = value; }
    public event Action defineIsInteractingInputEvent;
    private void defineIsInteracting() => defineByGetKey(ref isInteracting, KeyCode.F, defineIsInteractingInputEvent);
    #endregion


    #region Throw
    private bool isThrow;
    public bool IsThrow { get => isThrow; private set => isThrow = value; }
    public event Action defineIsThrowInputEvent;
    private void defineIsThrow() => defineByGetKey(ref isThrow, KeyCode.Q, defineIsThrowInputEvent);
    #endregion


    #region Mix
    private bool isMix;
    public bool IsMix { get => isMix; private set => isMix = value; }
    public event Action defineIsMixInputEvent;
    private void defineIsMix() => defineByGetKey(ref isMix, KeyCode.E, defineIsMixInputEvent);
    #endregion

    #region Respawn
    private bool isRespawning;
    public bool IsRespawning { get => isRespawning; private set => isRespawning = value; }
    public event Action defineIsRespawningInputEvent;
    private void defineIsRespawning() => defineByGetKey(ref isRespawning, KeyCode.Q, defineIsRespawningInputEvent);
    #endregion
    private void Update()
    {
        SetInputAxis();
        defineJumpInput();
        defineIsInteracting();
        defineIsRespawning();
        defineIsThrow();
        defineIsMix();
    }

}
