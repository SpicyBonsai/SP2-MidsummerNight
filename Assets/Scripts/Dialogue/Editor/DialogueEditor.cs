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
        [NonSerialized] GUIStyle playerNodeStyle;
        [NonSerialized] GUIStyle textAreaStyle;
        [NonSerialized] DialogueNode draggingNode = null;
        [NonSerialized] public Vector2 draggingOffset;
        [NonSerialized] DialogueNode creatingNode = null;
        [NonSerialized] DialogueNode nodeToRemove = null;
        [NonSerialized] DialogueNode linkingParentNode = null;
        Vector2 scrollPosition;
        [NonSerialized] bool draggingCanvas = false;
        [NonSerialized] Vector2 draggingCanvasOffset;
        const float canvasSize = 10000f;
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

            playerNodeStyle = new GUIStyle();
            playerNodeStyle.normal.background = EditorGUIUtility.Load("node2") as Texture2D;
            playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            playerNodeStyle.border = new RectOffset(12, 12, 12, 12);
            
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
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }

                if(nodeToRemove != null)
                {
                    Undo.RecordObject(selectedDialogue, "remove");
                    selectedDialogue.RemoveNode(nodeToRemove);
                    nodeToRemove = null;
                }
            } 
        }

        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                //Event.current.button 0 == Left Click
                //Event.current.button 1 == Right Click
                //Event.current.button 2 == Middle Click

                if (Event.current.button == 0)
                {
                    draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
                    if(draggingNode != null)
                    {
                        draggingOffset = new Vector2 (draggingNode.GetRect().x, draggingNode.GetRect().y) - Event.current.mousePosition;
                        Selection.activeObject = draggingNode;
                    }
                    else
                    {
                        Selection.activeObject = selectedDialogue;
                        draggingCanvas = true;
                        draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                    }
                } 
                //if using middle click only drag canvas
                else if (Event.current.button == 2)
                {
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                }
            }
            //move node
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                
                draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);
                GUI.changed = true;
            }
            //move canvas
            else if (Event.current.type == EventType.MouseDrag && draggingCanvas)
            {
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
                GUI.changed = true;
            }

            //end dragging 
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
            GUIStyle style = nodeStyle;
            if (node.IsPlayerSpeaking())
            {
                style = playerNodeStyle;
            }

            foreach(DialogueNode otherNode in selectedDialogue.GetAllNodes())
            {
                //if nodes overlap, offset their position a bit
                if(otherNode.name != node.name && 
                Mathf.Abs(otherNode.GetRect().x - node.GetRect().x) < 20 && 
                Mathf.Abs(otherNode.GetRect().y - node.GetRect().y) < 20)
                {
                    node.SetPosition(new Vector2(node.GetRect().x + 20, node.GetRect().y + 20f));
                }
            }

            float textAreaHeight = style.CalcHeight(new GUIContent(node.GetText()), node.GetRect().width);
            textAreaStyle = new GUIStyle(EditorStyles.textField);
            textAreaStyle.wordWrap = true;

            // GUILayout.BeginArea(new Rect(node.GetRect().x, node.GetRect().y, node.GetRect().width, node.GetRect().height), nodeStyle);
            GUILayout.BeginArea(new Rect(node.GetRect().x, node.GetRect().y, node.GetRect().width, textAreaHeight + node.textAreaHeightOffset + 40f), style);
            
            //set text using the text field from the scriptable object as an input
            //if it's the same, it doesn't record an undo event 
            
            //node.SetText(EditorGUILayout.TextField(node.GetText()));
            node.SetText(EditorGUILayout.TextArea(node.GetText(), textAreaStyle, GUILayout.ExpandHeight(true)));


            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Remove"))
            {
                if (Event.current.button == 0)
                {
                    nodeToRemove = node;
                }
            }

            DrawLinkButtons(node);

            if (GUILayout.Button("Add"))
            {
                if (Event.current.button == 0)
                {
                    creatingNode = node;
                }
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
                    if (Event.current.button == 0)
                    {
                        linkingParentNode = node;
                    }
                }
            }
            else if (linkingParentNode == node)
            {
                if (GUILayout.Button("Cancel"))
                {
                    if (Event.current.button == 0)
                    {
                        linkingParentNode = null;
                    }
                }
            }
            else if (linkingParentNode.GetChildren().Contains(node.name))
            {
                if (GUILayout.Button("Unlink"))
                {
                    if (Event.current.button == 0)
                    {
                        linkingParentNode.RemoveChild(node.name);
                        linkingParentNode = null;
                    }
                }
            }
            else
            {
                if (GUILayout.Button("Child"))
                {
                    if (Event.current.button == 0)
                    {
                    linkingParentNode.AddChild(node.name);
                    linkingParentNode = null;
                    }
                }
            }
        }

        private void DrawConnections(DialogueNode node)
        {
            foreach(DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {
                Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
                Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
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
                if(node.GetRect().Contains(point))
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
 