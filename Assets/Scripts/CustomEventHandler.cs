using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEventHandler : DefaultObserverEventHandler
{
   [SerializeField] private string trackerId;


   protected override void OnTrackingFound()
   {
        // Call the custom ImageTrackerManager to invoke the event
        ImageTrackerManager.Instance.HandleImageTrackingDetected(trackerId);
        base.OnTrackingFound();
   }

   protected override void OnTrackingLost()
   {
        // Call the custom ImageTrackerManager to invoke the event
        ImageTrackerManager.Instance.HandleImageTrackingLost(trackerId);
        base.OnTrackingLost();
   }
}
