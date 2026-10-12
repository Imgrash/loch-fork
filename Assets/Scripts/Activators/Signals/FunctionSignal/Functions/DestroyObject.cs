using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DestroyObject", menuName = "ScriptableObject/SignalFunction/DestroyObject")]
public class DestroyObject : SignalFunctionBase
{
    public override SignalFunctionType functionType => SignalFunctionType.DestroyThis;

    public override void Process(GameObject gameObject, SignalGetterState State, SignalActionType actionType)
    {
        if (actionType == SignalActionType.Adding)
        {
            Destroy(gameObject);
        }
    }
}
