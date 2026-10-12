using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SignalContentType
{
    Boolean,
    WithActor,
    Functions
}
public abstract class SignalContent : UniqueScriptable, ICloneable
{
    public virtual SignalContentType TypeOfContent => SignalContentType.Boolean;
    public GameObject go;
    public object Clone()
    {
        return MemberwiseClone();
    }
}
