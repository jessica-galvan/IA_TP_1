using System.Threading.Tasks; //Esto es para hacer una funcion asincronica. 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseController : EntityController
{
    protected enum states
    {
        Idle,
        Patrol, //Patrol is walking around an area, no chasing or any other steering behaviour
        GetHit,
        Steering,
        Attack,
        Dead,
    }

    [SerializeField] protected float timeTree = 1f;

    protected EnemyBaseView _view;
    protected FSM<states> _fsm;
    protected IState<states> _attackState;
    protected IState<states> _idleState;
    protected IState<states> _deadState;
    protected IState<states> _patrolState;
    protected INode _rootNode;
    protected Transform _target;

    protected virtual void Awake()
    {
        _model = GetComponent<EnemyBaseModel>();
        InitializedTree();
        InitializedFSM();
    }

    protected virtual void InitializedTree()
    {
        INode dead = new ActionNode(()=> _fsm.Transition(states.Dead));
        INode getHit = new ActionNode(() => _fsm.Transition(states.GetHit));
        INode idle = new ActionNode(() => _fsm.Transition(states.Idle));
        INode attack = new ActionNode(() => _fsm.Transition(states.Attack));
        INode steering = new ActionNode(() => _fsm.Transition(states.Steering));
        INode patrol = new ActionNode(() => _fsm.Transition(states.Patrol)); 

        Dictionary<INode, int> random = new Dictionary<INode, int>();
        random[idle] = 50;
        random[patrol] = 50;

        INode randomAction = new RandomNode(random);
        INode qCanAttack = new QuestionNode(() => (_model as IAttack).CanAttack, attack, idle);
        INode qIsInAttackRange = new QuestionNode(CheckIsInAttackRange,qCanAttack, randomAction); //Si no esta en rango de ataque
        INode qLineOfSight = new QuestionNode(CheckLineOfSight, qIsInAttackRange, randomAction);
        INode qIsDead = new QuestionNode(()=>_model.LifeController.IsDead,dead, qLineOfSight);

        _rootNode = qIsDead;
    }

    protected bool CheckLineOfSight()
    {
        if(_model is IAttack) 
        {      
            Transform[] targets = (_model as EnemyBaseModel).CheckTargetsInRadious();
            for (int i = targets.Length - 1; i >= 0; i--)
            {
                if(_model is ILineOfSight)
                {
                    (_model as ILineOfSight).LineOfSight(targets[i]);
                    return true;
                }
            }
        }
        return false;
    }

    protected bool CheckIsInAttackRange()
    {
        if(_model is IAttack)
            return (_model as EnemyBaseModel).CheckTargetInFront();  //TODO: mejorar. Por ahora es un raycast hacia adelante para chequear 
        return false;
    }

    protected virtual void InitializedFSM()
    {
        _fsm = new FSM<states>();

        _attackState = new AttackState<states>((_model as IAttack), timeTree,_rootNode);
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
    }

    protected void Update()
    {
        _fsm.OnUpdate();
    }

    //async void WaitForDeadEnemies() //Esto es para hacer una funcion asincronica. Podria ir en el bailecito de los enemigos luego de que destruyan a Crash. Esto es porque el destroy se hace en un frame mas tarde
    //{
    //    await Task.Delay(100);
    //    _rootNode.Execute();
    //}

    protected bool CheckDistance()
    {
        //TODO: check if player is in range of attack.
        //state of pursit llama a esta funcion, si da true, que llame de nuevo al arbol!
        return false;
    }
}
