using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SignalContentFunctions", menuName = "ScriptableObject/SignalContent/SignalContentFunctions")]
public class SignalContentFunctions : SignalContent
{
    public override SignalContentType TypeOfContent => SignalContentType.Functions;

    public List<SignalFunctionBase> ListFunctions;
}
