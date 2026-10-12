using Scripts.MovingThings;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionsDispatcher: MonoBehaviour
{
    public GameObject Mark;
    private GameObject interactedObject;

    private MovementVariables movVars;
    private bool IsSuitableTagForInteraction = false;
    void Start()
    {
        movVars = GetComponent<MovementVariables>();
    }
    void OnTriggerEnter(Collider c)
    {
        SetVars(c.gameObject);
    }
    
    void OnTriggerExit()
    {
        if (IsSuitableTagForInteraction)
            ClearVars();
    }
    private void ClearVars()
    {
        interactedObject = null;
        Mark.SetActive(false);
        IsSuitableTagForInteraction = false;
    }
    private void SetVars(GameObject interactedObject)
    {
        checkIfTagIsSuitable(interactedObject.tag);
        if (IsSuitableTagForInteraction)
        {
            this.interactedObject = interactedObject;
            Mark.SetActive(true);
            Debug.Log("F");
        }
    }
    public bool isStillInteracting = false;
    private void checkIfTagIsSuitable(string tag) => IsSuitableTagForInteraction = interactions.Where(x => tag == x.TagInteractedObject).Any();
    public void defineInteractionInput(bool isInteracting)
    {
        if (!isStillInteracting && isInteracting && IsSuitableTagForInteraction && !(movVars?.grab == true))
        {
            isStillInteracting = true;
            DispatchInteraction();
            isStillInteracting = false;
        }
    }
    [SerializeField]
    private List<InteractBase> interactions = new List<InteractBase>();
    public void DispatchInteraction()
    {
        var suitableInteractions = interactions.Where(x => interactedObject.tag == x.TagInteractedObject).ToList();
        suitableInteractions?.ForEach(x => x.Interact(interactedObject));
        ClearVars();
    }
}
