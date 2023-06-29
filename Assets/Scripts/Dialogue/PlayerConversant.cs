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
        DialogueInitiator currentConversant = null;
        bool isChoosing = false;
        
        //public event Action onConversationUpdated;

        public void StartDialogue(Dialogue newDialogue, DialogueInitiator conversant = null)
        {
            currentConversant = conversant;
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            //onConversationUpdated();
        }
        public void Quit()
        {
            currentDialogue = null;
            currentConversant = null;
            currentNode = null;
            isChoosing = false;
            //onConversationUpdated();
        }

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
                //onConversationUpdated();
                return;
            }

            DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Count());
            currentNode = children[randomIndex];
            //onConversationUpdated();
        }

        public void ResetDialogue()
        {
            currentNode = currentDialogue.GetRootNode();
        }


        public void SelectChoice(DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            TriggerExitAction();
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

        public void TriggerEnterAction()
        {
            if (currentNode != null)
            {
                TriggerAction(currentNode.GetOnEnterAction());
            }
        }

        public void TriggerExitAction()
        {
            if (currentNode != null)
            {
                TriggerAction(currentNode.GetOnExitAction());
            }
        }

        public void TriggerOnTime()
        {
            if (currentNode != null)
            {
                TriggerAction(currentNode.GetOnTimeAction());
            }
        }

        public void TriggerAction(string action)
        {
            if(action == "") return;

            foreach (DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>())
            {
                trigger.Trigger(action);
            }
        }

    }
}
