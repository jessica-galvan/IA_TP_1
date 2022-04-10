using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : ISteering
{
    ITarget _target;
    IArtificialMovement _entity;

    public Seek(IArtificialMovement entity)
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

        Vector3 dir = _target.transform.position - _entity.transform.position;
        return dir.normalized;
    }
}
