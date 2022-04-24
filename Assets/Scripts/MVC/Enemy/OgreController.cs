using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreController : EnemyBaseController
{
    private IState<EnemySates> _steeringState;
    private IArtificialMovement _ia;

    protected override void Awake()
    {
        base.Awake();
        _model = GetComponent<OgreModel>();
        if (_model is IArtificialMovement)
            _ia = _model as IArtificialMovement;
        else
            Debug.LogError("Model is missing IA movement interface");
    }

    protected override void InitializedTree()
    {
        INode dead = new ActionNode(() => _fsm.Transition(EnemySates.Dead));
        INode idle = new ActionNode(() => _fsm.Transition(EnemySates.Idle));
        INode attack = new ActionNode(() => _fsm.Transition(EnemySates.Attack));
        INode steering = new ActionNode(() => _fsm.Transition(EnemySates.Steering));
        INode patrol = new ActionNode(() => _fsm.Transition(EnemySates.Patrol));
        //INode getHit = new ActionNode(() => _fsm.Transition(states.GetHit)); //TODO: add get hit animation?

        Dictionary<INode, int> random = new Dictionary<INode, int>();
        random[idle] = _ia.IAStats.RandomnessIdle;
        random[patrol] = _ia.IAStats.RandomnessPatrol;
        INode randomAction = new RandomNode(random);

        //LOGIC: Is it dead? -> Can I See You? -> Are you in Attack Range? -> Can I Attack You?
        INode qCanAttack = new QuestionNode(CanAttack, attack, idle); //Si estas en rango, pero no te puedo atacar-> idle. 
        INode qIsInAttackRange = new QuestionNode((_model as IArtificialMovement).CheckIsInRange, qCanAttack, steering); //Si no esta en rango de ataque -> chase.
        INode qLineOfSight = new QuestionNode(CheckLineOfSight, qIsInAttackRange, randomAction); // Si no esta visible -> idle
        INode qIsDead = new QuestionNode(IsDead, dead, qLineOfSight); //Si no estas con vida -> muerto.

        _rootNode = qIsDead;;
    }

    protected override void InitializedFSM()
    {
        _fsm = new FSM<EnemySates>();

        _idleState = new IdleState<EnemySates>(_model as IArtificialMovement, (_model as IArtificialMovement).IAStats.TimeRoot, _rootNode);
        _attackState = new PhysicalAttackState<EnemySates>((_model as IAttack), _rootNode);
        _steeringState = new SteeringState<EnemySates>(_model as IArtificialMovement, _rootNode);
        _patrolState = new PatrolState<EnemySates>(_model as IPatrol, _rootNode, (_model as IPatrol).IAStats.CanReversePatrol);
        _deadState = new DeadState<EnemySates>(_model, (_model as IArtificialMovement).IAStats.TimeRoot);

        _attackState.AddTransition(EnemySates.Dead, _deadState);
        _attackState.AddTransition(EnemySates.Idle, _idleState);
        _attackState.AddTransition(EnemySates.Patrol, _patrolState);
        _attackState.AddTransition(EnemySates.Steering, _steeringState);

        _idleState.AddTransition(EnemySates.Attack, _attackState);
        _idleState.AddTransition(EnemySates.Dead, _deadState);
        _idleState.AddTransition(EnemySates.Steering, _steeringState);
        _idleState.AddTransition(EnemySates.Patrol, _patrolState);

        _steeringState.AddTransition(EnemySates.Attack, _attackState);
        _steeringState.AddTransition(EnemySates.Idle, _idleState);
        _steeringState.AddTransition(EnemySates.Dead, _deadState);
        _steeringState.AddTransition(EnemySates.Patrol, _patrolState);

        _patrolState.AddTransition(EnemySates.Attack, _attackState);
        _patrolState.AddTransition(EnemySates.Idle, _idleState);
        _patrolState.AddTransition(EnemySates.Dead, _deadState);
        _patrolState.AddTransition(EnemySates.Steering, _steeringState);

        _fsm.SetInit(_idleState);
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
