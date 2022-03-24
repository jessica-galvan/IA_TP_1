using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LifeController))]

public class ActorModel : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected ActorStats _actorStats;
    [SerializeField] protected AttackStats _attackStats;

    //PROPIEDADES
    public ActorStats ActorStats => _actorStats;
    public AttackStats AttackStats => _attackStats;
    public LifeController LifeController { get; private set; }

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
