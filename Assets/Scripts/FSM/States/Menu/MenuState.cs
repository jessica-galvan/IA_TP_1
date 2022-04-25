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
        if (SceneManager.GetActiveScene().name != _sceneName) //Si no estabamos en al escena... cargala
        {
            SceneManager.LoadScene(_sceneName);
            GameManager.instance.SetCursorActive(true);
        }

        if (GameManager.instance.MenuController == null)
            GameManager.instance.OnSetMenuController += OnSetMenuController;
        else
            SuscribeEvents();
    }

    private void OnSetMenuController()
    {
        GameManager.instance.OnSetMenuController -= OnSetMenuController;
        SuscribeEvents();
    }

    private void SuscribeEvents()
    {
        GameManager.instance.MenuController.OnPlay += OnPlay;
        GameManager.instance.MenuController.OnQuit += OnQuit;
    }

    private void OnPlay()
    {
        _fsm.Transition(_inputLevel);
    }

    private void OnQuit()
    {
        Debug.Log("Cerramos el juego");
        Application.Quit();
    }

    public override void Exit()
    {
        GameManager.instance.MenuController.OnPlay -= OnPlay;
        GameManager.instance.MenuController.OnQuit -= OnQuit;
    }
}
