using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InputManager : MonoBehaviour
{
    // Singleton implementation
    private static InputManager instance;
    public static InputManager Instance => instance;


    public delegate void TouchEvent(Vector2 position);
    public event TouchEvent OnTouchStarted;
    public event TouchEvent OnTouchEnded;
    public event TouchEvent OnTouchMoved;


    private InputControls inputControls;

    private void Awake()
    {
        // Initialize the inputControls
        inputControls = new InputControls();
        instance = this;
    }

    private void OnEnable()
    {
        inputControls.Enable();
        // since this is an ar app, we want to automatically enable ar input
        // so you are free to enable/disable specific action maps
        // based on your design
          inputControls.ARInput.Enable();
        //  inputControls.VehicleMovement.Disable();
    }

    private void OnDisable()
    {
        inputControls.Disable();  
    }

    private void Start()
    {
        // Since TouchPress is a "Button" action type, it will fire the
        // 'started' event when we get in contact with the screen once
        // You can think of this as the Input.GetKeyDown equivalent for keyboard
        // It will only fire when we press the key once, in this case
        // only once when we touch the screen
        inputControls.ARInput.TouchPress.started += ctx => StartTouch(ctx);
        // only fire when we release
        inputControls.ARInput.TouchPress.canceled += ctx => EndTouch(ctx);
        // Since TouchPosition is a "Pass-through" action type, it will update 
        // the value (which is set as a Vector 2) every frame, and that is
        // accessible through the 'performed' event
        inputControls.ARInput.TouchPosition.performed += ctx => UpdateTouchPosition(ctx);
    }

    private void UpdateTouchPosition(InputAction.CallbackContext context){
        Vector2 contextVal = context.ReadValue<Vector2>();
        Vector2 getVal = GetTouchPosition();
        //Debug.Log($"Touch updated: {getVal} vs {contextVal}");
        OnTouchMoved?.Invoke(GetTouchPosition());
    }

    private void StartTouch(InputAction.CallbackContext context){
        //Debug.LogWarning($"Touch has started at {GetTouchPosition()}");
        OnTouchStarted?.Invoke(GetTouchPosition());
    }

    private void EndTouch(InputAction.CallbackContext context){
        //Debug.LogWarning($"Touch has ended  at {GetTouchPosition()}");
        OnTouchEnded?.Invoke(GetTouchPosition());
    }

    private Vector2 GetTouchPosition(){
        return inputControls.ARInput.TouchPosition.ReadValue<Vector2>();
    }

    
}
