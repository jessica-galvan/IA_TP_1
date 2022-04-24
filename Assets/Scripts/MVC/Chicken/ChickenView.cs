using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenView : MonoBehaviour
{
    protected Animator _animator;
    private ChickenModel _model;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _model = GetComponent<ChickenModel>();
    }
    private void Start()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _model.OnFleeing += RunAnimation;
        _model.OnMove += WalkAnimation;
        _model.OnEat += EatAnimation;
        _model.OnTurnHead += TurnHeadAnimation;
    }


    private void Idle()
    {
        _animator?.Play("Idle");
    }

    private void RunAnimation(bool value)
    {
        _animator?.SetBool("Run", value);
    }

    private void WalkAnimation(bool value)
    {
        _animator?.SetBool("Walk", value);
    }

    private void TurnHeadAnimation()
    {
        _animator?.SetTrigger("TurnHead");
    }

    private void EatAnimation()
    {
        _animator?.SetTrigger("Eat");
    }

    private void OnDestroy()
    {
        _model.OnFleeing -= RunAnimation;
        _model.OnMove -= WalkAnimation;
        _model.OnEat -= EatAnimation;
        _model.OnTurnHead -= TurnHeadAnimation;
    }
}
