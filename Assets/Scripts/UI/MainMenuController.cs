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


    void Awake()
    {
        buttonPlay.onClick.AddListener(OnClickPlayHandler);
        buttonQuit.onClick.AddListener(OnClickQuitHandler);
    }

    private void OnClickPlayHandler()
    {
        SceneManager.LoadScene(GameManager.instance.LevelScene);
    }

    private void OnClickQuitHandler()
    {
        Application.Quit();
        print("Cerramos el juego");
    }
}
