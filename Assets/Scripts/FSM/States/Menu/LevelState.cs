using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelState<T> : State<T>
{
    FSM<T> _fsm;

    string _sceneName;
    private T _inputMenu;
    private T _inputGameOver;
    private T _inputVictory;

    public  LevelState(string sceneName, FSM<T> fsm, T inputMenu, T inputGameOver, T inputVictory)
    {
        _sceneName = sceneName;
        _fsm = fsm;
        _inputMenu = inputMenu;
        _inputGameOver = inputGameOver;
        _inputVictory = inputVictory;
    }

    public override void Init()
    {
        if(GameManager.instance.CurrentScene != _sceneName)
        {
            SceneManager.LoadScene(_sceneName);
            GameManager.instance.OnVictory += OnVictory;
            GameManager.instance.OnGameOver += OnGameOver;
        }
    }

    public override void Execute()
    {
        //TODO: logica que corra el input controller update? no idea. 
        //PODRIIIA tener un listado de game objets y correr manualmente el update de todos los gameobjects necesarios en la escena pero... no nos compliquemos.
    }

    public override void Exit()
    {
        GameManager.instance.OnVictory -= OnVictory;
        GameManager.instance.OnGameOver -= OnGameOver;
    }

    private void OnVictory()
    {
        _fsm.Transition(_inputVictory);
    }

    private void OnGameOver()
    {
        _fsm.Transition(_inputGameOver);
    }

    private void OnClickMenu() //Mover al Pause Menu
    {
        _fsm.Transition(_inputMenu);
    }

}
