using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StorySpace
{
    public class HandlerHideName : MonoBehaviour, IHandler
    {
        public const string nameHandler = "HideName";
        public GameObject namePanel;

        public string getName() => nameHandler;

        public bool Handle(string tagValue = null)
        {
            if (namePanel.activeInHierarchy)
                namePanel.SetActive(false);
            return true;
        }
    }
}
