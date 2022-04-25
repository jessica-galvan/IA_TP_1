using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button buttonPlay;
    [SerializeField] private Button buttonQuit;

    public Button PlayButton => buttonPlay;
    public Button QuitButton => buttonQuit;

    public Action OnPlay;
    public Action OnQuit;

    void Awake()
    {
        buttonPlay.onClick.AddListener(OnClickPlayHandler);
        buttonQuit.onClick.AddListener(OnClickQuitHandler);
    }

    void Start()
    {
        if (GameManager.instance.FSMActive)
            GameManager.instance.SetMenuController(this);
    }

    private void OnClickPlayHandler()
    {
        if (GameManager.instance.FSMActive)
            OnPlay?.Invoke();
        else
            SceneManager.LoadScene(GameManager.instance.LevelScene);
    }

    private void OnClickQuitHandler()
    {
        if (GameManager.instance.FSMActive)
            OnQuit?.Invoke();
        else
        {
            Application.Quit();
            print("Cerramos el juego");
        }
    }
}
