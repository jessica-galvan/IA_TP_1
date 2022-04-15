using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState<T> : State<T>
{
    private IAttack _model;
    private INode _root;
    private float _counter;
    private float _attackDelayTimer;
    private bool hasAttacked;

    public AttackState(IAttack model, INode root = null)
    {
        _model = model;
        _root = root;
    }

    public override void Init()
    {
        if (_model.CanAttack) //Check si puede estar acá, sino volve al root
        {
            _model.Attack();
            _counter = _model.AttackStats.AttackAnimationTime;
            hasAttacked = false;
        } else
        {
            if (_root != null)
                _root.Execute();
        }
    }

    public override void Execute()
    {
        _counter -= Time.deltaTime;

        if (!hasAttacked && _counter <= _attackDelayTimer)
            OnDamageMoment();

        if (_counter <= 0)
        {
            _counter = _model.AttackStats.AttackAnimationTime;
            OnFinishedAction();
        }
    }

    public void OnDamageMoment()
    {
        hasAttacked = true;
        //call to damage instantiation;
    }

    public void OnFinishedAction()
    {
        if (_root != null)
            _root.Execute();
    }
}
