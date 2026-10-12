using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SignalContentWithActor", menuName = "ScriptableObject/SignalContent/SignalContentWithActor")]
public class SignalContentWithActor : SignalContent
{
    public override SignalContentType TypeOfContent => SignalContentType.WithActor;
    public BaseActor actor;
}
