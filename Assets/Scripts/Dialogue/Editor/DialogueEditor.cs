using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;

namespace Lyr.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue;
        [NonSerialized] GUIStyle nodeStyle;
        [NonSerialized] DialogueNode draggingNode = null;
        [NonSerialized] public Vector2 draggingOffset;
        [NonSerialized] DialogueNode creatingNode = null;
        [NonSerialized] DialogueNode nodeToRemove = null;
        [NonSerialized] DialogueNode linkingParentNode = null;
        Vector2 scrollPosition;
        [NonSerialized] bool draggingCanvas = false;
        [NonSerialized] Vector2 draggingCanvasOffset;
        const float canvasSize = 4000f;
        const float backgroundSize = 50f;



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
             
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                Rect canvas = GUILayoutUtility.GetRect(canvasSize, canvasSize);
                Texture2D backgroundTex = Resources.Load("background") as Texture2D;
                Rect texCoords = new Rect(0, 0, canvasSize/backgroundSize, canvasSize/backgroundSize);
                GUI.DrawTextureWithTexCoords(canvas, backgroundTex, texCoords);
                
                foreach(DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }
                foreach(DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawNodes(node);
                }

                EditorGUILayout.EndScrollView();

                if(creatingNode != null)
                {
                    Undo.RecordObject(selectedDialogue, "Added new node");
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }

                if(nodeToRemove != null)
                {
                    Undo.RecordObject(selectedDialogue, "Removed Node");
                    selectedDialogue.RemoveNode(nodeToRemove);
                    nodeToRemove = null;
                }
            } 
        }

        private void ProcessEvents()
        {
            if(Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
                if(draggingNode != null)
                {
                    draggingOffset = new Vector2 (draggingNode.rect.x, draggingNode.rect.y) - Event.current.mousePosition;
                }
                else
                {
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                Undo.RecordObject(selectedDialogue, "Move node around");
                draggingNode.rect.position = Event.current.mousePosition + draggingOffset;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && draggingCanvas)
            {
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseUp && draggingCanvas)
            {
                draggingCanvas = false;
            }

        }

        private void DrawNodes(DialogueNode node)
        {   
            foreach(DialogueNode otherNode in selectedDialogue.GetAllNodes())
            {
                if(otherNode.uniqueID != node.uniqueID && 
                Mathf.Abs(otherNode.rect.x - node.rect.x) < 20 && 
                Mathf.Abs(otherNode.rect.y - node.rect.y) < 20)
                {
                    node.rect.x += 20;
                    node.rect.y += 20;
                }
            }

            GUILayout.BeginArea(new Rect(node.rect.x, node.rect.y, node.rect.width, node.rect.height), nodeStyle);

            //start tracking changes made to the following variables
            EditorGUI.BeginChangeCheck();

            string newText = EditorGUILayout.TextField(node.text);


            //if anything changed then record the original item values, assign new ones and mark the SO as dirty so unity would save the file 
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(selectedDialogue, "Update Dialogue Text");
                node.text = newText;
            }


            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Remove"))
            {
                nodeToRemove = node;
            }

            DrawLinkButtons(node);

            if (GUILayout.Button("Add"))
            {
                creatingNode = node;
            }
            GUILayout.EndHorizontal();
            // foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            // {
            //     //EditorGUILayout.LabelField(childNode.text);
            // }

            GUILayout.EndArea();
        }

        private void DrawLinkButtons(DialogueNode node)
        {
            if (linkingParentNode == null)
            {
                if (GUILayout.Button("Link"))
                {
                    linkingParentNode = node;
                }
            }
            else if (linkingParentNode == node)
            {
                if (GUILayout.Button("Cancel"))
                {
                    linkingParentNode = null;
                }
            }
            else if (linkingParentNode.children.Contains(node.uniqueID))
            {
                if (GUILayout.Button("Unlink"))
                {
                    Undo.RecordObject(selectedDialogue, "Removed child relationship to node");
                    linkingParentNode.children.Remove(node.uniqueID);
                    linkingParentNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("Child"))
                {
                    Undo.RecordObject(selectedDialogue, "Added child relationship to node");
                    linkingParentNode.children.Add(node.uniqueID);
                    linkingParentNode = null;
                }
            }
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
 