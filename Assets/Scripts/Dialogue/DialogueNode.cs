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
            Undo.RecordObject(this, "Move node around");
            rect.position = newPosition;
            EditorUtility.SetDirty(this);
        }

        public string GetText()
        {
            return text;
        }

        public void SetText(string newText)
        {
            if(newText != text)
            {
                Undo.RecordObject(this, "Update Dialogue Text");
                text = newText;
                EditorUtility.SetDirty(this);
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
            Undo.RecordObject(this, "Change dialogue speaker");
            isPlayerSpeaking = value;

            EditorUtility.SetDirty(this);
        }
        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Add Dialogue Link");
            children.Add(childID);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childID)
        {
            Undo.RecordObject(this, "Remove Dialogue Link");
            children.Remove(childID);
            EditorUtility.SetDirty(this);
        }


    }
}

