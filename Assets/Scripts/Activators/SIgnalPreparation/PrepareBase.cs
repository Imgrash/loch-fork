using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PrepareBase : UniqueScriptable, IPrepare, ICloneable
{
    public SignalAction PrepareSignal(SignalAction signalAction, SignalGetterState state)
    {
        Signal signal = Signal.Instance(signalAction.SignalInAction, signalAction.SignalInAction.Content);
        SignalAction signalActionNew = new SignalAction(signal, signalAction.TypeOfAction);
        return signalActionNew;
    }
    public object Clone()
    {
        return MemberwiseClone();
    }
}
