using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListFunctions", menuName = "ScriptableObject/ListFunctions")]
public class ListFunctions : ScriptableObject
{
    public List<SignalFunctionType> signalFunctionTypes;
}
