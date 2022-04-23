using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerState<T> : State<T>
{
    private PlayerModel _model;
    private FSM<T> _fsm;
    private T _inputIdle;
    private T _inputAttack;

    public MovePlayerState(PlayerModel model, FSM<T> fsm, T inputAttack, T inputIdle)
    {
        _model = model;
        _fsm = fsm;
        _inputIdle = inputIdle;
        _inputAttack = inputAttack;
    }

    public override void Init() //Me suscribo al move y al attack
    {
        InputController.instance.OnAttack += Attack;
        InputController.instance.OnMove += Move;
        _model.LifeController.OnDie += Die;
    }

    public override void Execute() 
    {
        InputController.instance.PlayerUpdate(); //Llamo al playupdate del input controller para chequear si se esta moviendo, si esta en idle o si esta atacando.
    }

    public void Attack()
    {
        if(_model.CanAttack)
            _fsm.Transition(_inputAttack);
    }

    public void Idle()
    {
        _fsm.Transition(_inputIdle);
    }

    public void Move(Vector3 direction)
    {
        if (direction == Vector3.zero)
            Idle();

        direction.y = 0;
        direction = direction.normalized;

        if (direction.magnitude >= 0.1f)
        {
            _model.Rb.velocity = direction * _model.ActorStats.Speed;
            _model.transform.forward = Vector3.Lerp(_model.transform.forward, direction, _model.ActorStats.TurnSpeed);
        }

        _model.Move();
    }

    public void Die() //vuelven a Idle parar morir
    {
        if(_model.LifeController.IsDead)
            _fsm.Transition(_inputIdle);
    }

    public override void Exit()
    {
        base.Exit();
        InputController.instance.OnAttack -= Attack;
        InputController.instance.OnMove -= Move;
        _model.LifeController.OnDie -= Die;
    }
}
