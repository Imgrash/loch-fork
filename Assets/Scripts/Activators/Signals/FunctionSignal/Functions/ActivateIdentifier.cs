using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActivatorTags
{
    BebebeBababa = 1
}

[CreateAssetMenu(fileName = "ActivateIdentifier", menuName = "ScriptableObject/SignalFunction/ActivateIdentifier")]
public class ActivateIdentifier : SignalFunctionBase
{
    public List<ActivatorTags> activatorTags;
    public override SignalFunctionType functionType => SignalFunctionType.ActivateIdentifier;
    public Vector3 changeSizeScale = new Vector3(1, 1, 1);
    public override void Process(GameObject gameObject, SignalGetterState State, SignalActionType actionType)
    {
        if (State is GetterStateIdentifyEmitter st)
        {
            //if (actionType == SignalActionType.Adding)
            {
                Debug.Log($"{gameObject.name}");
                gameObject.transform.localScale = Vector3.Scale(changeSizeScale, gameObject.transform.localScale);
            }
        }
    }
}
