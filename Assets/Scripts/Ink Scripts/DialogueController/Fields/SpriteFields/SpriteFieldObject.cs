using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class SpriteFieldObject : MonoBehaviour, ISpriteField
{

    public SpriteRenderer srender;
    public void Awake()
    {
        srender = this.gameObject.GetComponent<SpriteRenderer>();
    }

    const string spritesRootFolderPath = "Sprites";

    public void Set(GameObject place, string Name)
    {
        SetPlace(place);
        SetName(Name);
    }

    string namePlace = null;
    public void SetPlace(GameObject place)
    {
        namePlace = place.name;
        Transform tr = gameObject.transform, pltr = place.transform;
        (tr.position, tr.rotation) = (pltr.position, pltr.rotation);
    }
    public string GetPlace() => namePlace;

    string nameCharacter = null;
    public void SetName(string Name)
    {
        nameCharacter = Name;
    }
    public string GetName() => nameCharacter;
    private Sprite getSprite(string spriteName, string path = null)
    {
        string pp = spritesRootFolderPath + '/'
                + (string.IsNullOrEmpty(path) ? nameCharacter : path) + '/'
                + spriteName;
        Debug.Log(pp);
        return Resources.Load<Sprite>(
              pp  
            );
    }

    public void ShowSprite(Sprite sprite)
    {
        srender.sprite = sprite;
    }

    public void ShowSprite(string spriteName, string path = null)
    {
        Sprite sprite = null;
        try
        {
            sprite = getSprite(spriteName, path);
            if (sprite == null)
                throw (new Exception("Спрайт \""+ (string.IsNullOrEmpty(path) ? nameCharacter : path) + '\\' + spriteName + "\" не найден"));
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            return;
        }
        ShowSprite(sprite);
    }
    public void Clear()
    {
        srender.sprite = null;
    }
    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
