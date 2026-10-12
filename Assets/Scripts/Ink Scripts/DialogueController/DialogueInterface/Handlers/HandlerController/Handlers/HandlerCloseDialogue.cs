using UnityEngine;

namespace StorySpace
{
    public class HandlerCloseDialogue : MonoBehaviour, IHandler
    {
        public const string nameHandler = "closeDialogue";
        public DialogueInterface di;

        public string getName() => nameHandler;

        public bool Handle(string tagValue = null)
        {
            di.deactivate();
            return true;
        }
    }
}