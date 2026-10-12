using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public abstract class MultipleConditionsChecker : SignalChecker
{
    [SerializeField]
    protected List<SignalChecker> conditions = new();
    public MultipleConditionsChecker(List<SignalChecker> _conditions)
    { 
        conditions = _conditions; 
    }
}
