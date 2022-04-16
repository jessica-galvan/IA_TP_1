using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHitState<T> : CooldownState<T>
{
    private IModel _model;

    public GetHitState(IModel model, float runTime, INode root = null) : base(runTime, root)
    {
        _model = model;
    }

    public override void Init()
    {
        base.Init();
        _model.GetHitAnimation();
    }

    public override void Execute()
    {
        base.Execute();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
