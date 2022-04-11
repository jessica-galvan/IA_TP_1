using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PidgeonModel : EntityModel, IArtificialMovement
{
    //Variables
    [SerializeField] private IAStats _stats;
    [SerializeField] private PlayerModel _target;
    private ISteering _steering;
    private ISteering _avoidance;
    private Rigidbody _rb;
    private float timeTurn = 1f;

    //Properties
    public ISteering Avoidance => _avoidance;
    public ITarget Target => (PlayerModel)_target ;
    public IAStats IAStats => _stats;
    public ISteering Steering => _steering;

    public Action<bool> OnMove { get => _onMove; set => _onMove = value; }
    private Action<bool> _onMove = delegate { };

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody>();
        InitilizeSteering();
    }

    void InitilizeSteering()
    {
        var seek = new Seek(this);
        var flee = new Flee(this);
        var pursuit = new Pursuit(this);
        var escape = new Escape(this);
        _avoidance = new ObstacleAvoidance(this);
        SetNewSteering(flee);
    }

    public void Move(Vector3 dir)
    {
        dir.y = 0;
        _rb.velocity = dir * _actorStats.Speed;

        // OnMove?.Invoke(_rb.velocity.magnitude >= 0.1f);
    }

    public void LookDir(Vector3 dir)
    {
        if(dir != Vector3.zero)
        {
            dir.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, dir, timeTurn);
        }
    }

    public void SetNewSteering(ISteering newSteering)  
    {
        _steering = newSteering;
    }

    public bool CheckIsInRange()
    {
        float distance = (transform.position - Target.transform.position).sqrMagnitude;
        return distance <= IAStats.MaxDistanceFromTarget;
    }

    public void Update()
    {
        if (CheckIsInRange())
        {
            var dir = (_avoidance.GetDir() * IAStats.AvoidanceWeight + _steering.GetDir() * IAStats.SteeringWeight).normalized; //el avoidance puede ir adentro del state chase por ejemplo. o el seek, pursuit, flee, etc. 

            LookDir(dir);
            Move(dir);
        }
    }

    public bool CheckIsTooFar()
    {
        throw new NotImplementedException();
    }
}
