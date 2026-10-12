using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeSize", menuName = "ScriptableObject/SignalFunction/ChangeSize")]
public class ChangeSize : SignalFunctionBase
{
    public override SignalFunctionType functionType => SignalFunctionType.ChangeSize;
    public Vector3 changeSizeScale = new Vector3(1, 1, 1);
    public override void Process(GameObject gameObject, SignalGetterState State, SignalActionType actionType)
    {
        if (actionType == SignalActionType.Adding)
        {
            Debug.Log($"{gameObject.name}");
            gameObject.transform.localScale = Vector3.Scale(changeSizeScale, gameObject.transform.localScale);
        }
    }
}
