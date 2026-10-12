using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPrepare
{
    SignalAction PrepareSignal(SignalAction signalAction, SignalGetterState state);
}
