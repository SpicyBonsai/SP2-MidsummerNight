using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lyr.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue currentDialogue;
        DialogueNode currentNode = null;
        bool isChoosing = false;
        
        private void Awake() {
             currentNode = currentDialogue.GetRootNode();
        }
        public bool IsChoosing()
        {
            return isChoosing;
        }


        public string GetText()
        {
            if(currentNode == null)
            {
                return "";
            }

            return currentNode.GetText();
        }

        public void Next()
        {
            int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if(numPlayerResponses > 0)
            {
                isChoosing = true;
                return;
            }

            DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Count());
            currentNode = children[randomIndex];
        }

        public void Quit()
        {
            currentDialogue = null;
            currentNode = null;
            isChoosing = false;
            //onConversationUpdated();
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            isChoosing = false;

            Next();
        }

        public bool HasNext()
        {
            return currentNode.GetChildren().Count > 0;
        }

        internal IEnumerable<DialogueNode> GetChoices()
        {
            foreach(DialogueNode node in currentDialogue.GetPlayerChildren(currentNode))
            {
                yield return node;  
            }
        }
    }
}
