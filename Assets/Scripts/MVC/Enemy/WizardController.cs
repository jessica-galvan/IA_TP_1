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
        INode idle = new ActionNode(() => _fsm.Transition(states.Idle));
        INode attack = new ActionNode(() => _fsm.Transition(states.Attack));
        INode steering = new ActionNode(() => _fsm.Transition(states.Steering));
        INode patrol = new ActionNode(() => _fsm.Transition(states.Patrol));
        //INode getHit = new ActionNode(() => _fsm.Transition(states.GetHit)); //TODO: add get hit animation?

        Dictionary<INode, int> random = new Dictionary<INode, int>();
        random[idle] = 1;
        random[patrol] = 99;
        INode randomAction = new RandomNode(random);

        //LOGIC: Is it dead? -> Can I See You? -> Are you in Attack Range? -> Can I Attack You?
        INode qCanAttack = new QuestionNode(CanAttack, attack, randomAction); //Si estas en rango, pero no te puedo atacar-> idle. 
        INode qIsInAttackRange = new QuestionNode((_model as IArtificialMovement).CheckIsInRange, qCanAttack, steering); //Si no esta en rango de ataque -> chase.
        INode qLineOfSight = new QuestionNode(CheckLineOfSight, qIsInAttackRange, randomAction); // Si no esta visible -> idle
        INode qIsDead = new QuestionNode(() => _model.LifeController.IsDead, dead, qLineOfSight); //Si no estas con vida -> muerto.

        _rootNode = qIsDead;;
    }

    protected override void InitializedFSM()
    {
        base.InitializedFSM();

        //Steering
        _steeringState = new SteeringState<states>(_model as IArtificialMovement, _rootNode);
        _attackState.AddTransition(states.Steering, _steeringState);
        _idleState.AddTransition(states.Steering, _steeringState);
        _steeringState.AddTransition(states.Attack, _attackState);
        _steeringState.AddTransition(states.Idle, _idleState);
        _steeringState.AddTransition(states.Dead, _deadState);

        //Patrol 
        _patrolState = new PatrolState<states>(_model as IPatrol, _rootNode);
        _attackState.AddTransition(states.Patrol, _patrolState);
        _idleState.AddTransition(states.Patrol, _patrolState);
        _steeringState.AddTransition(states.Patrol, _patrolState);
        _patrolState.AddTransition(states.Attack, _attackState);
        _patrolState.AddTransition(states.Idle, _idleState);
    }

    protected override bool CanAttack()
    {
        return (_model as IAttack).CanAttack;
    }

    protected override void Update()
    {
        _fsm.OnUpdate();
    }
}
