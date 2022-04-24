using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState<T> : CooldownState<T>
{
    private IArtificialMovement _model;

    public IdleState(IArtificialMovement actor, float time, INode root) : base(time, root)
    {
        _model = actor;
        _root = root;
    }

    public override void Init()
    {
        _model.IdleAnimation();
    }

    public override void Execute()
    {
        base.Execute();
        if(_model.Target != null)
        {
            if (_model.LineOfSight(_model.Target.transform)) //En el enemigo, tendria que llamar al line of sight durante el execute, si lo ve, llamar al root. 
                _root.Execute();
        }
    }    

}
