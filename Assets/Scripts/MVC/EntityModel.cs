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

    protected void InitStats()
    {
        LifeController.SetStats(_actorStats);
    }
}
