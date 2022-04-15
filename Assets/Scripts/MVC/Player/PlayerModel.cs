using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerModel : EntityModel, IAttack, ITarget
{
    //VARIABLES
    protected float cooldownTimer;
    protected Rigidbody _rb;

    //PROPIERTIES
    public Rigidbody Rb { get => _rb; }
    public bool CanAttack { get; private set ; }
    public float Velocity => _rb.velocity.magnitude;
    public Vector3 GetFoward => transform.forward;

    //EVENTS
    private Action _onAttack = delegate { };
    private Action _onMove = delegate { };

    public Action OnAttack { get => _onAttack; set => _onAttack = value; }
    public Action OnMove { get => _onMove; set => _onMove = value; }

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        CheckCanAttack();
    }

    protected void CheckCanAttack()
    {
        if (!CanAttack)
        {
            if (cooldownTimer > 0)
                cooldownTimer -= Time.deltaTime;
            else
                CanAttack = true;
        }
    }

    public void Attack()
    {
        if (CanAttack)
        {
            CanAttack = false;
            cooldownTimer = _attackStats.Cooldown;
            OnAttack?.Invoke();
        }
    }

    public void Move()
    {
        OnMove?.Invoke();
    }

    public override void IdleAnimation()
    {
        Move();
        base.IdleAnimation();
    }

    //private void OnDrawGizmos()
    //{
    //    Debug.DrawRay(transform.position + _actorStats.OffSetToCenter, transform.forward, Color.red, _attackStats.AttackRadious);
    //}
}
