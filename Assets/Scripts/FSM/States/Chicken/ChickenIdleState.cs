using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenIdleState<T> : CooldownState<T>
{
    private enum IdleStates
    {
        eat,
        turnHead,
        idle
    }
    private RouletteWheel<IdleStates> _roulette = new RouletteWheel<IdleStates>();
    private Dictionary<IdleStates, int> random;
    private ChickenModel _model;
    private FSM<T> _fsm;
    private T _inputFleeing;

    public ChickenIdleState(ChickenModel actor, FSM<T> fsm, T inputFleeing, float time, INode root) : base(time, root)
    {
        _model = actor;
        _root = root;
        random = new Dictionary<IdleStates, int>(); ;
        random[IdleStates.eat] = 5;
        random[IdleStates.turnHead] = 5;
        random[IdleStates.idle] = 2;

    }
    public override void Init()
    {
        _model.LifeController.OnTakeDamage += TakeDamage;
        _model.Move(Vector3.zero);
        _model.Fleeing(Vector3.zero);
        var result = _roulette.Run(random);
        switch (result)
        {
            case IdleStates.eat:
                _model.OnEat?.Invoke();
                break;
            case IdleStates.turnHead:
                _model.OnTurnHead?.Invoke();
                break;
            case IdleStates.idle:
                _model.IdleAnimation();
                break;
            default:
                _model.IdleAnimation();
                break;
        }

    }

    public override void Execute()
    {
        base.Execute();
        if (_model.Target != null)
        {
            if (_model.LineOfSight(_model.Target.transform))
                _root.Execute();
        }
    }

    private void TakeDamage()
    {
        Debug.Log("I TOOK DAMAGE");
        _fsm.Transition(_inputFleeing);
    }

    public override void Exit()
    {
        base.Exit();
        _model.LifeController.OnTakeDamage -= TakeDamage;
    }

}
