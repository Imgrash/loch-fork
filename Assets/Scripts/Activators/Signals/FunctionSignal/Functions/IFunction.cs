using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFunction
{
    void Process(GameObject gameObject, SignalGetterState State, SignalActionType actionType);
}
