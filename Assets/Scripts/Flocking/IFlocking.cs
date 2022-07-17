using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFlocking
{
    public float Multiplier { get; set; }
    public Vector3 GetDir(List<Transform> boids, Transform self);
}
