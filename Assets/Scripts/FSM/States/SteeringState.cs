using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringState<T> : State<T>
{
    private IArtificialMovement _model;

    public SteeringState(IArtificialMovement model, INode root = null)
    {
        _model = model;
    }

    public override void Execute()
    {
        Vector3 dir = (_model.Avoidance.GetDir() * _model.IAStats.AvoidanceWeight + _model.Steering.GetDir() * _model.IAStats.SteeringWeight).normalized; //el avoidance puede ir adentro del state chase por ejemplo. o el seek, pursuit, flee, etc. 
        _model.LookDir(dir);
        _model.Move(dir);
    }

}
