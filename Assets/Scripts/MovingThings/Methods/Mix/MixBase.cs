using RealtimeCSG.Legacy;
using Scripts.MovingThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixBase : MonoBehaviour, IMix
{
    private MovementVariables movVars;
    void Start()
    {
        movVars = GetComponent<MovementVariables>();
    }

    [SerializeField]
    private CocktailInventory cocktailInventory;
    public void defineIsMix(bool isMix)
    {
        if (isMix && !(movVars?.grab == true))
        {
            Mix();
        }
    }

    public void Mix()
    {
        cocktailInventory.Mix();
    }

}
