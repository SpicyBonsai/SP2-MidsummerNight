using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Lyr.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] 
        bool isPlayerSpeaking = false;
        [SerializeField] 
        private string text;
        [SerializeField] 
        private List<string> children = new List<string>();
        [SerializeField] 
        private Rect rect = new Rect(0, 0, 300, 100);
        
        [Range(0f, 1000f)] 
        public float textAreaHeightOffset = 0f;

        [SerializeField] 
        private string OnEnterAction;
        [SerializeField] 
        private string OnExitAction;

        [SerializeField] 
        private string OnTimeAction;

        public string GetOnEnterAction()
        {
            return OnEnterAction;
        }
        public string GetOnExitAction()
        {
            return OnExitAction;
        }        

        public string GetOnTimeAction()
        {
            return OnTimeAction;
        }


        public Rect GetRect()
        {
            return rect;
        }

        public void SetRectHeight(float height)
        {
            rect.height = height;
        }

        public void SetPosition(Vector2 newPosition)
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Move node around");
#endif
            rect.position = newPosition;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        public string GetText()
        {
            return text;
        }

        public void SetText(string newText)
        {
            if(newText != text)
            {
#if UNITY_EDITOR
                Undo.RecordObject(this, "Update Dialogue Text");
#endif
                text = newText;
#if UNITY_EDITOR
                EditorUtility.SetDirty(this);
#endif
            }
        }

        public List<string> GetChildren()
        {
            
            return children;
        }
        
        public bool IsPlayerSpeaking()
        {
            return isPlayerSpeaking;
        }

        public void SetPlayerSpeaking(bool value)
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Change dialogue speaker");
#endif
            isPlayerSpeaking = value;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
        public void AddChild(string childID)
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Add Dialogue Link");
#endif
            children.Add(childID);
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        public void RemoveChild(string childID)
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Remove Dialogue Link");
#endif
            children.Remove(childID);
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }


    }
}

