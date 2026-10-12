using Scripts.MovingThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowShakerBase : MonoBehaviour, IThrow
{
    public Transform startTransform;
    public GameObject Shaker;
    public bool canThrow = true;

    private MovementVariables movVars;
    void Start()
    {
        movVars = GetComponent<MovementVariables>();
    }

    [SerializeField]
    private CocktailInventory cocktailInventory;
    public void defineIsThrow(bool isThrow)
    {
        if (isThrow && canThrow && cocktailInventory?.isCocktailMixed == true && !(movVars?.grab == true))
            Throw();
    }

    public void Throw()
    {
        var shakerInstance = Instantiate(Shaker);
        ShakerTakeFlight shakerTakeFlight = shakerInstance.GetComponent<ShakerTakeFlight>();
        shakerTakeFlight.SetStartTransform(startTransform);
        shakerTakeFlight.Throw();

        shakerTakeFlight.SetCocktailSignal(cocktailInventory.Cocktail.signalContent);
        shakerInstance.SetActive(true);
        StartCoroutine(crtnJustThrowed());
    }
    public float WaitFor = 0.1f;

    IEnumerator crtnJustThrowed()
    {
        canThrow = false;
        yield return new WaitForSeconds(WaitFor);
        canThrow = true;
    }
}
