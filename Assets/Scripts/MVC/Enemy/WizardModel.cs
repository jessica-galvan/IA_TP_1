using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardModel : EnemyBaseModel, IPatrol, IAttackMagic
{
    //Variables
    [SerializeField] private IAStats _stats;
    [SerializeField] private BulletStats _bulletStats;
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private PlayerModel player;
    private ISteering _steering;
    private ISteering _avoidance;
    private float timeTurn = 1f;

    //Properties
    public BulletStats BulletStats => _bulletStats;
    public ISteering Avoidance => _avoidance;
    public IAStats IAStats => _stats;
    public ISteering Steering => _steering;
    public GameObject[] PatrolRoute { get; private set; }


    //Events
    public Action<bool> OnMove { get => _onMove; set => _onMove = value; }
    private Action<bool> _onMove = delegate { };

    #region Private
    protected override void Awake()
    {
        base.Awake();
        var patrol = GetComponentInChildren<PatrolRoute>();
        patrol.Initialize();
        PatrolRoute = patrol.PatrolNodes;
    }

    protected void InitilizeSteering()
    {
        var seek = new Seek(this);
        _avoidance = new ObstacleAvoidance(this);
        SetNewSteering(seek);
    }

    protected override void SetEnemyList(ITarget playerTarget)
    {
        base.SetEnemyList(playerTarget);
        player = playerTarget as PlayerModel;
        InitilizeSteering();
    }

    #endregion

    #region Public
    public void Move(Vector3 dir)
    {
        dir.y = 0;
        _rb.velocity = dir * _actorStats.Speed;

        OnMove?.Invoke(_rb.velocity.magnitude >= 0.1f);
    }

    public void LookDir(Vector3 dir)
    {
        dir.y = 0;
        //transform.LookAt(dir);
        transform.forward = Vector3.Lerp(transform.position, dir, timeTurn);
    }

    public void SetNewSteering(ISteering newSteering) //Patron Strategy: utiliar interfaces  
    {
        _steering = newSteering;
    }

    public bool CheckIsInRange() //Lets check when to we are too close or too far away
    {
        float distance = (transform.position - Target.transform.position).sqrMagnitude;
        //print(distance + " Radious: " + AttackStats.AttackRadious);
        return distance <= AttackStats.AttackRadious;
    }

    public bool CheckIsTooFar()
    {
        float distance = Vector3.Distance(transform.position, Target.transform.position);
        if (IAStats.MaxDistanceFromTarget < ActorStats.RangeVision) //Ehhh para que no vuelva a pasar, si la persecucion maxima es menor a la vision.. usan vision range. 
        {
            print("OJO!! stat de distancia maxima de persecución es menor a la de vision!");
            return distance < ActorStats.RangeVision;
        }
        else
            return distance < IAStats.MaxDistanceFromTarget;
    }

    public override void IdleAnimation()
    {
        base.IdleAnimation();  
        OnMove?.Invoke(false);
        _rb.velocity = Vector3.zero;
    }

    public void ShootBullet()
    {
        var bullet = Instantiate(_bulletStats.Prefab, _shootingPoint.position, transform.rotation);
        bullet.SetStats(AttackStats, BulletStats);
        bullet.Initialize();
    }
    #endregion
}

