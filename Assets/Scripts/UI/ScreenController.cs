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


    void Awake()
    {
        buttonLevel.onClick.AddListener(OnClickLevelHandler);
        buttonMenu.onClick.AddListener(OnClickMenuHandler);
    }

    private void OnClickLevelHandler()
    {
        SceneManager.LoadScene(GameManager.instance.LevelScene);
    }

    private void OnClickMenuHandler()
    {
        SceneManager.LoadScene(GameManager.instance.MenuScene);
    }
}
