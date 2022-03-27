using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseController : EntityController
{
    protected enum states
    {
        Idle,
        Run,
        Attack,
        Dead,
    }

    protected EnemyBaseModel _model;
    protected FSM<states> _fsm;
    protected float time = 5f;

    private void Awake()
    {
        _model = GetComponent<EnemyBaseModel>();
        InitializedFSM();
    }
    void InitializedFSM()
    {
        _fsm = new FSM<states>();

        var attack = new AttackState<states>(_model, time, _fsm, states.Idle);
        var idle = new IdleState<states>(_model, time, _fsm, states.Attack);
        var run = new RunState<states>(_model, time, _fsm, states.Idle);
        var dead = new DeadState<states>(_model, time);

        attack.AddTransition(states.Dead, dead);
        attack.AddTransition(states.Run, run);

        idle.AddTransition(states.Run, run);
        idle.AddTransition(states.Dead, dead);

        run.AddTransition(states.Attack, attack);

        _fsm.SetInit(idle);
    }
    void Update()
    {
        _fsm.OnUpdate();
    }
}
