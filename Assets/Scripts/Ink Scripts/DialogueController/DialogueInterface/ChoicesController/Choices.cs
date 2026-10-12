using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace StorySpace
{
    public class Choices : BaseChoices
    {
        private void deleteChildren()
        {
            int childCount = choicesContentObject.transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                Transform child = choicesContentObject.transform.GetChild(i);
                Destroy(child.gameObject);
            }
        }
        private void HideViewer()
        {
            if(viewPortObject.activeInHierarchy)
                viewPortObject.SetActive(false);
        }
        private void ShowViewer()
        {
            if (!viewPortObject.activeInHierarchy)
                viewPortObject.SetActive(true);
        }

        public override void SetDialogueInterface(IDialogueInterface di)
        {
            choiceDialogueInterface = di;
        }
        public override void changeCurrentChoices(Dictionary<int, string> CurrentDialogueChoices)
        {
            deleteChildren();
            if (!CurrentDialogueChoices.Any())
            {
                HideViewer();
            }
            else
            {
                ShowViewer();
                foreach (var a in CurrentDialogueChoices)
                {
                    var btn = Instantiate(choiceButtonTemplate);
                    btn.transform.SetParent(choicesContentObject.transform);

                    var textObj = btn.transform.GetChild(0).gameObject;
                    var textComponent = textObj.GetComponent<TMP_Text>();
                    var oneChoice = btn.GetComponent<IOneChoice>();
                    oneChoice.Set(this, a, new TextFieldCanvas(textComponent));
                }
            }
        }
        public override void makeChoice(int i) => choiceDialogueInterface.makeChoice(i);
    }
}