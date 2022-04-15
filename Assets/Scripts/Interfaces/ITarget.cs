using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget //Si implementa esta interface, IArtificialMovement puede utilizar algun steering contra dicho target
{
    Transform transform { get; }

    Rigidbody Rb { get; }

    float Velocity { get; }

    Vector3 GetFoward { get; }

    void Move();

    Action OnMove { get; set; }
}
