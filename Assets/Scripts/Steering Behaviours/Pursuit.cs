using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursuit : ISteering
{
    Transform _entity;
    Transform _target;
    IVelocity _targetVelocity;
    float _predictionTime;

    public Pursuit(Transform entity, Transform target, IVelocity targetVelocity, float _predictionTime)
    {
        _entity = entity;
        SetTarget(target, targetVelocity);
    }

    public void SetTarget(Transform newTarget, IVelocity newTargetVelocity)
    {
        _target = newTarget;
    }

    public Vector3 GetDir()
    {
        if (_target != null || _targetVelocity != null)
            return Vector3.zero;

        float distance = Vector3.Distance(_entity.position, _target.position) - 0.1f;
        //Movimiento Rectilineo Uniforme = Posicion Actual + Direccion * Velocidad * Tiempo
        //Se hizo un clamp para evitar que si el enemigo cambia el foward, no vaya en contra del objetivo. 
        Vector3 targetPoint = _target.position + _targetVelocity.GetFoward * Mathf.Clamp(_targetVelocity.GetVel * _predictionTime, -distance, distance);
        Vector3 dir = targetPoint - _entity.position;
        return dir.normalized;
    }
}
