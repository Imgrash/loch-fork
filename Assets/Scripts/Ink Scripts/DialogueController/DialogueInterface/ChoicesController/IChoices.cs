using System.Collections.Generic;

namespace StorySpace
{
    public interface IChoices
    {
        public void SetDialogueInterface(IDialogueInterface di);
        public void changeCurrentChoices(Dictionary<int, string> CurrentDialogueChoices);
        public void makeChoice(int i);
    }
}