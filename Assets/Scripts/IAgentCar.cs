using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAgentCar
{
    float Steer { get; }
    float Accel { get; }
}