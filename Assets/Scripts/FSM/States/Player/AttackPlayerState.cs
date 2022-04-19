using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayerState<T> : State<T>
{
    private PlayerModel _model;
    private float _counter;
    private float _attackDelayTimer;
    private bool hasAttacked;

    private Vector3 offsetToCenter = new Vector3(0, 0.5f, 0);
    private FSM<T> _fsm;
    private T _inputIdle;
    private T _inputMove;

    public AttackPlayerState(PlayerModel model, FSM<T> fsm, T inputIdle, T inputMove,  Vector3 offset)
    {
        _model = model;
        _fsm = fsm;
        _inputIdle = inputIdle;
        _inputMove = inputMove;
        offsetToCenter = offset;
    }

    public override void Init()
    {
        //Nos suscribimos a los eventos
        InputController.instance.OnMove += OnMove; 
        _model.LifeController.OnDie += OnDie;
        
        _model.Attack(); //Ejecutamos la animacion

        //Reset
        hasAttacked = false;
        _model.Rb.velocity = Vector3.zero;
        _counter = _model.AttackStats.AttackAnimationTime;
    }

    public override void Execute()
    {
        InputController.instance.PlayerUpdate(); //Checkeo de inputs

        _counter -= Time.deltaTime;

        if (!hasAttacked && _counter <= _attackDelayTimer)
            OnDamageMoment();

        if (_counter <= 0)
        {
            OnFinishedAction();
        }
    }

    public void OnDamageMoment()
    {
        hasAttacked = true;

        if (Physics.Raycast(_model.transform.position + offsetToCenter, _model.transform.forward, out RaycastHit hit, _model.AttackStats.AttackRadious, _model.AttackStats.TargetList))
        {
            LifeController life = hit.collider.GetComponent<LifeController>();
            if (life != null)
                life.TakeDamage(_model.AttackStats.Damage);
        }
    }

    private void OnMove(Vector3 move)
    {
        if (_counter <= 0 && move != Vector3.zero)
            _fsm.Transition(_inputMove);
    }

    private void OnDie()
    {
        if (_model.LifeController.IsDead)
            _fsm.Transition(_inputIdle);
    }

    public void OnFinishedAction()
    {
        if (InputController.instance.IsMoving)
            _fsm.Transition(_inputMove);
        else
            _fsm.Transition(_inputIdle);
    }

    public override void Exit()
    {
        _counter = _model.AttackStats.AttackAnimationTime;
        InputController.instance.OnMove -= OnMove;
        _model.LifeController.OnDie -= OnDie;
    }
}
