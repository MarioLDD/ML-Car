using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
public class AgentCar : Agent
{
    private float steer;
    private float accel;

    public float Steer { get => steer; }
    public float Accel { get => accel; }
    void Start()
    {

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

    public override void CollectObservations(VectorSensor sensor)
    {


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Wall"))
        {
            AddReward(-1);
        }
        else if(other.CompareTag("Checkpoints"))
        {
            AddReward(1);
        }
    }


}
