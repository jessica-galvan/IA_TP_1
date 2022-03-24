using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController instance;

    #region KeyCodes
    private string horizontalAxis = "Horizontal";
    private string verticalAxis = "Vertical";
    private KeyCode attack = KeyCode.Mouse0;
    private KeyCode defend = KeyCode.Mouse1;
    private KeyCode pause = KeyCode.Escape;
    //private KeyCode jump = KeyCode.Space;
    #endregion

    #region Events
    public Action OnPause;
    public Action OnAttack;
    public Action OnDefend;
    public Action OnJump;
    public Action<float, float> OnMove;
    #endregion

    #region Unity
    private void Awake()
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

    private void Update()
    {
        CheckPause();

        if (!GameManager.instance.IsGameFreeze)
        {
            CheckMovement();
            CheckAttack();
            CheckDefend();
            //CheckJump();
        }
    }
    #endregion

    #region Private
    private void CheckMovement()
    {
        float horizontal = Input.GetAxisRaw(horizontalAxis);
        float vertical = Input.GetAxisRaw(verticalAxis);
        OnMove?.Invoke(horizontal, vertical);
    }
    private void CheckAttack()
    {
        if (Input.GetKeyDown(attack))
            OnAttack?.Invoke();
    }
    private void CheckDefend()
    {
        if (Input.GetKeyDown(defend))
            OnDefend?.Invoke();
    }
    private void CheckPause()
    {
        if (Input.GetKeyDown(pause))
            OnPause?.Invoke();
    }
    //private void CheckJump()
    //{
    //    if (Input.GetKeyDown(jump))
    //        OnJump?.Invoke();
    //}
    #endregion
}
