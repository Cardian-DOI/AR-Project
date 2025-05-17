using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARInteraction : MonoBehaviour
{
    // To handle rotation for the quiz,
    // Store the initial value
    // Update the rotation everytime the user drags left/right
    // You can determine the direction of the movement by comparing it to the initial position
    // Make sure to add a rotationSpeed so it will rotate at just the right spped

    private IEnumerator Start(){
        while(InputManager.Instance == null){
            yield return null;
        }
        InputManager.Instance.OnTouchStarted += HandleTouchStarted;
        InputManager.Instance.OnTouchMoved += HandleTouchMoved;
        InputManager.Instance.OnTouchEnded += HandleTouchEnded;
    }

    private void HandleTouchStarted(Vector2 startPosition){
        // Get the initial position
    }

    private void HandleTouchMoved(Vector2 updatedPosition){
        // Do something with the updated position
        float zPos = transform.position.z;
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(updatedPosition.x, updatedPosition.y, zPos));
        targetPosition.z = zPos;
        transform.position = targetPosition;
    }

    private void HandleTouchEnded(Vector2 endPosition){

    }
}
