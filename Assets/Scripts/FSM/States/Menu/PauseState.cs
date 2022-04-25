using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseState<T> : State<T>
{
    private Button resumeButton;
    private Button restartButton;
    private Button mainMenuButton;
    private Button quitButton;

    public override void Init()
    {
        //resumeButton?.onClick.AddListener(OnResumeHandler);
        //restartButton?.onClick.AddListener(OnRestartHandler);
        //mainMenuButton?.onClick.AddListener(OnMenuHandler);
        //quitButton?.onClick.AddListener(OnQuitHandler);
        //IsGameFreeze true
        //timescale again
        //suscribe to buttons
        //hide hud
        //activate pause menu canvas
        GameManager.instance.SetCursorActive(true);
    }

    public override void Execute()
    {
        //check for ESC input to close again, then call function to close. 
    }

    public override void Exit()
    {
        //Undo IsGameFreeze and time scale. 
        //Visible HUD again?
        //unscribe to buttons
        GameManager.instance.SetCursorActive(false);
    }

    private void OnMenu()
    {
        
    }
}
