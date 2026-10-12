using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Cocktail", menuName = "ScriptableObject/Cocktail")]
public class Cocktail : ScriptableObject
{
    public Sprite _sprite;

    [SerializeField]
    private CocktailsEnum _cocktailEnum;
    public CocktailsEnum CocktailEnum => _cocktailEnum;
    public string _cocktailName => CocktailEnum.ToString();
    public int _cocktailID => (int)CocktailEnum;
    [SerializeField]
    private List<IngredientsEnum> _ingredients;
    public string Recipe => string.Join(".", _ingredients.OrderBy(x => x).Select(x => ((int)x).ToString()));
    public SignalContent signalContent;
}
