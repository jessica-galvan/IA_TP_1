using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenFleeingState<T> : State<T>
{
    private ChickenModel _model;
    private INode _root;

    public ChickenFleeingState(ChickenModel model, INode root)
    {
        _model = model;
        _root = root;

    }

    public override void Init()
    {
        base.Init();
        _model.OnMove?.Invoke(false);
    }

    public override void Execute()
    {
        if (_model.CheckIsStillTooNear()) 
        {
            Vector3 dir = (_model.Avoidance.GetDir() * _model.IAStats.AvoidanceWeight + _model.Steering.GetDir() * _model.IAStats.SteeringWeight).normalized; //el avoidance puede ir adentro del state chase por ejemplo. o el seek, pursuit, flee, etc. 
            _model.LookDir(dir);
            _model.Fleeing(dir);
        }
        else
        {
            _root.Execute();
        }
    }

    public override void Exit()
    {
        base.Exit();
        _model.Fleeing(Vector3.zero);
        _model.OnFleeing.Invoke(false);
    }
}
