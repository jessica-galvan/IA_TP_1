using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState<T> : CooldownState<T>
{
    private IModel _model;

    public IdleState(IModel actor, float timeIdle, INode root = null) : base(timeIdle, root)
    {
        _model = actor;
    }

    public override void Init()
    {
           
        //Debug.Log(_model.gameObject.name + " idle");
        _model.IdleAnimation();
        //TODO: call change animation to idle??
    }

    //aca chequea por input para el player. 
    //En el enemigo, tendria que llamar al line of sight durante el execute, si lo ve, llamar al root. 
}
