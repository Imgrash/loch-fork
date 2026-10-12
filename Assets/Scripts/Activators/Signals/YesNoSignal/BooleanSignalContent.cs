using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BooleanSignalContent", menuName = "ScriptableObject/SignalContent/BooleanSignalContent")]
public class BooleanSignalContent : SignalContent
{
    public override SignalContentType TypeOfContent => SignalContentType.Boolean;
    [SerializeField]
    protected bool _enabled = true;
}
