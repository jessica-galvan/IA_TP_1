using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePlayerState<T> : State<T>
{
    private PlayerModel _model;
    private FSM<T> _fsm;
    private T _inputDead;
    private T _inputMove;
    private T _inputAttack;

    public IdlePlayerState(PlayerModel model, FSM<T> fsm, T inputAttack, T inputMove, T inputDead)
    {
        _model = model;
        _fsm = fsm;
        _inputDead = inputDead;
        _inputAttack = inputAttack;
        _inputMove = inputMove;
    }

    public override void Init() 
    {
        _model.IdleAnimation();
        Die(); //On every entry to idle, check once if player is dead, cuz the other states don't!

        //Event Suscription
        InputController.instance.OnAttack += Attack;
        InputController.instance.OnMove += Move;
        _model.LifeController.OnDie += Die;
    }

    public override void Execute()
    {
        InputController.instance.PlayerUpdate(); 
    }

    public void Attack()
    {
        if (_model.CanAttack)
            _fsm.Transition(_inputAttack);
    }

    public void Move(Vector3 move)
    {
        if (move != Vector3.zero)
            _fsm.Transition(_inputMove);
    }

    public void Die()
    {
        if (_model.LifeController.IsDead)
            _fsm.Transition(_inputDead);
    }

    public override void Exit()
    {
        base.Exit();
        InputController.instance.OnAttack -= Attack;
        InputController.instance.OnMove -= Move;
        _model.LifeController.OnDie -= Die;
    }
}
