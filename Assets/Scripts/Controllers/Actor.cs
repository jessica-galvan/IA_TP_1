using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LifeController))]
[RequireComponent(typeof(Animator))]

public class Actor : MonoBehaviour, IDamagable
{
    [Header("Stats")]
    [SerializeField] protected ActorStats _actorStats;
    [SerializeField] protected AttackStats _attackStats;
    protected Animator _animator;

    //PROPIEDADES
    public LifeController LifeController { get; private set; }
    public AttackStats AttackStats => _attackStats;

    //EVENTS
    public Action OnDie;

    protected virtual void Awake()
    {
        LifeController = GetComponent<LifeController>();
        _animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        InitStats();
    }

    protected void InitStats()
    {
        LifeController.SetStats(_actorStats);
        LifeController.OnTakeDamage += OnTakeDamage;
        LifeController.OnDie += OnDeath;
    }

    protected virtual IDamagable[] CheckEnemyIsInRange()
    {
        //Raycasting? Overlap? Depends on the type of attack
        return null;
    }

    protected virtual void OnAttack()
    {
        //???
    }

    protected virtual void OnTakeDamage()
    {
        //_animatorController.SetTrigger("TakeDamage");
    }

    protected virtual void OnDeath()
    {
        //_animatorController.SetTrigger("IsDead");
    }

    protected virtual void DeathAnimationOver()
    {
        OnDie?.Invoke();
    }
}
