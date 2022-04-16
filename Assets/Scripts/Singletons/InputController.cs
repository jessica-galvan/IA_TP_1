using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController instance;
    public bool IsMoving { get; private set; }

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
    public Action<Vector3> OnMove;
    #endregion

    #region Unity
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        CheckPause();
    }

    public void PlayerUpdate()
    {
        if (!GameManager.instance.IsGameFreeze)
        {
            CheckAttack();
            CheckMovement();
            //CheckDefend();
            //CheckJump();
        }
    }
    #endregion

    #region Private
    private void CheckMovement() //Moverlo para controllar en idle (chequeo si no me muevo) y en move (si me muevo) del player. 
    {
        float horizontal = Input.GetAxisRaw(horizontalAxis);
        float vertical = Input.GetAxisRaw(verticalAxis);
        IsMoving = (vertical != 0|| horizontal != 0) ? true : false;
        OnMove?.Invoke(new Vector3(horizontal, 0, vertical));
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
