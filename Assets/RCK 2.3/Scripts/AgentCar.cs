using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;

public class AgentCar : Agent
{
    [SerializeField] TrackerPoint trackerPoint;
    [SerializeField] Transform spawnPos;
    [SerializeField] Rigidbody rb;

    private float steer;
    private float accel;

    public float Steer { get => steer; }
    public float Accel { get => accel; }

    float nextPointDistance;
    void Start()
    {
        //trackerPoint.OnCorrectCheck += TrackerCorrectCheck;
        //trackerPoint.OnIncorrectCheck += TrackerIncorrectCheck;

        Rigidbody rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, trackerPoint.GetNextCheck(this.transform).transform.position);
        if (distance < nextPointDistance)
        {
            //Debug.Log(1/distance);
            AddReward(1/distance);
        }
        else if (distance > nextPointDistance)
        {
            //Debug.Log("Mas disntancia");
            AddReward(1/distance * -1f);
        }
        nextPointDistance = distance;
    }

    public void TrackerCorrectCheck()
    {
        //Debug.Log(transformEvent.transformCar.name);
        Debug.Log("CorrectCheckpoint = 1");
        nextPointDistance = Vector3.Distance(transform.position, trackerPoint.GetNextCheck(this.transform).transform.position);
        AddReward(1f);
    }

    public void TrackerIncorrectCheck()
    {
        Debug.Log("IncorrectCheck = -1");
        AddReward(-1);

        //EndEpisode();
    }
    //public void TrackerIncorrectCheck(object sender, EventArgs e)
    //{
    //    Debug.Log("IncorrectCheck = -1");
    //    AddReward(-1);

    //    //EndEpisode();
    //}

    public override void OnEpisodeBegin()
    {
        Debug.Log("Comenzando!!!");
        transform.position = spawnPos.position;
        transform.rotation = spawnPos.rotation;

        trackerPoint.RestartCheckPoint(this.transform);
        accel = 0;
        Contador.Instance.AddSteps();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 nextCheckPoint = trackerPoint.GetNextCheck(this.transform).transform.forward;
        float directionDot = Vector3.Dot(transform.forward, nextCheckPoint);
        sensor.AddObservation(directionDot);
        sensor.AddObservation(trackerPoint.GetNextCheck(this.transform).transform.position);
        sensor.AddObservation(transform.position);
        //Debug.Log(trackerPoint.GetNextCheck(this.transform).transform.position);
        //Debug.Log(directionDot);
        var localVelocity = transform.InverseTransformDirection(rb.velocity);
        sensor.AddObservation(localVelocity.x);
        sensor.AddObservation(localVelocity.z);
        sensor.AddObservation(rb.velocity.magnitude);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        switch (actions.DiscreteActions[0])
        {
            case 0:
                accel = 0;
                break;
            case 1:
                accel = 1;
                break;
            case 2:
                accel = -1;
                break;
        }
        switch (actions.DiscreteActions[1])
        {
            case 0:
                steer = Mathf.MoveTowards(steer, 0, 0.2f) ;
                break;
            case 1:
                steer = Mathf.MoveTowards(steer, 1, 0.2f);
                break;
            case 2:
                steer = Mathf.MoveTowards(steer, -1, 0.2f);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Wall"))
        {
            Debug.Log("Wall = -5");
            AddReward(-5);

            EndEpisode();
        }
 
    }

    IEnumerator DistanceCarCheckPoint()
    {
        yield return new WaitForSeconds(1.0f);
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Checkpoints"))
    //    {
    //        Debug.Log("Wall = -0.1f");
    //        AddReward(-0.1f);
    //    }
    //}
}
