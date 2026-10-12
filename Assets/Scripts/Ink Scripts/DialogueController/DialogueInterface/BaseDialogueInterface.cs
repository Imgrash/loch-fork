using StorySpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StorySpace
{
    public abstract class BaseDialogueInterface : MonoBehaviour, IDialogueInterface
    {
        public GameObject clickableFieldObject;
        public GameObject textFieldObject;
        public ITextField storyField;
        public IChoices choicesRoot;
        public ISprites sprites;
        public GameObject spritePlacesRoot;
        public HandlerController handlersController;
        public IStoryInterface st;
        public string currentChunkOfStory;

        public abstract void activate(IStoryInterface si);
        public abstract void ShowText();
        public abstract void CreateChoices();
        public abstract void makeChoice(int indexChoice);
        public abstract void nextChunkOfStory();
        public abstract void deactivate();
        public abstract void HandleTags(List<string> tags);
    }
}