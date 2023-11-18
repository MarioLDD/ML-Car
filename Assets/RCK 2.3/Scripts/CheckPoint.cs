using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]private TrackerPoint trackerPoint;

    private void OnTriggerEnter(Collider other)
    {
        trackerPoint.CarThrough(this);
    }

    public void SetTrackCheckPoint(TrackerPoint trackerPoint)
    {
        this.trackerPoint = trackerPoint;
    }
}
