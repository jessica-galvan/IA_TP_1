using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : ISteering
{
    ITarget _target;
    IArtificialMovement _entity;

    public Flee(IArtificialMovement entity)
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

        Vector3 dir = _entity.transform.position - _target.transform.position;
        return dir.normalized;
    }
}
