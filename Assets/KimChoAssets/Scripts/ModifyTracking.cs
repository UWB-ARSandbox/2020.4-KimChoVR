using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;

public class ModifyTracking : MonoBehaviour
{
    public TrackedPoseDriver trackPoseScript;

    // Update is called once per frame
    void Update()
    {
        if (trackPoseScript == null)
        {
            trackPoseScript = this.GetComponent<TrackedPoseDriver>();
        } else if (trackPoseScript != null && trackPoseScript.trackingType == TrackedPoseDriver.TrackingType.RotationAndPosition)
        {
            trackPoseScript.trackingType = TrackedPoseDriver.TrackingType.RotationOnly;
        }
    }
}
