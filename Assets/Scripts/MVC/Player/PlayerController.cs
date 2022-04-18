using System;
using UnityEngine;

enum statesPlayer
{
    Idle,
    Move,
    Attack,
    Dead,
    Win
}

public class PlayerController : EntityController
{
    [SerializeField] private float _time = 5f;
    
    //FMS
    private FSM<statesPlayer> _fsm;
    private IState<statesPlayer> _attackState;
    private IState<statesPlayer> _idleState;
    private IState<statesPlayer> _deadState;
    private IState<statesPlayer> _moveState;

    //EVENTS
    public Action OnAttack;
    public Action<float, float> OnMove;
    public Action OnDefend;

    protected override void Awake()
    {
        _model = GetComponent<PlayerModel>();
    }

    protected override void Start()
    {
        base.Start();
        InitializedFSM();
    }

    private void InitializedFSM()
    {
        _fsm = new FSM<statesPlayer>();

        _idleState = new IdlePlayerState<statesPlayer>(_model as PlayerModel, _fsm, statesPlayer.Attack, statesPlayer.Move, statesPlayer.Dead);
        _attackState = new AttackPlayerState<statesPlayer>(_model as PlayerModel, _fsm,statesPlayer.Idle,statesPlayer.Move, _model.ActorStats.OffSetToCenter);
        _moveState = new MovePlayerState<statesPlayer>(_model as PlayerModel, _fsm, statesPlayer.Attack, statesPlayer.Idle);
        _deadState = new DeadState<statesPlayer>(_model, _time);

        _idleState.AddTransition(statesPlayer.Move, _moveState);
        _idleState.AddTransition(statesPlayer.Attack, _attackState);
        _idleState.AddTransition(statesPlayer.Dead, _deadState);

        _attackState.AddTransition(statesPlayer.Move, _moveState);
        _attackState.AddTransition(statesPlayer.Idle, _idleState);

        _moveState.AddTransition(statesPlayer.Attack, _attackState);
        _moveState.AddTransition(statesPlayer.Idle, _idleState);

        _fsm.SetInit(_idleState);
    }

    void Update()
    {
        _fsm.OnUpdate();
    }
}
