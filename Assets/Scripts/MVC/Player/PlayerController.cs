using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{
    enum states
    {
        Idle,
        Run,
        Attack,
        Dead, 
        Win
    }

    [SerializeField] private float _time = 5f;
    
    //FMS
    private FSM<states> _fsm;
    private IState<states> _attackState;
    private IState<states> _idleState;
    private IState<states> _deadState;
    private IState<states> _runState;

    //EVENTS
    public Action OnAttack;
    public Action<float, float> OnMove;
    public Action OnDefend;

    protected override void Awake()
    {
        _model = GetComponent<PlayerModel>();
        InitializedFSM();
    }

    protected override void Start()
    {
        base.Start();
        //GameManager.instance.SetPlayer(this);
    }

    protected override void SubscribeEvents() //El input se recibe en el controller
    {
        base.SubscribeEvents();
        InputController.instance.OnMove += Move;
        InputController.instance.OnAttack += Attack;
        //InputController.instance.OnJump += OnJump;
        //InputController.instance.OnDefend += OnDefend;
    }

    private void InitializedFSM()
    {
        _fsm = new FSM<states>();

        _attackState = new AttackState<states>((_model as IAttack), _time);
        _idleState = new IdleState<states>(_model, _time);
        _runState = new PatrolState<states>(_model, _time);
        _deadState = new DeadState<states>(_model, _time);

        _attackState.AddTransition(states.Dead, _deadState);
        _attackState.AddTransition(states.Run, _runState);
        _attackState.AddTransition(states.Idle, _idleState);

        _idleState.AddTransition(states.Run, _runState);
        _idleState.AddTransition(states.Attack, _attackState);
        _idleState.AddTransition(states.Dead, _deadState);

        _runState.AddTransition(states.Attack, _attackState);
        _runState.AddTransition(states.Idle, _idleState);
        _runState.AddTransition(states.Dead, _deadState);

        _fsm.SetInit(_idleState);
    }

    void Update()
    {
        _fsm.OnUpdate();
    }

    private void Move(float x, float y) //TODO: Make player stop moving when attacking
    {   

        if (_fsm.GetCurrentState != _runState)
        {
            _fsm.Transition(states.Run);
        }

        if (_fsm.GetCurrentState == _runState) //if the character is running, the move. Because else, it moves when attacking
            (_model as ITarget).Move(x, y);
    }

    private void Attack()
    {
        if (_fsm.GetCurrentState != _attackState)
        {
            _fsm.Transition(states.Attack);
            OnAttack?.Invoke();
        }
    }

    //private void Defend()
    //{
    //    OnDefend?.Invoke();
    //    if (_fsm.GetCurrentState != _defendState)
    //      _fsm.Transition(states.Defend);
    //}

    //private void Jump()
    //{
    //    OnJump?.Invoke();
    //}

    protected override void OnDestroy()
    {
        base.OnDestroy();
        InputController.instance.OnMove -= Move;
        InputController.instance.OnAttack -= Attack;
        //InputController.instance.OnJump -= OnJump;
        //InputController.instance.OnDefend -= OnDefend;
    }
}
