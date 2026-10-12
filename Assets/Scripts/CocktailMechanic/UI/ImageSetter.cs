using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageSetter : MonoBehaviour
{
    private Image _image;
    void Start()
    {
        _image = GetComponent<Image>();
        _image.enabled = false;
    }

    public void SetImage(Sprite sprite)
    {
        _image.sprite = sprite;
        _image.enabled = true;
    }
    public void DisableImage()
    {
        _image.enabled = false;
    }
}
