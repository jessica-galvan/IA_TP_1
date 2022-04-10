using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardModel : EnemyBaseModel, IArtificialMovement
{
    //Variables
    [SerializeField] private IAStats _stats;
    [SerializeField] private ITarget _target;
    private ISteering _steering;
    private ISteering _avoidance;
    private float timeTurn = 1f;

    //Properties
    public ISteering Avoidance => _avoidance;
    public ITarget Target => _target;
    public IAStats IAStats => _stats;
    public ISteering Steering => _steering;

    public Action<bool> OnMove { get => _onMove; set => _onMove = value; }
    private Action<bool> _onMove = delegate { };

    protected override void Awake()
    {
        base.Awake();
        InitilizeSteering();
    }

    void InitilizeSteering()
    {
        var seek = new Seek(this);
        var flee = new Flee(this);
        var pursuit = new Pursuit(this);
        _avoidance = new ObstacleAvoidance(this);
        
        SetNewSteering(seek);
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

    public bool CheckTargetDistance()
    {
        //Lets check when to change to call for a _root
        float distance = (transform.position - Target.transform.position).sqrMagnitude;
        print(distance);
        return (distance < IAStats.MaxDistanceFromTarget || distance >= AttackStats.AttackRadious)? true : false;
    }

}

