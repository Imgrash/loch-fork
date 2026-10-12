using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public abstract class SignalChecker : MonoBehaviour
{
    public virtual bool Evaluate(Signal _signal, SignalGetterState _actGetterState, SignalActionType _signalActionType)
    {
        return true;
    }
    public bool Evaluate(SignalAction _signalAction, SignalGetterState _signalGetterState)
    {
        return Evaluate(_signalAction.SignalInAction, _signalGetterState, _signalAction.TypeOfAction);
    }
}
