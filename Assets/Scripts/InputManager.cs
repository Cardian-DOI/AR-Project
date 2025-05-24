using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class InputManager : MonoBehaviour
{
    // Singleton implementation
    private static InputManager instance;
    public static InputManager Instance => instance;


    public delegate void TouchEvent(Vector2 position, Vector2 delta);
    public event TouchEvent OnTouchStarted;
    public event TouchEvent OnTouchEnded;
    public event TouchEvent OnTouchMoved;


    public delegate void PinchEvent(float delta);
    public event PinchEvent OnPinchUpdated;

    private InputControls inputControls;
    private Vector2 initialPosition;
    private float previousPinchDistance = 0;
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

        // Enable the EnhancedTouch for it to work
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        inputControls.Disable();  
        EnhancedTouchSupport.Disable();
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
    

        // The below events is similar to the inputControls implementation, except using EnhancedTouch
        Touch.onFingerDown += OnFingerDown;
        Touch.onFingerUp += OnFingerUp;
        Touch.onFingerMove += OnFingerMove;
    }

    private void OnFingerDown(Finger finger){
        initialPosition = finger.screenPosition;
        OnTouchStarted?.Invoke(finger.screenPosition, Vector2.zero);
    }

    private void OnFingerUp(Finger finger){
        var delta = finger.screenPosition - initialPosition;
        OnTouchEnded?.Invoke(finger.screenPosition, delta);
    }

    private void OnFingerMove(Finger finger){
        Debug.Log($"Finger with index {finger.index} is at {finger.screenPosition}");
        var delta = finger.screenPosition - initialPosition;
        OnTouchMoved?.Invoke(finger.screenPosition, delta);
    }

    private void UpdateTouchPosition(InputAction.CallbackContext context){
        Vector2 currentValue = context.ReadValue<Vector2>();
        // Get the difference between the start position and the current position
        var delta = currentValue - initialPosition;
        //OnTouchMoved?.Invoke(currentValue, delta);
    }

    private void StartTouch(InputAction.CallbackContext context){
        //Debug.LogWarning($"Touch has started at {GetTouchPosition()}");
        // Assign the initial position at the start of the touch
        initialPosition = GetTouchPosition();
        //OnTouchStarted?.Invoke(initialPosition, Vector2.zero);
    }

    private void EndTouch(InputAction.CallbackContext context){
        //Debug.LogWarning($"Touch has ended  at {GetTouchPosition()}");
        Vector2 currentValue = context.ReadValue<Vector2>();
         var delta = currentValue - initialPosition;
        //OnTouchEnded?.Invoke(currentValue, delta);
    }


    private void Update()
    {
        if(Touch.activeTouches.Count == 2)
        {
            // Store each touch
            var touch0 = Touch.activeTouches[0];
            var touch1 = Touch.activeTouches[1];

            // Get actual screen position of each touch
            Vector2 currentPos0 = touch0.screenPosition;
            Vector2 currentPos1 = touch1.screenPosition;

            // Get the distance between the two positions
            float currentDistance = Vector2.Distance(currentPos0, currentPos1);

            // Only run the pinch event when there is a change in the pinch distance
            if(previousPinchDistance != 0f){
                float delta = currentDistance - previousPinchDistance;
                OnPinchUpdated?.Invoke(delta);
            }

            // Make sure to update this on the next frame
            previousPinchDistance = currentDistance;
        }
        // If more than 2 or less touch count, dont do anything
        else{
            // Reset when fewere/more touches are active
            previousPinchDistance = 0;
        }   
    }

    private Vector2 GetTouchPosition(){
        return inputControls.ARInput.TouchPosition.ReadValue<Vector2>();
    }

    
}
