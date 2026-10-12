using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace StorySpace
{
    public class HandlerCreateSpritePlaceHandler : MonoBehaviour, IHandler
    {
        public const string nameHandler = "putEmptySprite";
        public Sprites sprites;

        public string getName() => nameHandler;

        public bool Handle(string tagValue = null)
        {
            string[] tag = tagValue.Split('-');
            sprites.AddSpriteField(tag[0], tag[1]);
            return true;
        }
    }
}