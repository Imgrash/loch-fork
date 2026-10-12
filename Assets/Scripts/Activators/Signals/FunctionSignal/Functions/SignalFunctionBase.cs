using UnityEngine;

public enum SignalFunctionType
{
    ChangeSize = 0,
    DestroyThis,
    ActivateIdentifier
}

public abstract class SignalFunctionBase : ScriptableObject, IFunction
{
    public virtual SignalFunctionType functionType => SignalFunctionType.ChangeSize;
    public abstract void Process(GameObject gameObject, SignalGetterState State, SignalActionType actionType);
}
