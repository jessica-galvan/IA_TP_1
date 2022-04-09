using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMove : IVelocity
{
    void Move(float x, float y);

    Action<bool> OnMove { get; set; }
}
