using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerPoint : MonoBehaviour
{
    public event EventHandler OnCorrectCheck;
    public event EventHandler OnIncorrectCheck;

    public List<Transform> carTransformList;
    public List<CheckPoint> checkPointsList;
    public List<int> nextCheckPointList;

    private void Awake()
    {
        Transform checksTransform = transform.Find("Checkpoints");
        Debug.Log(checksTransform);

        foreach (Transform checkTransform in checksTransform)
        {
            CheckPoint checkPointSingle = checkTransform.GetComponent<CheckPoint>();
            checkPointSingle.SetTrackCheckPoint(this);

            checkPointsList.Add(checkPointSingle);
        }

        nextCheckPointList = new List<int>();
        foreach (Transform carTransfor in carTransformList)
        {
            nextCheckPointList.Add(0);
        }
    }

    public void CarThrough(CheckPoint checkPoint, Transform carTransform)
    {
        int nextCheckPoint = nextCheckPointList[carTransformList.IndexOf(carTransform)];
        if (checkPointsList.IndexOf(checkPoint) == nextCheckPoint)
        {
            //Debug.Log("Correct");
            nextCheckPointList[carTransformList.IndexOf(carTransform)] = (nextCheckPoint + 1) % checkPointsList.Count;
            //TransformEventArgs transformEvent = new TransformEventArgs();
            //transformEvent.transformCar = carTransform;
            //OnCorrectCheck?.Invoke(this, EventArgs.Equals());
            carTransform.GetComponent<AgentCarContinuous>().TrackerCorrectCheck();
        }
        else
        {
            //Debug.Log("Incorrect");
            //OnIncorrectCheck?.Invoke(this, EventArgs.Empty);
            carTransform.GetComponent<AgentCarContinuous>().TrackerIncorrectCheck();
        }
    }

    public void RestartCheckPoint(Transform carTransform)
    {
        nextCheckPointList[carTransformList.IndexOf(carTransform)] = 0;
    }

    public CheckPoint GetNextCheck(Transform carTransform)
    {
        return checkPointsList[nextCheckPointList[carTransformList.IndexOf(carTransform)]];
    }
}

public class TransformEventArgs : EventArgs
{
    public Transform transformCar { get; set; }
}
