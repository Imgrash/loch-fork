using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReactorProcessFunctions : ReactorBase
{
    public bool IsWhiteList;
    public ListFunctions functions;
    public override bool Process(SignalAction signalAction, ref SignalAction signalActionNew)
    {
        if (checker == null || checker != null && checker.Evaluate(signalAction.SignalInAction, State, signalAction.TypeOfAction))
        {
            if (signalAction.SignalInAction.Content.TypeOfContent == SignalContentType.Functions)
            {
                SignalContentFunctions content = (SignalContentFunctions)signalAction.SignalInAction.Content;
                foreach (var func in content.ListFunctions)
                {
                    bool funcTypesContain = functions.signalFunctionTypes?.Contains(func.functionType) == true;
                    if (IsWhiteList ? funcTypesContain : !funcTypesContain)
                    {
                        func.Process(this.gameObject, State, signalAction.TypeOfAction);
                    }
                }
            }
            signalActionNew = Preparation.PrepareSignal(signalAction, State);
            return true;
        }
        return false;
    }
}
