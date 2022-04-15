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
        //GameManager.instance.SetPlayer(this);
    }

    protected override void SubscribeEvents() //El input se recibe en el controller
    {
        base.SubscribeEvents();
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

    //private void Move(Vector3 movement) //TODO: Mover todo esto a un PlayerMoveState. Le pasaria el inputController. 
    //{   
    //    if(movement != Vector3.zero)
    //    {
    //        if (_fsm.GetCurrentState != _moveState)
    //            _fsm.Transition(statesPlayer.Move);
    //    }

    //    if (_fsm.GetCurrentState == _moveState) //if the character is running, the move. Because else, it moves when attacking
    //        (_model as ITarget).Move(movement);
    //}

    //private void Attack()
    //{
    //    if (_fsm.GetCurrentState != _attackState)
    //    {
    //        _fsm.Transition(statesPlayer.Attack);
    //        OnAttack?.Invoke();
    //    }
    //}

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

    //protected override void OnDestroy()
    //{
    //    base.OnDestroy();
    //    InputController.instance.OnMove -= Move;
    //    InputController.instance.OnAttack -= Attack;
    //    //InputController.instance.OnJump -= OnJump;
    //    //InputController.instance.OnDefend -= OnDefend;
    //}
}
