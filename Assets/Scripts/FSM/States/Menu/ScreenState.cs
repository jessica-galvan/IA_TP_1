using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenState<T> : State<T>
{
    private string _sceneName;
    private FSM<T> _fsm;
    private T _inputMenu;
    private T _inputLevel;

    public ScreenState(string sceneName, FSM<T> fsm, T inputMenu, T inputLevel)
    {
        _sceneName = sceneName;
        _fsm = fsm;
    }

    public override void Init()
    {
        SceneManager.LoadScene(_sceneName);
        //suscribe to button events. 
        //check if there is ONE or TWO buttons (cuz game over is going to have two!
    }

    public override void Execute()
    {
        //check for button events?
    }

    public override void Exit()
    {
        //desuscribe to button events. 
    }

    private void OnClickMenu()
    {
        _fsm.Transition(_inputMenu);
    }

    private void OnClickLevel()
    {
        _fsm.Transition(_inputLevel);
    }
}
