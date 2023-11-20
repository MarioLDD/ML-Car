using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;

public class AgentCarContinuous : Agent, IAgentCar
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
        //trackerPoint.OnCorrectCheck += TrackerCorrectCheck;
        //trackerPoint.OnIncorrectCheck += TrackerIncorrectCheck;

        Rigidbody rb = GetComponent<Rigidbody>();
        //StartCoroutine(VelocityRewards());

    }

    //private void FixedUpdate()
    //{

    //    if (rb != null)
    //    {
    //        if (rb.velocity.z < 0)
    //        {
    //            Debug.Log("Rb Velocity negativa!!");

    //            AddReward(-0.1f);

    //        }
    //    }

    //}

    public void TrackerCorrectCheck()
    {
        Debug.Log("CorrectCheckpoint = 1");
        AddReward(1f);

    }

    public void TrackerIncorrectCheck()
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

        trackerPoint.RestartCheckPoint(this.transform);

        Contador.Instance.AddSteps();

    }



    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 nextCheckPoint = trackerPoint.GetNextCheck(this.transform).transform.forward;
        float directionDot = Vector3.Dot(transform.forward, nextCheckPoint);
        sensor.AddObservation(trackerPoint.GetNextCheck(this.transform).transform.position);
        sensor.AddObservation(trackerPoint.GetNextCheck(this.transform).transform.position - this.transform.position);
        sensor.AddObservation(directionDot);
        sensor.AddObservation(transform.position);
        sensor.AddObservation(rb.velocity);

    }

    public override void OnActionReceived(ActionBuffers actions)
    {

        accel = actions.ContinuousActions[0];
        steer = actions.ContinuousActions[1];
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float floatVertical = Input.GetAxis("Vertical");
        ActionSegment<float> continuousActionsOut = actionsOut.ContinuousActions;


        continuousActionsOut[1] = moveHorizontal;
        continuousActionsOut[0] = floatVertical;
    }

    private IEnumerator VelocityRewards()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (rb.velocity.z > 0.5f)
            {
                AddReward(rb.velocity.magnitude * 0.01f);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Debug.Log("Wall = -1");
            AddReward(-10);

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
