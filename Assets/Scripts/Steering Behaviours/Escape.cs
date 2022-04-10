using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape : ISteering
{
    IArtificialMovement _entity;
    ITarget _target;

    public Escape(IArtificialMovement  entity)
    {
        _entity = entity;
        SetTarget(_entity.Target);
    }

    public void SetTarget(ITarget newTarget)
    {
        _target = newTarget;
    }

    public Vector3 GetDir()
    {
        if (_target == null)
            return Vector3.zero;

        float distance = Vector3.Distance(_entity.transform.position, _target.transform.position) - 0.1f;
        Vector3 targetPoint = _target.transform.position + _target.GetFoward * Mathf.Clamp(_target.Velocity * _entity.IAStats.TimePrediction, -distance, distance);
        //A:targetPoint
        //B:entity
        //B-A
        Vector3 dir = _entity.transform.position - targetPoint;
        return dir.normalized;
    }
}
