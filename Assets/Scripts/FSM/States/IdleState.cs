using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState<T> : CooldownState<T>
{
    private EntityModel _model;

    public IdleState(EntityModel actor, float timeIdle, INode root = null) : base(timeIdle, root)
    {
        _model = actor;
    }

    public override void Init()
    {
        Debug.Log(_model.gameObject.name + " idle");
        //TODO: call change animation to idle??
    }
}
