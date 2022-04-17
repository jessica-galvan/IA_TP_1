using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAttackState<T> : State<T>
{
    private IAttackMagic _model;
    private INode _root;
    private float _counter;
    private bool hasAttacked;
    private float _attackDelay;

    public ShootAttackState(IAttackMagic model, INode root = null)
    {
        _model = model;
        _root = root;
        _counter = _model.AttackStats.AttackAnimationTime;
        _attackDelay = _model.AttackStats.AttackAnimationTime - _model.AttackStats.AttackDelay;
        hasAttacked = false;
    }

    public override void Init()
    {
        _model.Attack();
    }

    public override void Execute()
    {
        _counter -= Time.deltaTime;

        _model.transform.LookAt((_model as IArtificialMovement).Target.transform);

        if (!hasAttacked && _counter <=  _attackDelay)
            OnDamageMoment();

        if (_counter <= 0)
            OnFinishedAction();
    }

    private void OnDamageMoment()
    {
        hasAttacked = true;

        _model.ShootBullet();
    }

    private void OnFinishedAction()
    {
        hasAttacked = false;
        if (_root != null)
            _root.Execute();
    }

    public override void Exit()
    {
        _counter = _model.AttackStats.AttackAnimationTime;
        hasAttacked = false;
    }
}
