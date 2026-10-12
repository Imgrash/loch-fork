using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StorySpace
{
    public class OneChoice : MonoBehaviour, IOneChoice
    {
        private int choiceIndex;
        private string choiceName;
        private IChoices root;
        public ITextField tf;

        public void Set(Choices root, KeyValuePair<int, string> choiceVal, ITextField tf)
        {
            this.choiceIndex = choiceVal.Key;
            this.choiceName = choiceVal.Value;
            this.root = root;
            this.tf = tf;
            tf.ShowText(choiceName);
            this.gameObject.GetComponent<Button>().onClick.AddListener(makeChoice);
        }
        public void makeChoice() => root.makeChoice(choiceIndex);
    }
}
