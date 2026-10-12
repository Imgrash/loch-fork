using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpriteField
{
    public void Set(GameObject place, string Name);

    public void SetPlace(GameObject place);
    public string GetPlace();

    public void SetName(string Name);
    public string GetName();

    public void ShowSprite(Sprite sprite);
    public void ShowSprite(string spriteName, string path = null);
    public void Clear();
    public void DestroySelf();
}
