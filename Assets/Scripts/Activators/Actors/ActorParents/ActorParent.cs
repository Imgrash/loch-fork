using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorParent : UniqueObject
{
    public bool IsChildActive = true;
    public void SetActiveChild(bool isActive)
    {
        childActor.enabled = isActive;
        Debug.Log($"enabled {isActive} obj {childActor.name}");
        IsChildActive = isActive;
    }
    public void SetChildSignal(SignalContent content)
    {
        Signal signal = ScriptableObject.CreateInstance<Signal>();
        signal.Content = content;
        childActor.SetSignal(signal);
    }
    public BaseActor childActor;
    private void Start()
    {
        if (childActor != null)
            childActor.Parent = this;
    }
}
