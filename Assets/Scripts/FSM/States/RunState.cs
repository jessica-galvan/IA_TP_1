using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState<T> : CooldownState<T>
{
    private EntityModel _model;

    public RunState(EntityModel model, float runTime, FSM<T> fsm, T input) : base(runTime, fsm, input)
    {
        _model = model;
    }

    public override void Init()
    {
        base.Init();
        Debug.Log(_model.name + " run");
    }
}
