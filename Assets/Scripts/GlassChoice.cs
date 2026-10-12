using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlassChoice : MonoBehaviour
{
    //система из трёх кнопок
    //при нажатии кнопки проверяем остальные стаканы и выставляем их SetActive в false
    //после этого выставляем нужный стакан в true

    public GameObject MiniGlass, Glass, MaxGlass, MiniGlassIce, GlassIce, MaxGlassIce, MiniGlassDrink, GlassDrink, MaxGlassDrink;

    public void PickTheGlass(GameObject glass)
    {
        if (MiniGlass != false) { MiniGlass.SetActive(false); }
        if (Glass != false) { Glass.SetActive(false); }
        if (MaxGlass != false) { MaxGlass.SetActive(false); }

        glass.SetActive(true);

        if (glass.name == "MiniGlass") { MiniGlassIce.SetActive(true); MiniGlassDrink.SetActive(true); } else { MiniGlassIce.SetActive(false); MiniGlassDrink.SetActive(false); }
        if (glass.name == "Glass") { GlassIce.SetActive(true); GlassDrink.SetActive(true); } else { GlassIce.SetActive(false); GlassDrink.SetActive(false); }
        if (glass.name == "MaxGlass") { MaxGlassIce.SetActive(true); MaxGlassDrink.SetActive(true); } else { MaxGlassIce.SetActive(false); MaxGlassDrink.SetActive(false); }
    }

    public void PickTheIce(GameObject ice)
    {
        for (int i = 0; i < 3; i++)
        {
            MiniGlassIce.transform.GetChild(i).gameObject.SetActive(false);
            GlassIce.transform.GetChild(i).gameObject.SetActive(false);
            MaxGlassIce.transform.GetChild(i).gameObject.SetActive(false); 
        }
        for (int i = 0; i < 6; i++) 
        { 
            MiniGlassDrink.transform.GetChild(i).gameObject.SetActive(false);
            GlassDrink.transform.GetChild(i).gameObject.SetActive(false);
            MaxGlassDrink.transform.GetChild(i).gameObject.SetActive(false);
        }

        if (ice.name == "MiniIce")
        {
            if (MiniGlassIce != false)
            {
                MiniGlassIce.transform.GetChild(0).gameObject.SetActive(true);
                MiniGlassDrink.transform.GetChild(3).gameObject.SetActive(true);
            }
            if (GlassIce != false)
            {
                GlassIce.transform.GetChild(0).gameObject.SetActive(true);
                GlassDrink.transform.GetChild(3).gameObject.SetActive(true);
            }
            if (MaxGlassIce != false)
            {
                MaxGlassIce.transform.GetChild(0).gameObject.SetActive(true);
                MaxGlassDrink.transform.GetChild(3).gameObject.SetActive(true);
            }
        }

        if (ice.name == "Ice")
        {
            if (MiniGlassIce != false)
            {
                MiniGlassIce.transform.GetChild(1).gameObject.SetActive(true);
                MiniGlassDrink.transform.GetChild(4).gameObject.SetActive(true);
            }
            if (GlassIce != false)
            {
                GlassIce.transform.GetChild(1).gameObject.SetActive(true);
                GlassDrink.transform.GetChild(4).gameObject.SetActive(true);
            }
            if (MaxGlassIce != false)
            {
                MaxGlassIce.transform.GetChild(1).gameObject.SetActive(true);
                MaxGlassDrink.transform.GetChild(4).gameObject.SetActive(true);
            }
        }

        if (ice.name == "MaxIce")
        {
            if (MiniGlassIce != false)
            {
                MiniGlassIce.transform.GetChild(2).gameObject.SetActive(true);
                MiniGlassDrink.transform.GetChild(5).gameObject.SetActive(true);
            }
            if (GlassIce != false)
            {
                GlassIce.transform.GetChild(2).gameObject.SetActive(true);
                GlassDrink.transform.GetChild(5).gameObject.SetActive(true);
            }
            if (MaxGlassIce != false)
            {
                MaxGlassIce.transform.GetChild(2).gameObject.SetActive(true);
                MaxGlassDrink.transform.GetChild(5).gameObject.SetActive(true);
            }
        }
    }

    public void PickTheDrink(GameObject taste)
    {
        GameObject glass = FindActive();

        if (taste.name == "PickTea")
        {
            glass.transform.GetChild(0).gameObject.SetActive(true);
            if (glass.transform.GetChild(1).gameObject.activeInHierarchy == true)
            {
                glass.transform.GetChild(0).gameObject.SetActive(false);
                glass.transform.GetChild(1).gameObject.SetActive(false);
                glass.transform.GetChild(2).gameObject.SetActive(true); 
            }
        }

        if (taste.name == "PickJam")
        {
            glass.transform.GetChild(1).gameObject.SetActive(true);
            if (glass.transform.GetChild(0).gameObject.activeInHierarchy == true)
            {
                glass.transform.GetChild(0).gameObject.SetActive(false);
                glass.transform.GetChild(1).gameObject.SetActive(false);
                glass.transform.GetChild(2).gameObject.SetActive(true);
            }

        }
    }

    private GameObject FindActive()
    {
        if (MiniGlassDrink.activeInHierarchy != false)
            return MiniGlassDrink;
        else if (GlassDrink.activeInHierarchy != false)
            return GlassDrink;
        else if (MaxGlassDrink.activeInHierarchy != false)
            return MaxGlassDrink;
        else
            return null; 
    }
}