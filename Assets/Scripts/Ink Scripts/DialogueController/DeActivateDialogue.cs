using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace StorySpace
{
    public class DeActivateDialogue : MonoBehaviour
    {
        IDialogueInterface di;
        private void Start()
        {
            di = transform.parent.GetComponent<IDialogueInterface>();
            this.gameObject.GetComponent<Button>().onClick.AddListener(deactivate);
        }
        public void deactivate()
        {
            di.deactivate();
        }

    }
}
