using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ReactorBase : MonoBehaviour, IReactor
{
    //изменяет сигнал для дальнейшей передачи
    public PrepareBase Preparation;

    private void Start()
    {
        PrepareScriptables();
    }

    private void PrepareScriptables()
    {
        State = State == null ? ScriptableObject.CreateInstance<GetterStateEmpty>() : (SignalGetterState)State.Clone();
        Preparation = Preparation == null ? ScriptableObject.CreateInstance<PrepareEmpty>() : (PrepareBase)Preparation.Clone();
    }

    //состояние приёмника - пока никак не задействуется
    public SignalGetterState State;
    //проверка сигнала на соответствие каким-либо условиям
    public SignalChecker checker;

    public abstract bool Process(SignalAction signalAction, ref SignalAction signalActionNew);
}
