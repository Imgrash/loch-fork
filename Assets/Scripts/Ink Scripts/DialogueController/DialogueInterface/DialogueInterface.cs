using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace StorySpace
{
    public class DialogueInterface : BaseDialogueInterface
    {
        public static DialogueInterface instance;
        public void Start()
        {
            clickableFieldObject.GetComponent<Clickable>().ev += nextChunkOfStoryEventHandler;
            instance = this;
        }
        public override void activate(IStoryInterface si)
        {
            if (this.gameObject.activeInHierarchy)
                return;
            this.st = si;
            si.goToKnot("begin");
            this.gameObject.SetActive(true);

            choicesRoot = this.GetComponent<IChoices>();
            choicesRoot.SetDialogueInterface(this);

            sprites = this.GetComponent<ISprites>();
            sprites.AddPlaces(spritePlacesRoot);

            storyField = new TextFieldCanvas(textFieldObject.GetComponent<TMP_Text>());
            nextChunkOfStory();
        }
        public override void ShowText() => storyField.ShowText(currentChunkOfStory);
        public override void CreateChoices() => choicesRoot.changeCurrentChoices(st.CurrentDialogueChoices());
        public override void makeChoice(int indexChoice)
        {
            st.MakeChoice(indexChoice); 
            nextChunkOfStory();
        }
        public void nextChunkOfStoryEventHandler(object sender, EventArgs e)
        {
            nextChunkOfStory();
        }
        public override void nextChunkOfStory()
        {
            bool canContinue = st.nextChunkOfStory(out currentChunkOfStory);
            if (canContinue)
            {
                ShowText();
                CreateChoices();
                HandleTags(st.getCurrentTags());
            }
            else if(!st.CurrentDialogueChoices().Any())
            {
                deactivate();
            }

        }
        public override void deactivate()
        {
            this.st = null;
            sprites.Clear();
            gameObject.SetActive(false);
        }
        public override void HandleTags(List<string> tags)
        {
            foreach (var tag in tags)
            {
                string[] splitTag = tag.Split(':');
                var handler = handlersController.getHandler(splitTag[0]);
                if (handler != null)
                {
                    Debug.Log(handler.getName());
                    handler.Handle(splitTag.Count() == 2 ? splitTag[1] : null);
                    Debug.Log("Тег \"" + tag + "\" обработан");
                }
                else
                {
                    Debug.Log("Тег \"" + tag + "\" не обработан");
                }
            }
        }
    }

}