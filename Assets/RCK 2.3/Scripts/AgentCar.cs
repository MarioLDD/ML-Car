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
    void Start()
    {
        trackerPoint.OnCorrectCheck += TrackerCorrectCheck;
        trackerPoint.OnIncorrectCheck += TrackerIncorrectCheck;

        Rigidbody rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        
        if (rb != null)
        {
            if (rb.velocity.z < 0)
            {
                Debug.Log("Rb Velocity negativa!!");
                
                AddReward(-0.1f);

            }
        }

    }

    public void TrackerCorrectCheck(object sender, EventArgs e)
    {
        Debug.Log("CorrectCheckpoint = 1");
        AddReward(1f);
       
    }

    public void TrackerIncorrectCheck(object sender, EventArgs e)
    {
        Debug.Log("IncorrectCheck = -1");
        AddReward(-1);

        EndEpisode();
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("Comenzando!!!");
        transform.position = spawnPos.position;
        transform.rotation = spawnPos.rotation;

        trackerPoint.RestartCheckPoint();

        Contador.Instance.AddSteps();

    }

    

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 nextCheckPoint = trackerPoint.GetNextCheck().transform.forward;
        float directionDot = Vector3.Dot(transform.forward, nextCheckPoint);

        sensor.AddObservation(directionDot);
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
            Debug.Log("Wall = -1");
            AddReward(-1);

            EndEpisode();
            
        }
 
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
