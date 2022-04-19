using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//enum GameStates
//{
//    Menu,
//    Level,
//    Victory,
//    GameOver
//}

//enum InGameStatus
//{
//    Playing,
//    Pause
//}

public class GameManager : MonoBehaviour
{
    //SINGLETON
    public static GameManager instance;

    [SerializeField] private string levelScene = "SampleScene";
    [SerializeField] private string menuScene = "MainMenu";
    [SerializeField] private string victoryScene = "Victory";
    [SerializeField] private string gameOverScene = "GameOver";

    //FMS
    //private FSM<GameStates> _fsm;
    //private IState<GameStates> _menuState;
    //private IState<GameStates> _levelState;
    //private IState<GameStates> _victoryState;
    //private IState<GameStates> _gameOverState;

    //PROPIEDADES
    public bool IsGameFreeze { get; private set; }
    public ITarget Player { get; private set; }
    public string MenuScene => menuScene;
    public string LevelScene => levelScene;

    //EVENTS
    public Action OnPause;
    public Action<ITarget> OnPlayerInit;
    public Action OnVictory;
    public Action OnGameOver;

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    //private void InitializeFMS()
    //{
    //    _fsm = new FSM<GameStates>();
    //    _levelState = new LevelState<GameStates>(levelScene, _fsm, GameStates.Menu, GameStates.GameOver, GameStates.Victory);
    //    _menuState = new MenuState<GameStates>(menuScene, _fsm, GameStates.Level);
    //    _victoryState = new ScreenState<GameStates>(victoryScene, _fsm, GameStates.Menu, GameStates.Level);
    //    _gameOverState = new ScreenState<GameStates>(gameOverScene, _fsm, GameStates.Menu, GameStates.Level);

    //    _levelState.AddTransition(GameStates.Menu, _menuState);
    //    _levelState.AddTransition(GameStates.GameOver, _gameOverState);
    //    _levelState.AddTransition(GameStates.Victory, _victoryState);
    //    _gameOverState.AddTransition(GameStates.Level, _menuState);
    //    _gameOverState.AddTransition(GameStates.Menu, _menuState);
    //    _victoryState.AddTransition(GameStates.Menu, _menuState);
    //    _menuState.AddTransition(GameStates.Level, _levelState);

    //    _fsm.SetInit(_menuState);
    //}


    public void Pause(bool value)
    {
        IsGameFreeze = value;
        SetCursorActive(value);
        if (value)
        {
            Time.timeScale = 0;
            //TODO: lower music
        }
        else
        {
            Time.timeScale = 1;
            //TODO: subir musica
        }         
    }

    public void SetCursorActive(bool value)
    {
        if (value)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetPlayer(PlayerModel player)
    {
        Player = player;
        OnPlayerInit?.Invoke(Player);
    }

    public void Victory()
    {
        print("victory");
        SceneManager.LoadScene(victoryScene);
        //OnVictory?.Invoke();
    }

    public void GameOver()
    {
        SceneManager.LoadScene(gameOverScene);
        //OnGameOver?.Invoke();
    }
}
