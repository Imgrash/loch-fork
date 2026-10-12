using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StorySpace
{
    public abstract class BaseChoices : MonoBehaviour, IChoices
    {
        public GameObject choiceButtonTemplate;
        public GameObject choicesContentObject;
        public GameObject viewPortObject;
        public IDialogueInterface choiceDialogueInterface;

        public abstract void SetDialogueInterface(IDialogueInterface di);
        public abstract void changeCurrentChoices(Dictionary<int, string> CurrentDialogueChoices);
        public abstract void makeChoice(int i);

    }
}