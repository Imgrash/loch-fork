using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace StorySpace
{
    public class HandlerChangeSpriteHandler : MonoBehaviour, IHandler
    {
        public const string nameHandler = "changeSprite";
        public Sprites sprites;

        public string getName() => nameHandler;

        public bool Handle(string tagValue = null)
        {
            string[] vals = tagValue.Split('-');
            if(vals.Length == 1)
                sprites.SetSprite(vals[0]);
            else
                sprites.SetSprite(vals[0], vals[1]);
            return true;
        }
    }
}