using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARInteraction : MonoBehaviour
{
    [SerializeField] private float minScale = 0.01f;
    [SerializeField] private float maxScale = 0.2f;
    [SerializeField] private float rotationSpeed = 0.1f;
    [SerializeField] private float scaleSpeed = 0.1f;
    private IEnumerator Start(){
        while(InputManager.Instance == null){
            yield return null;
        }
        InputManager.Instance.OnTouchStarted += HandleTouchStarted;
        InputManager.Instance.OnTouchMoved += HandleTouchMoved;
        InputManager.Instance.OnTouchEnded += HandleTouchEnded;
        InputManager.Instance.OnPinchUpdated += HandlePinchUpdated;
    }

    private void HandleTouchStarted(Vector2 startPosition, Vector2 delta){
       
    }

    private void HandlePinchUpdated(float delta)
    {
        //do the scaling here
        float scaleValue = delta * scaleSpeed * Time.deltaTime;
        var currentScale = transform.localScale;
        // Add the scale value to the current scale
        Vector3 newScale = currentScale + Vector3.one * scaleValue;
        // Make sure to limit the scale so it doesnt grow too big or too small
        newScale = Vector3.Max(Vector3.one * minScale, Vector3.Min(Vector3.one * maxScale, newScale));
        // Set the scale as local
        transform.localScale = newScale;
    }

    private void HandleTouchMoved(Vector2 updatedPosition, Vector2 delta){
        var rotationValue = delta.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotationValue, 0);
       
    }

    private void HandleTouchEnded(Vector2 endPosition, Vector2 delta){

    }
}
