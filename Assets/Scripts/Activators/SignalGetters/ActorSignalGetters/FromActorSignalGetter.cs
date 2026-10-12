using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class FromActorSignalGetter : BaseSignalGetter
{
    public bool isStopActor = false;
    protected ActorGetterParent parent;
    public ActorGetterParent Parent
    {
        get => parent;
        set => parent = value;
    }
    public override void AddSignalToQueue(object emitter, bool afterEmitting)
    {
        BaseActor actor = (((BaseActor, SignalActionType))emitter).Item1;
        if (actor.IsOneTime)
        { 
            if(actor.HasAlreadyGivenSignal)
                return;
            else
                actor.HasAlreadyGivenSignal = true;
        }
        signalsQueue.Add(emitter);
        if (!isProcessingSignals)
            StartCoroutine(ProcessSignals(afterEmitting));
    }

    public override void ProcessFirstSignalInQueue(object signalTuple, bool afterEmitting)
    {
        (BaseActor, SignalActionType) signalTupleAsTuple = ((BaseActor, SignalActionType))signalTuple;
        BaseActor signalGiver = signalTupleAsTuple.Item1;
        if (signalGiver != null)
        {
            //signalsQueue.Add(signalTupleAsTuple);

            UniqueObject uoGiver = null;
            GlobalUniqueObjectsManager.getInstance().TryGet(signalGiver.Id, out uoGiver);
            UniqueObject uoTaker = null;
            GlobalUniqueObjectsManager.getInstance().TryGet(this.Id, out uoTaker);
            Debug.Log($"гивер: {uoGiver.name} тейкер: {uoTaker.name}");

            SignalActionType actionType = signalTupleAsTuple.Item2;
            SignalAction signalToRead = null;
            signalGiver.GiveSignalSafe(out signalToRead, actionType, this.Id, afterEmitting);
            if (signalToRead != null && signalToRead.SignalInAction != null)
            {
                SignalAction saNew = null;
                if (!reactor.Process(signalToRead, ref saNew) && !skipProcessIfNotPossible)
                    return;
                if (saNew != null)
                    Emitt(saNew, afterEmitting);
            }
            StopGettingSignalAction(signalTuple);
        }
        else if (signalsQueue.Contains(signalTuple))
            signalsQueue.Remove(signalTuple);

    }
    public override void StopGettingSignalAction(object emitter)
    {
        if (signalsQueue.Contains(emitter))
            signalsQueue.Remove(emitter);
        if (isStopActor)
        {
            BaseActor signalGiver = (((BaseActor signalGiver, SignalActionType actionType)) emitter).signalGiver;
            if (!signalGiver.IsStopActor)
                signalGiver.StopActor();
        }
    }
}
