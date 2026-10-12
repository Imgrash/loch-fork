using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalTypeChecker : SignalChecker
{
    [SerializeField]
    protected SignalType TypeToCheck = new();
    public override bool Evaluate(Signal _signal, SignalGetterState _actGetterState, SignalActionType _signalActionType)
    {
        return _signal.TypeOfSignal == TypeToCheck;
    }
}
