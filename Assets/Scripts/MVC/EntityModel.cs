using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LifeController))]
public abstract class EntityModel : MonoBehaviour, IDamagable, IModel
{
    [Header("Stats")]
    [SerializeField] protected ActorStats _actorStats;
    [SerializeField] protected AttackStats _attackStats;

    //PROPIEDADES
    public ActorStats ActorStats => _actorStats;
    public AttackStats AttackStats => _attackStats;
    public LifeController LifeController { get; private set; }

    public Action OnIdle { get => _onIdle; set => _onIdle = value; }
    private Action _onIdle = delegate { };

    public Action OnHit{ get => _onHit; set => _onHit = value; }
    private Action _onHit = delegate { };

    public Action OnDie { get => _onDie; set => _onDie = value; }
    private Action _onDie = delegate { };

    protected virtual void Awake()
    {
        LifeController = GetComponent<LifeController>();
    }

    protected virtual void Start()
    {
        InitStats();
    }

    protected virtual void InitStats()
    {
        LifeController.SetStats(_actorStats);
    }

    public virtual void IdleAnimation()
    {
        OnIdle?.Invoke();
    }

    public virtual void GetHitAnimation()
    {
        OnHit?.Invoke();
    }

}
