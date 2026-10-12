using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractBase : MonoBehaviour, IInteract
{
    public InteractBase(string tagInteractedObject)
    {
        this.tagInteractedObject = tagInteractedObject;
    }
    [SerializeField]
    protected readonly string tagInteractedObject;
    public string TagInteractedObject => tagInteractedObject;
    public abstract void Interact(GameObject interactedObject);
}
