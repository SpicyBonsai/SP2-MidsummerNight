using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;

namespace Lyr.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue;
        GUIStyle nodeStyle;
        DialogueNode draggingNode = null;
        public Vector2 draggingOffset;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
            
        }

        [OnOpenAssetAttribute(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            //find an object from this ID and then try to cast it into this type of object; if it doesn't work it returns null
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue; 
            if(dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }
            return false;
        }

        private void OnEnable() 
        {
            Selection.selectionChanged += OnSelectionChanged;

            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
            
        }

        private void OnSelectionChanged() 
        {
            Dialogue newDialogue = Selection.activeObject as Dialogue;
            if(newDialogue != null)
            {
                selectedDialogue = newDialogue;
            }
            Repaint();
        }

        private void OnGUI() 
        {
            if(selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No dialogue selected");
            }
            else
            {
                ProcessEvents();
                foreach(DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }
                foreach(DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawNodes(node);
                }
            } 
        }


        private void ProcessEvents()
        {
            if(Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition);
                if(draggingNode != null)
                {
                    draggingOffset = new Vector2 (draggingNode.rect.x, draggingNode.rect.y) - Event.current.mousePosition;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                Undo.RecordObject(selectedDialogue, "Move node around");
                draggingNode.rect.position = Event.current.mousePosition + draggingOffset;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
            }

        }




        private void DrawNodes(DialogueNode node)
        {
            GUILayout.BeginArea(new Rect(node.rect.x, node.rect.y, node.rect.width, node.rect.height), nodeStyle);

            //start tracking changes made to the following variables
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Node:", EditorStyles.whiteLabel);
            string newID = EditorGUILayout.TextField(node.uniqueID);
            string newText = EditorGUILayout.TextField(node.text);


            //if anything changed then record the original item values, assign new ones and mark the SO as dirty so unity would save the file 
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(selectedDialogue, "Update Dialogue Text");
                node.uniqueID = newID;
                node.text = newText;
            }


            EditorGUILayout.LabelField("Children:");
            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {
                EditorGUILayout.LabelField(childNode.text);
            }

            GUILayout.EndArea();
        }

        private void DrawConnections(DialogueNode node)
        {
            foreach(DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {
                Vector3 startPosition = new Vector2(node.rect.xMax, node.rect.center.y);
                Vector3 endPosition = new Vector2(childNode.rect.xMin, childNode.rect.center.y);
                Vector3 offsetDraw = endPosition - startPosition;
                offsetDraw.y = 0;
                offsetDraw *= 0.8f;
                
                Handles.DrawBezier(startPosition, endPosition, startPosition + offsetDraw, endPosition - offsetDraw, Color.grey, null, 6f);

            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;
            foreach(DialogueNode node in selectedDialogue.GetAllNodes())
            {
                if(node.rect.Contains(point))
                {
                    foundNode = node;

                }
            }

            if(foundNode != null)
            {
                return foundNode;
            }

            return null;
        }
    }
}
 