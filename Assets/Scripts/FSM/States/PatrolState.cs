using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState<T> : CooldownState<T>
{
    private EntityModel _model;

    public PatrolState(EntityModel model, float runTime, INode root) : base(runTime, root)
    {
        _model = model;
    }

    public override void Init()
    {
        base.Init();
        Debug.Log("Patrol");
    }
}
