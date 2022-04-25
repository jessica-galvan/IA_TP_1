using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ChickenModel : EntityModel, IPatrol, ILineOfSight
{
    //Variables
    [SerializeField] private IAStats _stats;
    [SerializeField] private PlayerModel _target;
    [SerializeField] private LayerMask _targetList;
    [SerializeField] private float fleeingSpeed;
    [SerializeField] private bool DrawGizmos;
    private ISteering _steering;
    private ISteering _avoidance;
    private Rigidbody _rb;

    //Properties
    public ISteering Avoidance => _avoidance;
    public ITarget Target => (PlayerModel)_target;
    public IAStats IAStats => _stats;
    public ISteering Steering => _steering;
    public GameObject[] PatrolRoute { get; private set; }

    //Events
    public Action<bool> OnMove { get => _onMove; set => _onMove = value; }
    public Action<bool> OnFleeing = delegate{};
    public Action OnTurnHead = delegate {};
    public Action OnEat = delegate {};
    private Action<bool> _onMove = delegate { };

    #region Private
    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody>();

    }

    protected override void Start()
    {
        base.Start();
        GameManager.instance.OnPlayerInit += SetEnemyList;
        var patrol = GetComponentInChildren<PatrolRoute>();
        patrol.Initialize();
        PatrolRoute = patrol.PatrolNodes;
    }

    public void InitilizeSteering()
    {
        var flee = new Flee(this);
        //var escape = new Escape(this);
        _avoidance = new ObstacleAvoidance(this);
        SetNewSteering(flee);
    }

    private void SetEnemyList(ITarget playerTarget)
    {
        _target = playerTarget as PlayerModel;
        InitilizeSteering();
    }


    private void OnDrawGizmos()
    {
        if (DrawGizmos)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _actorStats.RangeVision);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position + _actorStats.OffSetToCenter, Quaternion.Euler(0, _actorStats.AngleVision / 2, 0) * -transform.forward * _actorStats.RangeVision);
            Gizmos.DrawRay(transform.position + _actorStats.OffSetToCenter, Quaternion.Euler(0, -_actorStats.AngleVision / 2, 0) * -transform.forward * _actorStats.RangeVision);
        }
    }
    #endregion

    #region Public
    public void Move(Vector3 dir)
    {
        dir.y = 0;
        
        _rb.velocity = dir * _actorStats.Speed;

        OnMove?.Invoke(_rb.velocity.magnitude >= 0.1f);
    }

    public void Fleeing(Vector3 dir)
    {
        dir.y = 0;

        _rb.velocity = dir * fleeingSpeed;

        OnMove?.Invoke(_rb.velocity.magnitude >= 0.1f);
    }

    public void LookDir(Vector3 dir)
    {
        if(dir != Vector3.zero)
        {
            dir.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, dir, IAStats.TimeTurn);
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

    public bool CheckIsStillTooNear()
    {
        float distance = Vector3.Distance(transform.position, Target.transform.position);
        return distance < IAStats.MaxDistanceFromTarget;
    }

    public bool LineOfSight(Transform target) //Check if Enemy Target is Visible (in angle, range and no obstacles).
    {
        //RANGE 
        Vector3 diff = target.position - transform.position;
        float distance = diff.magnitude;
        if (distance > _actorStats.RangeVision)
            return false;

        //ANGLE
        float angleToTarget = Vector3.Angle(diff, transform.forward);
        if (angleToTarget > _actorStats.AngleVision / 2)
            return false;

        //TARGET VIEW
        if (Physics.Raycast(transform.position, diff.normalized, distance, IAStats.ObstacleList))
            return false;

        return true;
    }

    public Transform[] CheckTargetsInRadious() //Checks and Returns a List of Targets that are in the AOE vision radious
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, _actorStats.RangeVision, _targetList);
        Transform[] targets = new Transform[colls.Length];

        for (int i = 0; i < colls.Length; i++)
        {
            targets[i] = colls[i].transform;
        }
        return targets;
    }
    #endregion
}
