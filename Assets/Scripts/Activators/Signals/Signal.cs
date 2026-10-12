using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum SignalType
{
    LinkedEmitter = 0,
    Switch
}

public enum SignalActionType
{
    Adding = 0,
    Extracting
}
public class SignalAction
{
    public SignalAction(Signal _SignalInAction, SignalActionType _TypeOfAction)
    {
        SignalInAction = _SignalInAction;
        TypeOfAction = _TypeOfAction;
    }
    public Signal SignalInAction;
    public SignalActionType TypeOfAction;

}

public abstract class SignalGetterState : UniqueScriptable, ICloneable
{
    public SignalGetterState()
    {
    }
    public object Clone()
    {
        return MemberwiseClone();
    }
}

[CreateAssetMenu(fileName = "Signal", menuName = "ScriptableObject/Signal")]
public class Signal : UniqueScriptable, ICloneable
{
    [SerializeField]
    protected SignalType typeOfSignal;
    public SignalType TypeOfSignal => typeOfSignal;

    public SignalContent Content;
    public void GiveSignal(out Signal signalOut)
    {
        signalOut = this;
    }
    public object Clone()
    {
        return MemberwiseClone();
    }
    public static Signal Instance(Signal signal, SignalContent signalContent)
    {
        var signalType = signal.GetType();
        Signal signalNew = (Signal)ScriptableObject.CreateInstance(signalType);
        signalNew.Content = (SignalContent)signalContent.Clone();
        return signalNew;
    }
}
