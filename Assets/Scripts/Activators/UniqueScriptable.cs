using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class UniqueScriptable : ScriptableObject
{
    protected Guid? id;
    public Guid Id => id ??= Guid.NewGuid();
}
