using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "GetterStateIdentifyEmitter", menuName = "ScriptableObject/SignalGetterState/GetterStateIdentifyEmitter")]
public class GetterStateIdentifyEmitter : SignalGetterState
{
    public int counter = 0;
    public int goalCounter = 1;
    public bool withIdentification = false;
    public bool withRemoving = false;
    public List<ActivatorTags> identifyTag;

    public List<Guid> actorsGot = new();

}
