using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerPoint : MonoBehaviour
{
    public event EventHandler OnCorrectCheck;
    public event EventHandler OnIncorrectCheck;
    public List<CheckPoint> checkPointsList;
    private int nextCheckPoint;

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

        nextCheckPoint = 0;
    }

    public void CarThrough(CheckPoint checkPoint)
    {
        if (checkPointsList.IndexOf(checkPoint) == nextCheckPoint)
        {
            //Debug.Log("Correct");
            nextCheckPoint = (nextCheckPoint + 1) % checkPointsList.Count;
            OnCorrectCheck?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            //Debug.Log("Incorrect");
            OnIncorrectCheck?.Invoke(this, EventArgs.Empty);
        }
    }

    public void RestartCheckPoint()
    {
        nextCheckPoint = 0;
    }

    public CheckPoint GetNextCheck()
    {
        return checkPointsList[nextCheckPoint];
    }
}
