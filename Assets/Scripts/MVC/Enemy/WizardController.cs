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
        INode getHit = new ActionNode(() => _fsm.Transition(states.GetHit));
        INode idle = new ActionNode(() => _fsm.Transition(states.Idle));
        INode attack = new ActionNode(() => _fsm.Transition(states.Attack));
        INode steering = new ActionNode(() => _fsm.Transition(states.Steering));
        INode patrol = new ActionNode(() => _fsm.Transition(states.Patrol));


        INode qCanAttack = new QuestionNode(() => (_model as IAttack).CanAttack, attack, idle);
        INode qIsInAttackRange = new QuestionNode(CheckIsInAttackRange, qCanAttack, steering); //Si no esta en rango de ataque
        INode qLineOfSight = new QuestionNode(CheckLineOfSight, qIsInAttackRange, idle);
        //INode qIsHit = new QuestionNode(LifeController); //TODO: add get hit to tree behaviour?
        INode qIsDead = new QuestionNode(() => _model.LifeController.IsDead, dead, qLineOfSight);

        _rootNode = qIsDead;
    }

    protected override void InitializedFSM()
    {
        _fsm = new FSM<states>();

        _attackState = new AttackState<states>((_model as IAttack), timeTree, _rootNode);
        _idleState = new IdleState<states>(_model, timeTree, _rootNode);
        _patrolState = new PatrolState<states>(_model, timeTree, _rootNode);
        _deadState = new DeadState<states>(_model, timeTree);

        _attackState.AddTransition(states.Dead, _deadState);
        _attackState.AddTransition(states.Patrol, _patrolState);
        _attackState.AddTransition(states.Idle, _idleState);

        _idleState.AddTransition(states.Patrol, _patrolState);
        _idleState.AddTransition(states.Attack, _attackState);
        _idleState.AddTransition(states.Dead, _deadState);

        _patrolState.AddTransition(states.Attack, _attackState);
        _patrolState.AddTransition(states.Idle, _idleState);

        _fsm.SetInit(_idleState);

        base.InitializedFSM();

        _steeringState = new SteeringState<states>(_model as IArtificialMovement, _rootNode);
        _steeringState.AddTransition(states.Attack, _attackState);
        _steeringState.AddTransition(states.Idle, _idleState);
        _steeringState.AddTransition(states.Dead, _deadState);
    }
}
