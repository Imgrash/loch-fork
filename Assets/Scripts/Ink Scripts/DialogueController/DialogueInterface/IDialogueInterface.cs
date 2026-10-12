using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StorySpace
{
    public interface IDialogueInterface
    {
        public void activate(IStoryInterface si);
        public void nextChunkOfStory();
        public void ShowText();
        public void CreateChoices();
        public void makeChoice(int indexChoice);
        public void deactivate();
        public void HandleTags(List<string> tags);
    }
}