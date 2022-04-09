using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PidgeonController : EntityController
{
    public Transform _target;
    public LayerMask obsMask;

    private ISteering _steering;
    private ISteering _avoidance;
    private float avoidanceWeight = 1f;
    private float steeringWeight = 1f;

    private void Awake()
    {
        _model = GetComponent<PidgeonModel>();
        InitilizeSteering();
    }

    public void SetNewSteering(ISteering newSteering) //Patron Strategy: utiliar interfaces  
    {
        _steering = newSteering;
    }

    void InitilizeSteering()
    {
        var seek = new Seek(transform, _target);
        var flee = new Flee(transform, _target);
        var pursuit = new Pursuit(transform, _target, _target.GetComponent<IVelocity>(), 5f);
        if(_model is IArtificial)
        {
            _avoidance = new ObstacleAvoidance(_model as IArtificial); //TODO: Check if this is a good idea
        }
        _steering = seek; //this is supposed to be really in a chaseState
    }

    private void Update()
    {
        var dir = (_avoidance.GetDir() * avoidanceWeight  + steeringWeight * _steering.GetDir()).normalized; //el avoidance puede ir adentro del state chase por ejemplo. o el seek, pursuit, flee, etc. 
        if (_model is IMovement)
        {
            (_model as IMovement).LookDir(dir);
            (_model as IMovement).Move(dir);
        }
    }

}
