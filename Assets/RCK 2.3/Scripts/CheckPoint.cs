using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]private TrackerPoint trackerPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.parent.TryGetComponent<AgentCar>(out AgentCar agentCar))
        {
            //Debug.Log(other.transform.parent.parent.name);
            trackerPoint.CarThrough(this, other.transform.parent.parent);
        }
    }

    public void SetTrackCheckPoint(TrackerPoint trackerPoint)
    {
        this.trackerPoint = trackerPoint;
    }
}
