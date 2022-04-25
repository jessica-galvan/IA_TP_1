using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum GameStates
{
    Menu,
    Level,
    Victory,
    GameOver
}

enum InGameStatus
{
    Playing,
    Pause
}

public class GameManager : MonoBehaviour
{
    //SINGLETON
    public static GameManager instance;

    [Header("Scene Names")]
    [SerializeField] private string levelScene = "Level";
    [SerializeField] private string menuScene = "MainMenu";
    [SerializeField] private string victoryScene = "Victory";
    [SerializeField] private string gameOverScene = "GameOver";
    [SerializeField] private GameStates startingScene;

    [Header("Scene Managers")]
    [SerializeField] private MainMenuController menuController;
    [SerializeField] private ScreenController screenController;

    [Header("Level")]
    [SerializeField] private Transform enemyNodesParent;

    //FMS
    private FSM<GameStates> _fsm;
    private IState<GameStates> _menuState;
    private IState<GameStates> _levelState;
    private IState<GameStates> _victoryState;
    private IState<GameStates> _gameOverState;

    //PROPIEDADES
    public bool FSMActive;
    public bool IsGameFreeze { get; private set; }
    public ITarget Player { get; private set; }
    public Transform PatrolNodeParent => enemyNodesParent;
    public string MenuScene => menuScene;
    public string LevelScene => levelScene;
    public MainMenuController MenuController => menuController;
    public ScreenController ScreenController => screenController;

    //EVENTS
    public Action OnPause;
    public Action<ITarget> OnPlayerInit;
    public Action OnSetScreenController;
    public Action OnSetMenuController;
    public Action OnEnemyManagerInit;
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

    public void Start()
    {
        if(FSMActive)
            InitializeFMS();
    }

    private void InitializeFMS()
    {
        _fsm = new FSM<GameStates>();
        _levelState = new LevelState<GameStates>(levelScene, _fsm, GameStates.Menu, GameStates.GameOver, GameStates.Victory);
        _menuState = new MenuState<GameStates>(menuScene, _fsm, GameStates.Level);
        _victoryState = new ScreenState<GameStates>(victoryScene, _fsm, GameStates.Menu, GameStates.Level);
        _gameOverState = new ScreenState<GameStates>(gameOverScene, _fsm, GameStates.Menu, GameStates.Level);

        _levelState.AddTransition(GameStates.Menu, _menuState);
        _levelState.AddTransition(GameStates.GameOver, _gameOverState);
        _levelState.AddTransition(GameStates.Victory, _victoryState);
        _gameOverState.AddTransition(GameStates.Level, _levelState);
        _gameOverState.AddTransition(GameStates.Menu, _menuState);
        _victoryState.AddTransition(GameStates.Menu, _menuState);
        _menuState.AddTransition(GameStates.Level, _levelState);

        switch (startingScene)
        {
            case GameStates.Menu:
                _fsm.SetInit(_menuState);
                break;
            case GameStates.Level:
                _fsm.SetInit(_levelState);
                break;
            case GameStates.Victory:
                _fsm.SetInit(_victoryState);
                break;
            case GameStates.GameOver:
                _fsm.SetInit(_gameOverState);
                break;
            default:
                _fsm.SetInit(_menuState);
                break;
        }
    }

    private void Update()
    {
        if (FSMActive)
            _fsm.OnUpdate();
    }

    public void Pause(bool value)
    {
        IsGameFreeze = value;
        SetCursorActive(value);
        if (value)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
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

    public void SetScreenController(ScreenController screenController)
    {
        this.screenController = screenController;
        OnSetScreenController?.Invoke();
    }

    public void SetMenuController(MainMenuController menuController)
    {
        this.menuController = menuController;
        OnSetMenuController?.Invoke();
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
