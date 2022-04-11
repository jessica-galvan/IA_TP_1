using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : EnemyBaseController
{
    private IState<states> _steeringState;

    protected override void Awake()
    {
        base.Awake();
        _model = GetComponent<WizardModel>();
    }

    protected override void InitializedTree()
    {
        INode dead = new ActionNode(() => _fsm.Transition(states.Dead));
        //INode getHit = new ActionNode(() => _fsm.Transition(states.GetHit)); //TODO: add get hit animation?
        INode idle = new ActionNode(() => _fsm.Transition(states.Idle));
        INode attack = new ActionNode(() => _fsm.Transition(states.Attack));
        INode steering = new ActionNode(() => _fsm.Transition(states.Steering));

        //LOGIC: Is it dead? -> Can I See You? -> Are you in Attack Range? -> Can I Attack You?
        INode qCanAttack = new QuestionNode(CanAttack, attack, idle); //Si estas en rango, pero no te puedo atacar-> idle. 
        INode qIsInAttackRange = new QuestionNode((_model as IArtificialMovement).CheckIsInRange, qCanAttack, steering); //Si no esta en rango de ataque -> chase.
        INode qLineOfSight = new QuestionNode(CheckLineOfSight, qIsInAttackRange, idle); // Si no esta visible -> idle
        INode qIsDead = new QuestionNode(() => _model.LifeController.IsDead, dead, qLineOfSight); //Si no estas con vida -> muerto.

        _rootNode = qIsDead;;
    }

    protected override void InitializedFSM()
    {
        base.InitializedFSM();

        _steeringState = new SteeringState<states>(_model as IArtificialMovement, _rootNode);

        _attackState.AddTransition(states.Steering, _steeringState);
        _idleState.AddTransition(states.Steering, _steeringState);
        _steeringState.AddTransition(states.Attack, _attackState);
        _steeringState.AddTransition(states.Idle, _idleState);
        _steeringState.AddTransition(states.Dead, _deadState);
    }

    protected override bool CanAttack()
    {
        return (_model as IAttack).CanAttack;
    }

    //protected override void Update()
    //{
    //    if (CheckLineOfSight())
    //    {
    //        if ((_model as IArtificialMovement).CheckTargetDistance())
    //        {
    //            var dir = ((_model as IArtificialMovement).Avoidance.GetDir() * (_model as IArtificialMovement).IAStats.AvoidanceWeight + (_model as IArtificialMovement).Steering.GetDir() * (_model as IArtificialMovement).IAStats.SteeringWeight).normalized; //el avoidance puede ir adentro del state chase por ejemplo. o el seek, pursuit, flee, etc. 

    //            (_model as IArtificialMovement).LookDir(dir);
    //            (_model as IArtificialMovement).Move(dir);
    //        }
    //    }

    //}
}
