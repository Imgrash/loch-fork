using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorGetterParentOnTrigger : ActorGetterParent
{
    public void AddSignalByTriggerAction(Collider other, SignalActionType signalActionType)
    {
        ActorParent actorParent = null;
        if (other.gameObject.TryGetComponent<BaseActor>(out BaseActor actor) ||
            other.gameObject.TryGetComponent<ActorParent>(out actorParent) && actorParent.childActor != null)
        {
            if (actorParent != null && actorParent is ActorParentOnTrigger)
                return;
            if(actor == null && actorParent != null)
                actor = actorParent.childActor;
            childGetter.AddSignalToQueue((actor, signalActionType), false);
        }
    }
    protected virtual void OnTriggerEnter(Collider other) => AddSignalByTriggerAction(other, SignalActionType.Adding);
    protected virtual void OnTriggerExit(Collider other) => AddSignalByTriggerAction(other, SignalActionType.Extracting);
}
