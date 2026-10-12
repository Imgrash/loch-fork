using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseSignalGetter : UniqueObject
{
    //непосредственно обработчик сигнала
    [SerializeField]
    [RequireInterface(typeof(IReactor))]
    private UnityEngine.Object Reactor;

    public bool skipProcessIfNotPossible = true;
    public IReactor reactor => Reactor as IReactor;
    //очередь сигналов, поступивших на обработку (по-хорошему тут всегда должен быть
    //всего один сигнал, но мало ли)
    [SerializeField]
    public List<object> signalsQueue = new List<object>();
    public bool QueueIsNotEmpty() => signalsQueue.Any();
    //добавляет сигнал в очередь и, если ещё не в состоянии обработки сигналов,
    //заставляет войти в это состояние и начать обрабатывать сигналы
    public virtual void AddSignalToQueue(object emitter, bool afterEmitting)
    {
        signalsQueue.Add(emitter);
        if(!isProcessingSignals)
            StartCoroutine(ProcessSignals(afterEmitting));
    }
    protected bool isProcessingSignals = false;
    public void AllProcessed() =>
        isProcessingSignals = false;
    //корутина обработки пришедших в очередь сигналов
    //обрабатывает все пришедшие в очередь сигналы, пока очередь не опустеет
    public IEnumerator ProcessSignals(bool afterEmitting)
    {
        if (!isProcessingSignals)
            isProcessingSignals = true;
        while (QueueIsNotEmpty())
        {
            ProcessFirstSignalInQueue(signalsQueue[0], afterEmitting);
            yield return null;
        }
        AllProcessed();
    }
    public abstract void ProcessFirstSignalInQueue(object signalTuple, bool afterEmitting);
    public virtual void StopGettingSignalAction(object emitter)
    {
        if (signalsQueue.Contains(emitter))
            signalsQueue.Remove(emitter);
    }

    public List<FromEmitterSignalGetter> linkedEmitterSignalGettersOut = new List<FromEmitterSignalGetter>();
    public void Emitt(SignalAction signalAction, bool afterEmitting = false)
    {
        foreach (var activator in linkedEmitterSignalGettersOut)
            activator.AddSignalToQueue((this.Id, signalAction), afterEmitting);
    }


    public List<FromEmitterSignalGetter> afterEmittedActGetters = new List<FromEmitterSignalGetter>();
    public void LinkActorEmitterActGetter(FromEmitterSignalGetter actorEmittedActGetter) =>
        EmittFurther += actorEmittedActGetter.AddSignalToQueue;

    public event Action<object, bool> EmittFurther;
    private void Start()
    {
        foreach (var emittGetter in afterEmittedActGetters)
            LinkActorEmitterActGetter(emittGetter);
    }
}
