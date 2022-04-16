using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPatrol : IArtificialMovement
{
    GameObject[] PatrolRoute { get; }

    bool CanReversePatrol { get; }
}
