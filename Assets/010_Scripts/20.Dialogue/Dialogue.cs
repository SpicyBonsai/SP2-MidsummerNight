using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Lyr.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        List<DialogueNode> nodes = new List<DialogueNode>();

        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();



        private void OnValidate() 
        {

            nodeLookup.Clear();

            foreach(DialogueNode node in GetAllNodes())
            {
                if (node != null)
                {
                    nodeLookup[node.name] = node;
                }
            }
        }
        
        private void Awake() 
        {
            nodeLookup.Clear();

            foreach(DialogueNode node in GetAllNodes())
            {
                if (node != null)
                {
                    nodeLookup[node.name] = node;
                }
            }
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode parentNode)
        {
            foreach(DialogueNode node in GetAllChildren(parentNode))
            {
                if(node.IsPlayerSpeaking())
                {
                    yield return node;
                }
            }
        }

        public IEnumerable<DialogueNode> GetAIChildren(DialogueNode parentNode)
        {
            foreach(DialogueNode node in GetAllChildren(parentNode))
            {
                //the code inside here doesn't seem to be running
                //DebugConsole.GetInstance().LogMessage("GetAIChildren() called; current node is " + node.name);
                if(!node.IsPlayerSpeaking())
                {
                    yield return node;
                }
            }
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {

            foreach(string childID in parentNode.GetChildren())
            {
                if(nodeLookup.ContainsKey(childID))
                {
                    yield return nodeLookup[childID];
                }
            }
        }

#if UNITY_EDITOR

        public void CreateNode(DialogueNode parent)
        {
            DialogueNode newNode = MakeNode(parent);
            Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");
            Undo.RecordObject(this, "Added new node");
            AddNode(newNode);
        }

        public void RemoveNode(DialogueNode nodeToDelete)
        {
            nodes.Remove(nodeToDelete);
            CleanDanglingChildren(nodeToDelete); 
        
            OnValidate();
        }

        private DialogueNode MakeNode(DialogueNode parent)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.name = Guid.NewGuid().ToString();
            
            if (parent != null)
            {
                parent.AddChild(newNode.name);
                newNode.SetPlayerSpeaking(!parent.IsPlayerSpeaking());
                newNode.SetPosition(new Vector2(
                    parent.GetRect().xMax + parent.GetRect().width / 10,
                    parent.GetRect().y));
            }
            return newNode;
        }

        private void AddNode(DialogueNode newNode)
        {
            nodes.Add(newNode);
            OnValidate();
        }

        private void CleanDanglingChildren(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.RemoveChild(nodeToDelete.name);
            }
        }

        

#endif
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if(nodes.Count == 0)
            {
                DialogueNode newNode = MakeNode(null);
                AddNode(newNode);
            }


            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (DialogueNode node in GetAllNodes())
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        {
            
        }
    }
}


