using System.Threading.Tasks; //Esto es para hacer una funcion asincronica. 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemySates
{
    Idle,
    Patrol,
    GetHit,
    Steering,
    Attack,
    Dead,
}

public class EnemyBaseController : EntityController
{
    protected EnemyBaseView _view;
    protected FSM<EnemySates> _fsm;
    protected IState<EnemySates> _attackState;
    protected IState<EnemySates> _idleState;
    protected IState<EnemySates> _deadState;
    protected IState<EnemySates> _patrolState;
    protected INode _rootNode;

    #region Private
    protected override void Awake()
    {
        _model = GetComponent<EnemyBaseModel>();
    }

    protected override void Start() //Suscribir al LifeController.TakeDamage a una funcion que llame a la transicion de get hit con fms. 
    {
        base.Start();
        InitializedTree();
        InitializedFSM();
    }

    protected virtual void InitializedTree()
    {
        INode dead = new ActionNode(()=> _fsm.Transition(EnemySates.Dead));
        //INode getHit = new ActionNode(() => _fsm.Transition(states.GetHit));
        INode idle = new ActionNode(() => _fsm.Transition(EnemySates.Idle));
        INode attack = new ActionNode(() => _fsm.Transition(EnemySates.Attack));

        INode qCanAttack = new QuestionNode(() => (_model as IAttack).CanAttack, attack, idle);
        INode qIsInAttackRange = new QuestionNode(CheckIsInAttackRange,qCanAttack, idle); //Si no esta en rango de ataque
        INode qLineOfSight = new QuestionNode(CheckLineOfSight, qIsInAttackRange, idle); 
        INode qIsDead = new QuestionNode(()=>_model.LifeController.IsDead,dead, qLineOfSight);

        _rootNode = qIsDead;
    }

    protected bool CheckLineOfSight()
    {
        bool answer = false;
        if (_model is IAttack) 
        {      
            Transform[] targets = (_model as EnemyBaseModel).CheckTargetsInRadious();

            for (int i = targets.Length - 1; i >= 0; i--)
            {
                if(_model is ILineOfSight)
                    answer = (_model as ILineOfSight).LineOfSight(targets[i]);
            }
        }
        return answer;
    }

    protected bool CheckIsInAttackRange()
    {
        return (_model as IArtificialMovement).CheckIsInRange();  //TODO: mejorar. Por ahora es un raycast hacia adelante para chequear 
    }

    protected virtual bool CanAttack()
    {
        print("Can Attack?: " + (_model as IAttack).CanAttack);
        return (_model as IAttack).CanAttack;
    }

    protected virtual void InitializedFSM()
    {
        _fsm = new FSM<EnemySates>();

        _attackState = new PhysicalAttackState<EnemySates>((_model as IAttack), _rootNode);
        _idleState = new IdleState<EnemySates>(_model as IArtificialMovement, (_model as IArtificialMovement).IAStats.TimeRoot, _rootNode);
        _deadState = new DeadState<EnemySates>(_model, (_model as IArtificialMovement).IAStats.TimeRoot);

        _attackState.AddTransition(EnemySates.Dead, _deadState);
        _attackState.AddTransition(EnemySates.Idle, _idleState);

        _idleState.AddTransition(EnemySates.Attack, _attackState);
        _idleState.AddTransition(EnemySates.Dead, _deadState);

        _fsm.SetInit(_idleState);
    }

    protected virtual void Update()
    {
        _fsm.OnUpdate();
    }
    #endregion
}
