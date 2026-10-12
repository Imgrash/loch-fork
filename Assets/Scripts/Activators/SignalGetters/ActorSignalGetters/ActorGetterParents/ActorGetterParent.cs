using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorGetterParent : UniqueObject
{
    public FromActorSignalGetter childGetter;
    private void Start()
    {
        if (childGetter != null)
            childGetter.Parent = this;
    }
}
