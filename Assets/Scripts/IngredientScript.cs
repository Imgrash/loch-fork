using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientScript : MonoBehaviour
{
    [SerializeField]
    private Ingredient ingredient;
    public Ingredient IngredientGet { 
        get 
        {
            Debug.Log(ingredient._ingredientName);
            return ingredient; 
        }
    }
}
