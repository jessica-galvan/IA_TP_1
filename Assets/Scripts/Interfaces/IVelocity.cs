using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVelocity
{
    Transform transform { get; }

    float GetVel { get; }

    Vector3 GetFoward { get; }
}
