using System;
using System.Collections.Generic;
using UnityEngine;
public class FromEmitterSignalGetter : BaseSignalGetter
{
    //public new List<(Guid signalGiverId, SignalAction signalAction)> signalsQueue = new();
    public override void ProcessFirstSignalInQueue(object signalTuple, bool afterEmitting = false)
    {
        (Guid, SignalAction) signalTupleAsTuple = ((Guid, SignalAction))signalTuple;
        SignalAction signalAction = signalTupleAsTuple.Item2;

        if (signalAction != null && signalAction.SignalInAction != null)
        {
            SignalAction saNew = null;
            if (!reactor.Process(signalAction, ref saNew) && !skipProcessIfNotPossible)
                return;
            if(saNew != null)
                Emitt(saNew);
        }
        StopGettingSignalAction(signalTupleAsTuple);
    }
}
