using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum states
    {
        Idle,
        Run,
        Attack,
        Dead, 
        Win
    }

    private PlayerModel __player;
    private FSM<states> _fsm;

    void Awake()
    {
        __player = GetComponent<PlayerModel>();
        InitializedFSM();
        SubscribeEvents();
    }

    private void SubscribeEvents() //El input se recibe en el controller
    {
        //InputController.instance.OnMove += OnMove;
        //InputController.instance.OnAttack += OnShoot;
        //InputController.instance.OnJump += OnJump;
        //InputController.instance.Defend += OnDefend;
    }

    private void InitializedFSM()
    {
        _fsm = new FSM<states>();

        //var dizzy = new DizzyState<states>(_batman, anim, dizzyTime, _fsm, states.Capoeira, states.Dead);
        //var capoeira = new CapoeiraState<states>(_batman, anim, attackTime, _fsm, states.Dizzy, states.win);
        //var dead = new DeadState<states>(_batman.gameObject, deadTime, anim);
        //var win = new WinState<states>(anim);

        //dizzy.AddTransition(states.Dead, dead);
        //dizzy.AddTransition(states.Capoeira, capoeira);

        //capoeira.AddTransition(states.Dizzy, dizzy);
        //capoeira.AddTransition(states.win, win);

        //_fsm.SetInit(capoeira);
    }

    void Update()
    {
        _fsm.OnUpdate();
    }
}
