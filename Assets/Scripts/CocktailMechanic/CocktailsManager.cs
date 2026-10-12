using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CocktailsManager : MonoBehaviour
{
    public static CocktailsManager instance = null;

    public List<Cocktail> cocktailsList = new();
    public Dictionary<string, Cocktail> recipes = new();
    public Cocktail GetCocktailByRecipe(string recipe) => recipes.ContainsKey(recipe) ? recipes[recipe] : null;

    public void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance = this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        cocktailsList = Resources.LoadAll<Cocktail>("ScriptableObjects/Cocktails").ToList();
        foreach(var cocktail in cocktailsList)
        {
            recipes.TryAdd(cocktail.Recipe, cocktail);
        }
    }
}
