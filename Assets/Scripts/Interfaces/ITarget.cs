using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget //Si implementa esta interface, IArtificialMovement puede utilizar algun steering contra dicho target
{
    Transform transform { get; }

    float Velocity { get; }

    Vector3 GetFoward { get; }

    void Move(float x, float y);

    Action<bool> OnMove { get; set; }
}
