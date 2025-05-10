using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageTrackerManager : MonoBehaviour
{
    // For Singleton purposes
    private static ImageTrackerManager instance;
    public static ImageTrackerManager Instance => instance;

    // Event that will be called when an image tracking is detected
    // It will pass in the name of the tracker id detected
    public System.Action<string> OnImageTrackerDetected;
    public System.Action<string> OnImageTrackerLost;

    // Event that will be called when any image is tracked
    public System.Action OnAnyImageTracked;
    // Event that will be called when there is no more active image tracking
    public System.Action OnImageTrackingEnded;

    // Just a collection of trackIds that are currently being tracked
    private readonly List<string> trackedIds = new();

    // For singleton purposes
    private void Awake()
    {
        instance = this;
    }

    // Add the trackId and call respective event when image is tracked
    public void HandleImageTrackingDetected(string trackerId){
        trackedIds.Add(trackerId);
        OnImageTrackerDetected?.Invoke(trackerId);
        OnAnyImageTracked?.Invoke();
    }

    // Remove the trackId and call respective event when image is tracked
    public void HandleImageTrackingLost(string trackerId){
        OnImageTrackerLost?.Invoke(trackerId);
         Debug.LogWarning($"Lost {trackerId}");
        if(trackedIds.Contains(trackerId)){
            trackedIds.Remove(trackerId);
        }
        if(trackedIds.Count == 0){
            OnImageTrackingEnded?.Invoke();
        }
    }
}
