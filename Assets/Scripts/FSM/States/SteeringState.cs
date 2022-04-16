using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringState<T> : State<T>
{
    private IArtificialMovement _model;
    private INode _root;

    public SteeringState(IArtificialMovement model, INode root)
    {
        _model = model;
        _root = root;
    }

    public override void Init()
    {
        base.Init();
    }

    public override void Execute()
    {
        if (!_model.CheckIsInRange() && _model.CheckIsTooFar()) //Si no estamos en rango de atacar o estamos a muuucha distancia (el player hullo!)....
        { //TODO: check si esta en line of sight tambien. Si lo deja de ver, que frene. 
            Vector3 dir = (_model.Avoidance.GetDir() * _model.IAStats.AvoidanceWeight + _model.Steering.GetDir() * _model.IAStats.SteeringWeight).normalized; //el avoidance puede ir adentro del state chase por ejemplo. o el seek, pursuit, flee, etc. 
            _model.LookDir(dir);
            _model.Move(dir);
        }
        else
        {
            _root.Execute();
        }

    }

}
