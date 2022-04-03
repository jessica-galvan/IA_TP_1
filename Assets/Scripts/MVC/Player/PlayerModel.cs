using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : EntityModel, IAttack, IModel
{
    //Variables
    protected float cooldownTimer;

    //PROPIERTIES
    public bool CanAttack { get; private set; }

    //EVENTS
    public Action OnAttack { get => _onAttack; set => _onAttack = value; }

    private Action _onAttack = delegate { };

    private Action OnMove;

    protected override void Awake()
    {
        base.Awake();
        
    }

    protected override void Start()
    {
        base.Start();
        InputController.instance.OnAttack += Attack;
    }

    public void Attack()
    {
        OnAttack?.Invoke();
    }

    public void Move(float x, float y)
    {
        if (x > 0 || y > 0)
            OnMove?.Invoke();
    }

    public void DamagePoint()
    {
        OnAttack?.Invoke();
    }

    public bool CheckTargetInFront()
    {
        return Physics.Raycast(transform.position, transform.forward, _actorStats.RangeVision, _attackStats.TargetList);
    }

    public Transform[] CheckTargetsInRadious()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, _actorStats.RangeVision, _attackStats.TargetList);
        Transform[] targets = new Transform[colls.Length];

        for (int i = 0; i < colls.Length; i++)
        {
            targets[i] = colls[i].transform;
        }
        return targets;
        //TODO: ACOMODARLO PARA EL PLAYER.
    }
}
