using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalContentTypeChecker : SignalChecker
{
    [SerializeField]
    protected SignalContentType ContentTypeToCheck = new();
    public override bool Evaluate(Signal _signal, SignalGetterState _actGetterState, SignalActionType _signalActionType)
    {
        return _signal.Content.TypeOfContent == ContentTypeToCheck;
    }
}
