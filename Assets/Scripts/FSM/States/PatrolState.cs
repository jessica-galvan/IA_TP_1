using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState<T> : CooldownState<T>
{
    private IModel _model;

    public PatrolState(IModel model, float runTime, INode root = null) : base(runTime, root)
    {
        _model = model;
    }

    public override void Init()
    {
        base.Init();
        //TODO: call change animation to walk/run + initialized patrol route in enemies
    }
}
