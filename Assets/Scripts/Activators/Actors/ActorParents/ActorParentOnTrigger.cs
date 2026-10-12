using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorParentOnTrigger : ActorParent
{
    public void AddSignalByTriggerAction(Collider other, SignalActionType signalActionType)
    {
        if (!IsChildActive)
            return;
        other.gameObject.TryGetComponent<FromActorSignalGetter>(out FromActorSignalGetter actGetter);
        other.gameObject.TryGetComponent<ActorGetterParent>(out ActorGetterParent actGetterParent);
        if (actGetter == null && actGetterParent != null)
            actGetter = actGetterParent.childGetter;
        if (childActor != null)
        {
            if (actGetter != null)
            {
                actGetter.AddSignalToQueue((childActor, signalActionType), false);
            }
            else if(IsEmittByCollidingAnything)
            {
                Debug.Log("yep, just collided, no getters found");
                childActor.EmittFurtherWithoutGetter();
            }
        }
    }
    Collider currentCollider = null;
    private void OnTriggerEnter(Collider other)
    {
        if (currentCollider != null && currentCollider == other)
            return;
        currentCollider = other;
        Debug.Log("OnTriggerEnter");
        AddSignalByTriggerAction(other, SignalActionType.Adding);
    }
    private void OnTriggerExit(Collider other)
    {/*
        if (currentCollider != null && currentCollider == other)
            currentCollider = null;
        AddSignalByTriggerAction(other, SignalActionType.Extracting);
        //*/
    }

    public bool IsEmittByCollidingAnything = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (currentCollider != null && currentCollider == collision.collider)
            return;
        currentCollider = collision.collider;
        Debug.Log("OnCollisionEnter");
        AddSignalByTriggerAction(collision.collider, SignalActionType.Adding);
    }
    private void OnCollisionExit(Collision collision)
    {
        /*
        if (currentCollider != null && currentCollider == collision.collider)
            currentCollider = null;
        AddSignalByTriggerAction(collision.collider, SignalActionType.Extracting);
        //*/
    }
}
