using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyr.Dialogue
{
    [System.Serializable]
    public class DialogueNode
    {
        [SerializeField] private string uniqueID;
        [SerializeField] private string text;
        [SerializeField] private string[] children;
    }
}

