using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class BaseActor : UniqueObject
{
    public bool IsOneTime = false;
    public bool HasAlreadyGivenSignal = false;
    //Родитель - объект, который отвечает за способ передачи сигнала
    protected ActorParent parent;
    public ActorParent Parent
    {
        get => parent;
        set => parent = value;
    }
    //объекты, которые принимают сигнал от актора на текущий момент
    protected List<Guid> temporarilyLinkedActGetters = new List<Guid>();
    //
    public bool linkedToAnyActGetters => temporarilyLinkedActGetters.Count > 0;
    public bool EmittingToActGetter(Guid actorId) => temporarilyLinkedActGetters.Contains(actorId);
    //начало передачи сигнала конкретному приёмнику - отмечаем, что приёмник добавлен в список приёмников для текущего актора
    public void StartEmitting(Guid actGetterId)
    {
        if (!EmittingToActGetter(actGetterId))
            temporarilyLinkedActGetters.Add(actGetterId);
    }

    private Coroutine WaitForSignalIsProcessedLastCoroutine = null;
    //конец передачи сигнала конкретному приёмнику - убираем приёмник из списка приёмников текущего актора
    public void StopEmitting(Guid actGetterId, SignalAction action, bool afterEmitting)
    {
        if (EmittingToActGetter(actGetterId))
            WaitForSignalIsProcessedLastCoroutine = StartCoroutine(WaitForSignalIsProcessed(actGetterId, action, afterEmitting));
    }
    public bool IsStopActor = false;
    public void StopActor()
    {
        IsStopActor = true;
        stopEverything();
    }
    private void stopEverything()
    {
        HasAlreadyGivenSignal = false;
        temporarilyLinkedActGetters.Clear();
        IsStopActor = false;
        if(WaitForSignalIsProcessedLastCoroutine != null)
            StopCoroutine(WaitForSignalIsProcessedLastCoroutine);
        parent.SetActiveChild(false);
    }
    public IEnumerator WaitForSignalIsProcessed(Guid actGetterId, SignalAction action, bool afterEmitting)
    {
        UniqueObject uo = null;
        GlobalUniqueObjectsManager.getInstance().TryGet(actGetterId, out uo);
        if(uo is FromActorSignalGetter sg)
            while (sg.signalsQueue.Select(x => ((BaseActor signalGiver, SignalActionType actionType))x).Select(x => x.signalGiver.Id).Any(i => this.Id == i)
                && !IsStopActor)
                yield return null;

        temporarilyLinkedActGetters.Remove(actGetterId);

        if (!afterEmitting && !IsStopActor)
        {
            Signal signalAfter = Signal.Instance(signalAfterEmitting, signalAfterEmitting.Content);
            this.signalAfterEmitting.GiveSignal(out signalAfter);
            SignalAction actionAfter = new SignalAction(signalAfter, action.TypeOfAction);
            EmittFurther?.Invoke((this.Id, actionAfter), true);
        }

        if (IsStopActor)
            stopEverything();
    }

    //подготовка актора
    private void Start()
    {
        //создаём инстанс указанного сигнала - если этого не сделать, то будет один сигнал на всех акторов
        signal = Signal.Instance(signal, signalContent);
        signalAfterEmitting = Signal.Instance(signalAfterEmitting, signalAfterEmittingContent);
        //подключаем 
        foreach (var emittGetter in afterEmittedSignalGetters)
            LinkAfterEmittedSignalGetters(emittGetter);
    }
    public void SetSignal(Signal signal)
    {
        this.signal = signal;
        this.signalContent = signal.Content;
    }
    [SerializeField]
    protected Signal signal;
    [SerializeField]
    protected SignalContent signalContent;
    public Signal SignalPeek() => signal;

    public Signal signalAfterEmitting;
    public SignalContent signalAfterEmittingContent;
    public virtual void GiveSignalSafe(out SignalAction signalAction, SignalActionType actionType, Guid SignalGetterId, bool afterEmitting)
    {
        StartEmitting(SignalGetterId);
        Signal signal;
        this.signal.GiveSignal(out signal);
        signalAction = new SignalAction(signal, actionType);
        StopEmitting(SignalGetterId, signalAction, afterEmitting);
    }
    //на случай отработки ситуации, когда приёмника сигнала нет, а дообработку провести надо
    //например, когда шейкер соприкоснулся с коллайдером, который не участвует в системе активаторов
    //по логике такие шейкеры (если они одноразовые) надо просто уничтожать
    public void EmittFurtherWithoutGetter()
    {
        Debug.Log("started emitting without getter");
        SignalActionType actionType = SignalActionType.Adding;
        Guid SignalGetterId = this.Id;
        StartEmitting(SignalGetterId);
        Signal signal;
        this.signalAfterEmitting.GiveSignal(out signal);
        Debug.Log(signal.Content.name);
        SignalAction signalAction = new SignalAction(signal, actionType);
        StopEmitting(SignalGetterId, signalAction, false);
        Debug.Log("stopped emitting without getter");
    }
    //дообработчики - активируются после передачи сигнала актором, настраиваются на этапе создания объекта актора
    public List<FromEmitterSignalGetter> afterEmittedSignalGetters = new List<FromEmitterSignalGetter>();
    public void LinkAfterEmittedSignalGetters(FromEmitterSignalGetter afterEmittedSignalGetter) =>
        EmittFurther += afterEmittedSignalGetter.AddSignalToQueue;
    /*
    public void UnlinkActorEmitterActGetter(FromEmitterSignalGetter actorEmittedActGetter)
    {
        if (actorEmittedActGetters.Contains(actorEmittedActGetter))
        {
            actorEmittedActGetters.Remove(actorEmittedActGetter);
            EmittFurther -= actorEmittedActGetter.AddSignalToQueue;
        }
    }
    public void AddActorEmitterActGetter(FromEmitterSignalGetter actorEmittedActGetter)
    {
        if(!actorEmittedActGetters.Contains(actorEmittedActGetter))
        {
            actorEmittedActGetters.Add(actorEmittedActGetter);
            LinkActorEmitterActGetter(actorEmittedActGetter);
        }
    }
    */
    public event Action<object,bool> EmittFurther;
}
