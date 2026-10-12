using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StorySpace
{
    public class InteractActivateStory : InteractBase
    {
        public InteractActivateStory() : base("InteractInk") { }
        public override void Interact(GameObject interactedObject)
        {
            var si = interactedObject.GetComponent<IStoryInterface>();
            si.activateDialogueWindow();
        }

    }
}
