using System.Threading.Tasks; //Esto es para hacer una funcion asincronica. 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseController : EntityController
{
    protected enum states
    {
        Idle,
        Run,
        Attack,
        Dead,
    }

    [SerializeField] protected float time = 5f;

    protected EnemyBaseModel _model;
    protected EnemyBaseView _view;
    protected FSM<states> _fsm;
    protected IState<states> _attackState;
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
        INode run = new ActionNode(() => _fsm.Transition(states.Run));
        INode attack = new ActionNode(() => _fsm.Transition(states.Attack));

        //INode randomAction = new RandomNode(); //TODO: patrol or idle?
        //INode qIsInAttackRange = new QuestionNode(_model.IsDetectedTargets,); //Si no esta en rango de ataque
        //INode qLineOfSight = new QuestionNode(_model.LineOfSight, qIsInAttackRange, idle); 
        //INode qIsDead = new QuestionNode(); //TODO: check if there is HP...

        //_rootNode = qIsDead;
    }

    bool IsAttacking() //Para chequear si esta en el estado que corresponde, en caso de necesitar un qNode
    {
        return _fsm.GetCurrentState == _attackState;
    }

    void InitializedFSM()
    {
        _fsm = new FSM<states>();

        _attackState = new AttackState<states>(_model, time,_rootNode);
        var idle = new IdleState<states>(_model, time, _rootNode);
        var run = new RunState<states>(_model, time, _rootNode);
        var dead = new DeadState<states>(_model, time);

        _attackState.AddTransition(states.Dead, dead);
        _attackState.AddTransition(states.Run, run);

        idle.AddTransition(states.Run, run);
        idle.AddTransition(states.Dead, dead);

        run.AddTransition(states.Attack, _attackState);

        _fsm.SetInit(idle);
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
