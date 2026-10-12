using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "ScriptableObject/Ingredient")]
public class Ingredient : ScriptableObject
{
    public Sprite _sprite;

    [SerializeField]
    private IngredientsEnum _ingredientEnum;
    public IngredientsEnum IngredientEnum  => _ingredientEnum;
    public string _ingredientName => IngredientEnum.ToString();
    public int _ingredientID => (int)IngredientEnum;
}
