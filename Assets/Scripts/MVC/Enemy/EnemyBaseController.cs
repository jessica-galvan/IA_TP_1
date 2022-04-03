using System.Threading.Tasks; //Esto es para hacer una funcion asincronica. 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseController : EntityController
{
    protected enum states
    {
        Idle,
        Patrol,
        Attack,
        Dead,
    }

    [SerializeField] protected float time = 5f;

    protected EnemyBaseModel _model;
    protected EnemyBaseView _view;
    protected FSM<states> _fsm;
    protected IState<states> _attackState;
    protected IState<states> _idleState;
    protected IState<states> _deadState;
    protected IState<states> _patrolState;
    protected INode _rootNode;

    private void Awake()
    {
        _model = GetComponent<EnemyBaseModel>();
        InitializedTree();
        InitializedFSM();
    }

    void InitializedTree()
    {
        INode dead = new ActionNode(()=> _fsm.Transition(states.Dead));
        INode idle = new ActionNode(() => _fsm.Transition(states.Idle));
        INode patrol = new ActionNode(() => _fsm.Transition(states.Patrol));
        INode attack = new ActionNode(() => _fsm.Transition(states.Attack));

        Dictionary<INode, int> random = new Dictionary<INode, int>();
        random[idle] = 50;
        random[patrol] = 50;

        INode randomAction = new RandomNode(random);
        INode qCanAttack = new QuestionNode(() => _model.CanAttack, attack, idle);
        INode qIsInAttackRange = new QuestionNode(CheckIsInAttackRange,qCanAttack, randomAction); //Si no esta en rango de ataque
        INode qLineOfSight = new QuestionNode(CheckLineOfSight, qIsInAttackRange, randomAction);
        INode qIsDead = new QuestionNode(()=>_model.LifeController.IsDead,dead, qLineOfSight);

        _rootNode = qIsDead;
    }

    bool IsAttacking() //Para chequear si esta en el estado que corresponde, en caso de necesitar un qNode
    {
        return _fsm.GetCurrentState == _attackState;
    }

    bool CheckLineOfSight()
    {
        Transform[] targets = _model.CheckTargetsInRadious();
        for (int i = targets.Length - 1; i >= 0; i--)
        {
            _model.LineOfSight(targets[i]);
            return true;
        }
        return false;
    }

    bool CheckIsInAttackRange()
    {
        return _model.CheckTargetInFront();  //TODO: mejorar. Por ahora es un raycast hacia adelante para chequear 
    }

    void InitializedFSM()
    {
        _fsm = new FSM<states>();

        _attackState = new AttackState<states>(_model, time,_rootNode);
        _idleState = new IdleState<states>(_model, time, _rootNode);
        _patrolState = new PatrolState<states>(_model, time, _rootNode);
        _deadState = new DeadState<states>(_model, time);

        _attackState.AddTransition(states.Dead, _deadState);
        _attackState.AddTransition(states.Patrol, _patrolState);
        _attackState.AddTransition(states.Idle, _idleState);

        _idleState.AddTransition(states.Patrol, _patrolState);
        _idleState.AddTransition(states.Attack, _attackState);
        _idleState.AddTransition(states.Dead, _deadState);

        _patrolState.AddTransition(states.Attack, _attackState);
        _patrolState.AddTransition(states.Idle, _idleState);

        _fsm.SetInit(_idleState);
    }

    void Update()
    {
        _fsm.OnUpdate();
    }

    //async void WaitForDeadEnemies() //Esto es para hacer una funcion asincronica. Podria ir en el bailecito de los enemigos luego de que destruyan a Crash. Esto es porque el destroy se hace en un frame mas tarde
    //{
    //    await Task.Delay(100);
    //    _rootNode.Execute();
    //}
}
