using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace StorySpace
{
    public class HandlerChangeName : MonoBehaviour, IHandler
    {
        public const string nameHandler = "CharName";
        public GameObject namePanel;

        public string getName() => nameHandler;

        public bool Handle(string tagValue = null)
        {
            if (!namePanel.activeInHierarchy)
                namePanel.SetActive(true);
            if (tagValue != null)
            {
                TMP_Text text = namePanel.GetComponentInChildren<TMP_Text>();
                text.SetText(tagValue);
            }
            return true;
        }
    }
}