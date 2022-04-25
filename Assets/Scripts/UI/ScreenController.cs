using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button buttonLevel;
    [SerializeField] private Button buttonMenu;

    public Action OnLevel;
    public Action OnMenu;

    void Awake()
    {
        buttonLevel.onClick.AddListener(OnClickLevelHandler);
        buttonMenu.onClick.AddListener(OnClickMenuHandler);
    }

    void Start()
    {
        if (GameManager.instance.FSMActive)
            GameManager.instance.SetScreenController(this);
    }

    private void OnClickLevelHandler()
    {
        if (GameManager.instance.FSMActive)
            OnLevel?.Invoke();
        else
            SceneManager.LoadScene(GameManager.instance.LevelScene);
    }

    private void OnClickMenuHandler()
    {
        if (GameManager.instance.FSMActive)
            OnMenu?.Invoke();
        else
            SceneManager.LoadScene(GameManager.instance.MenuScene);
    }
}
