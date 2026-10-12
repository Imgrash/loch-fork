using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CocktailInventory : MonoBehaviour
{
    public CocktailsManager Manager;
    public CocktailsUI cocktailsUI;
    [SerializeField]
    private List<Ingredient> collectedIngredients;
    public Cocktail Cocktail;
    public bool isCocktailMixed => Cocktail != null;
    [SerializeField]
    private int inventorySize = 2;
    private int currentIngredientCellNumber = 0;
    public string CurrentRecipe => string.Join(".", collectedIngredients.OrderBy(x => x._ingredientID).Select(x => (x._ingredientID).ToString()));

    public bool Collect(Ingredient ingredient)
    {
        Debug.Log("trycollect");
        if (currentIngredientCellNumber < inventorySize)
        {
            collectedIngredients.Add(ingredient);
            cocktailsUI.SetNextIngredientImage(ingredient, currentIngredientCellNumber);
            currentIngredientCellNumber++;

            if(currentIngredientCellNumber == inventorySize)
                cocktailsUI.ShowHintMix();

            return true;
        }
        return false;
    }
    public bool Mix()
    {
        Cocktail newCocktail = Manager.GetCocktailByRecipe(CurrentRecipe);
        if(newCocktail != null)
        {
            Cocktail = newCocktail;
            cocktailsUI.SetCocktailImage(Cocktail);
            collectedIngredients.Clear();
            cocktailsUI.ShowHintThrow();
            cocktailsUI.HideHintMix();
            currentIngredientCellNumber = 0;
            return true;
        }
        return false;
    }
}
