using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject scanningPanel;

    private IEnumerator Start(){
        while(ImageTrackerManager.Instance == null){
            yield return null;
        }
        ImageTrackerManager.Instance.OnImageTrackerDetected += HandleImageTrackerDetected;
        ImageTrackerManager.Instance.OnImageTrackerLost += HandleImageTrackerLost;
    }

    private void HandleImageTrackerDetected(string trackerId){
        HandleTrackingStarted();
    }

    private void HandleImageTrackerLost(string trackerId){
        HandleTrackingLost();
    }



    private void HandleTrackingStarted(){
        scanningPanel.SetActive(false);
        Debug.LogWarning("Tracking ui started");
    }

    private void HandleTrackingLost(){
        scanningPanel.SetActive(true);
        Debug.LogWarning("Tracking ui lost");
    }
}
