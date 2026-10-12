using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractCollectIngredient : InteractBase
{
    [SerializeField]
    private CocktailInventory cocktailInventory;
    public InteractCollectIngredient() : base("Ingredient") { }
    public override void Interact(GameObject interactedObject)
    {
        IngredientScript ingredientScript = interactedObject.GetComponent<IngredientScript>();
        Ingredient ingredient = ingredientScript.IngredientGet;
        if (cocktailInventory.Collect(ingredient))
        {
            Destroy(interactedObject);
            Debug.Log($"собран ингредиент {ingredient._ingredientName}");
        }
        else
            Debug.Log($"ингредиент {ingredient._ingredientName} не собран - не хватило места");
    }
}
