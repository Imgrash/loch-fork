using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace StorySpace
{
    public class HandlerDeleteSpritePlaceHandler : MonoBehaviour, IHandler
    {
        public const string nameHandler = "deleteSprite";
        public Sprites sprites;

        public string getName() => nameHandler;

        public bool Handle(string tagValue = null)
        {
            sprites.DestroySpriteField(tagValue);
            return true;
        }
    }
}