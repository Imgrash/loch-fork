using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StorySpace
{
    public interface IHandler
    {
        public string getName();
        public bool Handle(string tagValue = null);
    }
}