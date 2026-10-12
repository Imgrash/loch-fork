using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerClickHandler
{ 
    public event EventHandler ev;
    public void OnPointerClick(PointerEventData e)
    {
        ev.Invoke(this, EventArgs.Empty);
    }
}
