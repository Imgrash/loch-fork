using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StorySpace
{
    public class HandlerLoadScene : MonoBehaviour, IHandler
    {
        public const string nameHandler = "LoadScene";

        public string getName() => nameHandler;

        public bool Handle(string tagValue = null)
        {
            SceneManager.LoadScene(tagValue);
            return true;
        }
    }
}