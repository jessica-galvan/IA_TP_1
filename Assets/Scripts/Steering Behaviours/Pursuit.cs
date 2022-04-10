using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursuit : ISteering
{
    IArtificialMovement _entity;
    ITarget _target;

    public Pursuit(IArtificialMovement entity)
    {
        _entity = entity;
        SetTarget(_entity.Target);
    }

    public void SetTarget(ITarget target)
    {
        _target = target;
    }

    public Vector3 GetDir()
    {
        if (_target == null)
            return Vector3.zero;

        float distance = Vector3.Distance(_entity.transform.position, _target.transform.position) - 0.1f;
        //Movimiento Rectilineo Uniforme = Posicion Actual + Direccion * Velocidad * Tiempo
        //Se hizo un clamp para evitar que si el enemigo cambia el foward, no vaya en contra del objetivo. 
        Vector3 targetPoint = _target.transform.position + _target.GetFoward * Mathf.Clamp(_target.Velocity * _entity.IAStats.TimePrediction, -distance, distance);
        Vector3 dir = targetPoint - _entity.transform.position;
        return dir.normalized;
    }
}
