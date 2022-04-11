using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArtificialMovement //Es una IA con Movimiento, puede utilizar Flee, Seek, Pursuit, Escape y/ Avoidance contra un ITarget
{
    IAStats IAStats { get; }
    Transform transform { get; }
    ITarget Target { get; }
    ISteering Steering { get; }
    ISteering Avoidance { get; }
  
    Action<bool> OnMove { get; set; }

    void Move(Vector3 dir);
    void LookDir(Vector3 dir);
    void SetNewSteering(ISteering newSteering);
    bool CheckIsInRange();
    bool CheckIsTooFar();
}
