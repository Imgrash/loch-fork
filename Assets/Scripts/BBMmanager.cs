using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBMmanager : MonoBehaviour  //Battle Bar Mechanic
{
    public GameObject Ingredient1, Ingredient2;
    public GameObject Cocktail, Shaker;
    public GameObject HintCocktail, HintThrough;

    // Update is called once per frame
    void Update()
    {
        if (Ingredient1.activeSelf == true && Ingredient2.activeSelf == true)
            HintCocktail.SetActive(true);
        if (Input.GetKey(KeyCode.E) && HintCocktail.activeSelf != false)
        {
            Cocktail.SetActive(true);
            HintCocktail.SetActive(false);
            Ingredient1.SetActive(false);
            Ingredient2.SetActive(false);
            HintThrough.SetActive(true);
        }
        if (Input.GetKey(KeyCode.Q) && HintThrough.activeSelf != false)
        {
            Shaker.SetActive(true);
        }
    }
}
