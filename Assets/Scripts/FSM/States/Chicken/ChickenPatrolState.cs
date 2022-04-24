using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenPatrolState<T> : PatrolState<T>
{
    private FSM<T> _fsm;
    private T _inputFleeing;
    public ChickenPatrolState(IPatrol model, FSM<T> fsm, T inputFleeing, INode root, bool canRevert = false) : base(model, root, canRevert)
    {
        _fsm = fsm;

    }

    public override void Init()
    {
        base.Init();
        (_model as IModel).LifeController.OnTakeDamage += TakeDamage;
    }

    private void TakeDamage()
    {
        _fsm.Transition(_inputFleeing);
    }

    public override void Exit()
    {
        base.Exit();
        (_model as IModel).LifeController.OnTakeDamage -= TakeDamage;
        _model.Move(Vector3.zero);
    }
}
