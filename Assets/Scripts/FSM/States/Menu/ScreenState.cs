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

        if (SceneManager.GetActiveScene().name != _sceneName) //Si no estabamos en al escena... cargala
        {
            SceneManager.LoadScene(_sceneName);
            GameManager.instance.SetCursorActive(true);
        }

        if (GameManager.instance.ScreenController == null)
            GameManager.instance.OnSetScreenController += OnSetScreenController;
        else
            SuscribeEvents();
    }

    private void OnSetScreenController()
    {
        GameManager.instance.OnSetScreenController -= OnSetScreenController;
        SuscribeEvents();
    }

    private void SuscribeEvents()
    {
        GameManager.instance.ScreenController.OnLevel += OnClickLevel;
        GameManager.instance.ScreenController.OnMenu += OnClickMenu;
    }

    private void OnClickMenu()
    {
        _fsm.Transition(_inputMenu);
    }

    private void OnClickLevel()
    {
        _fsm.Transition(_inputLevel);
    }

    public override void Exit()
    {
        if (GameManager.instance.ScreenController != null)
        {
            GameManager.instance.ScreenController.OnLevel -= OnClickLevel;
            GameManager.instance.ScreenController.OnMenu -= OnClickMenu;
        }
    }
}
