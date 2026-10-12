using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReactor
{
    public abstract bool Process(SignalAction signalAction, ref SignalAction signalActionNew);
}
