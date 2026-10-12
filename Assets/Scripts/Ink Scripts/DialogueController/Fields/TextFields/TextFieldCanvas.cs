using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace StorySpace
{
    public class TextFieldCanvas : ITextField
    {
        public TMP_Text field;
        public TextFieldCanvas(TMP_Text field)
        {
            this.field = field;
        }
        public void ShowText(string Text)
        {
            field.text = Text;
        }
        public void Clear()
        {
            field.text = string.Empty;
        }
    }
}