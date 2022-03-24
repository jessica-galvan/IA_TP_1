using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T>
{
    void AddTransition(T input, IState<T> state);

    void RemoveTransition(T input);

    void RemoveTransition(IState<T> state);

    IState<T> GetTransition(T input);

    void Init();

    void Execute();

    void Exit();
}
