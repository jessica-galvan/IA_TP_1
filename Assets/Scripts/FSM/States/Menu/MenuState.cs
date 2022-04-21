using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuState<T> : State<T>
{
    string _sceneName;
    FSM<T> _fsm;
    T _inputLevel;

    public MenuState(string sceneName, FSM<T> fsm, T inputLevel)
    {
        _sceneName = sceneName;
        _fsm = fsm;
        _inputLevel = inputLevel;
    }

    public override void Init()
    {
        if(GameManager.instance.CurrentScene != _sceneName)
        {
            SceneManager.LoadScene(_sceneName);
            GameManager.instance.SetCursorActive(true);
        }
    }

    public override void Execute()
    {
        //TODO: logica botones??? o algun script que le pase los inputs de los OnClickEvent?
    }

    private void OnClickLevel()
    {
        _fsm.Transition(_inputLevel);
    }
}
