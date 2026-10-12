using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultipleConditionsORChecker : MultipleConditionsChecker
{
    public MultipleConditionsORChecker(List<SignalChecker> _conditions)
        : base(_conditions) { }
    public override bool Evaluate(Signal _signal, SignalGetterState _actGetterState, SignalActionType _signalActionType)
    {
        return conditions.All(x => x.Evaluate(_signal, _actGetterState, _signalActionType));
    }
}
