using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChickenSates
{
    Idle,
    Patrol,
    Fleeing
}
public class ChickenController : EntityController
{
    private FSM<ChickenSates> _fsm;
    private IState<ChickenSates> _idleState;
    private IState<ChickenSates> _deadState;
    private IState<ChickenSates> _patrolState;
    private IState<ChickenSates> _steeringState;
    private INode _rootNode;
    private ChickenModel _chickenModel;

    protected override void Awake()
    {
        base.Awake();
        _chickenModel = GetComponent<ChickenModel>();
    }

    protected override void Start()
    {
        base.Start();
        InitializedTree();
        InitializedFSM();
    }

    private void Update()
    {
        _fsm.OnUpdate();
    }

    private void InitializedTree()
    {
        INode idle = new ActionNode(() => _fsm.Transition(ChickenSates.Idle));
        INode steering = new ActionNode(() => _fsm.Transition(ChickenSates.Fleeing));
        INode patrol = new ActionNode(() => _fsm.Transition(ChickenSates.Patrol));

        Dictionary<INode, int> random = new Dictionary<INode, int>();
        random[idle] = _chickenModel.IAStats.RandomnessIdle;
        random[patrol] = _chickenModel.IAStats.RandomnessPatrol;
        INode randomAction = new RandomNode(random);

        //LOGIC: Can I See You? -> Run for your life, elses patrol or idle. //Chicken cannot die
        INode qLineOfSight = new QuestionNode(CheckLineOfSight, steering, randomAction);

        _rootNode = qLineOfSight;
    }

    private void InitializedFSM()
    {
        _fsm = new FSM<ChickenSates>();

        _idleState = new ChickenIdleState<ChickenSates>(_chickenModel, _fsm, ChickenSates.Fleeing, _chickenModel.IAStats.TimeIdle, _rootNode);
        _steeringState = new ChickenFleeingState<ChickenSates>(_chickenModel, _rootNode);
        _patrolState = new ChickenPatrolState<ChickenSates>(_chickenModel, _fsm, ChickenSates.Fleeing, _rootNode, _chickenModel.IAStats.CanReversePatrol);

        _idleState.AddTransition(ChickenSates.Fleeing, _steeringState);
        _idleState.AddTransition(ChickenSates.Patrol, _patrolState);

        _steeringState.AddTransition(ChickenSates.Idle, _idleState);
        _steeringState.AddTransition(ChickenSates.Patrol, _patrolState);

        _patrolState.AddTransition(ChickenSates.Idle, _idleState);
        _patrolState.AddTransition(ChickenSates.Fleeing, _steeringState);

        _fsm.SetInit(_idleState);
    }

    protected bool CheckLineOfSight()
    {
        bool answer =  false;

        if(_chickenModel.Target != null)
            answer = _chickenModel.LineOfSight(_chickenModel.Target.transform);

        return answer;
    }
}
