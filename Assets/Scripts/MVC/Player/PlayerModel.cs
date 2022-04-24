using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerModel : EntityModel, IAttack, ITarget
{
    [SerializeField] protected AttackStats _attackStats;

    //VARIABLES
    protected float cooldownTimer;
    protected Rigidbody _rb;

    //PROPIERTIES
    public Rigidbody Rb { get => _rb; }
    public bool CanAttack { get; private set ; }
    public float Velocity => _rb.velocity.magnitude;
    public Vector3 GetFoward => transform.forward;
    public AttackStats AttackStats => _attackStats;

    //EVENTS
    private Action _onAttack = delegate { };
    private Action _onMove = delegate { };

    public Action OnAttack { get => _onAttack; set => _onAttack = value; }
    public Action OnMove { get => _onMove; set => _onMove = value; }

    protected override void Awake()
    {
        base.Awake();
        CanAttack = true;
        _rb = GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        base.Start();
        GameManager.instance.SetPlayer(this);
    }

    private IEnumerator AttackTimer(float time)
    {
        yield return new WaitForSeconds(time);
        CanAttack = true;
    }

    protected override void OnDeath()
    {
        GameManager.instance.GameOver();
    }

    #region Public
    public void Attack()
    {
        if (CanAttack)
        {
            CanAttack = false;
            OnAttack?.Invoke();
            StartCoroutine(AttackTimer(_attackStats.Cooldown));
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
    #endregion

    //private void OnDrawGizmos()
    //{
    //    Debug.DrawRay(transform.position + _actorStats.OffSetToCenter, transform.forward, Color.red, _attackStats.AttackRadious);
    //}
}
