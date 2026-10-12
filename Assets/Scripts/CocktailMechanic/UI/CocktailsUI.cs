using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocktailsUI : MonoBehaviour
{
    [SerializeField]
    private ImageSetter cocktailImage;

    [SerializeField]
    private List<ImageSetter> ingredientsImageSetterList;

    public GameObject HintMix, HintThrow;

    public void ShowHintMix() => HintMix.SetActive(true);
    public void HideHintMix() => HintMix.SetActive(false);
    public void ShowHintThrow() => HintThrow.SetActive(true);
    public void HideHintThrow() => HintThrow.SetActive(false);


    public bool SetNextIngredientImage(Ingredient ingredient, int _currentIngredientNumber)
    {
        if (_currentIngredientNumber < ingredientsImageSetterList.Count)
        {
            ingredientsImageSetterList[_currentIngredientNumber].SetImage(ingredient._sprite);
            return true;
        }
        return false;
    }
    public void SetCocktailImage(Cocktail cocktail)
    {
        cocktailImage.SetImage(cocktail._sprite);
        foreach(var ingredientImageSetter in ingredientsImageSetterList)
            ingredientImageSetter.DisableImage();
    }
}
