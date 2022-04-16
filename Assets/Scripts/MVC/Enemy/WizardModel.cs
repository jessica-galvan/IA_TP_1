using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardModel : EnemyBaseModel, IPatrol
{
    //Variables
    [SerializeField] private IAStats _stats;
    private ISteering _steering;
    private ISteering _avoidance;
    private float timeTurn = 1f;

    //Properties
    public ISteering Avoidance => _avoidance;
    public IAStats IAStats => _stats;
    public ISteering Steering => _steering;

    public GameObject[] PatrolRoute { get; private set; }

    public Action<bool> OnMove { get => _onMove; set => _onMove = value; }
    private Action<bool> _onMove = delegate { };

    protected override void Awake()
    {
        base.Awake();
        var patrol = GetComponentInChildren<PatrolRoute>();
        patrol.Initialize();
        PatrolRoute = patrol.PatrolNodes;
    }

    protected override void Start()
    {
        base.Start();

    }

    public void InitilizeSteering()
    {
        var seek = new Seek(this);
        //var flee = new Flee(this);
        //var pursuit = new Pursuit(this);
        _avoidance = new ObstacleAvoidance(this);
        
        SetNewSteering(seek);
    }

    protected override void SetEnemyList(ITarget playerTarget)
    {
        base.SetEnemyList(playerTarget);
        InitilizeSteering();
    }

    public void Move(Vector3 dir)
    {
        dir.y = 0;
        _rb.velocity = dir * _actorStats.Speed;

        OnMove?.Invoke(_rb.velocity.magnitude >= 0.1f);
    }

    public void LookDir(Vector3 dir)
    {
        dir.y = 0;
        base.transform.forward = Vector3.Lerp(base.transform.forward, dir, timeTurn);
    }

    public void SetNewSteering(ISteering newSteering) //Patron Strategy: utiliar interfaces  
    {
        _steering = newSteering;
    }

    public bool CheckIsInRange() //Lets check when to we are too close or too far away
    {
        float distance = (transform.position - Target.transform.position).sqrMagnitude;
        return distance <= AttackStats.AttackRadious;
    }

    public bool CheckIsTooFar()
    {
        float distance = Vector3.Distance(transform.position, Target.transform.position);
        return distance < IAStats.MaxDistanceFromTarget;
    }

    public override void IdleAnimation()
    {
        base.IdleAnimation();
        _rb.velocity = Vector3.zero;

    }
}

    