using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyr.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue currentDialogue;
        
        public string GetText()
        {
            return currentDialogue.GetRootNode().GetText();
        }
    }
}
