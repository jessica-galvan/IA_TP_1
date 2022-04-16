using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState<T> : State<T>
{
    private IPatrol _model;
    private INode _root;
    private bool _canRevert;
    private int currentPosition = 0;
    private bool isDoingReverse = false;

    public PatrolState(IPatrol model, INode root, bool canRevert = false)
    {
        _model = model;
        _root = root;
        _canRevert = canRevert;
    }

    public override void Init()
    {
        //Debug.Log("Patrol Lenght " + _model.PatrolRoute.Length);
        //TODO: Decide if it should start from beginning to patrol or save last position and go back to that one.
    }

    public override void Execute()
    {
        if (!_model.LineOfSight(_model.Target.transform))
        {
            Movement();
        }
        else
        {
            _root.Execute();
        }

    }

    private void Movement()
    {
        Vector3 currentTarget = _model.PatrolRoute[currentPosition].transform.position;
        Vector3 direction = (currentTarget - _model.transform.position).normalized;
        //Vector3 direction = (_model.Avoidance.GetDir() * _model.IAStats.AvoidanceWeight + dir * _model.IAStats.SteeringWeight).normalized;
        _model.Move(direction);
        _model.LookDir(direction);

        var distance = Vector3.Distance(_model.transform.position, currentTarget);
        //Debug.Log("DISTANCIA " + distance);
        if (distance <= 1f)
        {
            ChangeCurrentPosition();
            _root.Execute();
        }
    }

    private void ChangeCurrentPosition()
    {
        if (!isDoingReverse) //Si no hace reverse, esto va a dar siempre falso!
        {
            if (currentPosition < _model.PatrolRoute.Length - 1) //Si es menor al total, sumale
            {
                currentPosition++;
                //Debug.Log("cambio position a: " + currentPosition + " de " + _model.PatrolRoute.Length);
            }
            else
            {
                if (_canRevert) //Si ya llego al final... Fijate si puede revertir
                {
                    isDoingReverse = true; //Activa la vuelva atras
                    currentPosition--; //Y resta una posicion
                }
                else
                {
                    currentPosition = 0; //Sino mandalo a la posicion inicial
                }
            }
        }
        else //Si esta haciendo el reverse
        {
            if (currentPosition > 0) //Y todavia no llego a 0
            {
                currentPosition--; //Segui restando
            }
            else
            {
                isDoingReverse = false; //Si llego a cero, sacalo de aca
                currentPosition++; //y ya sumale una para que valla caminando
            }
        }
    }
}
