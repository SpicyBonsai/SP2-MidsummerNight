using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyr.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject {
        [SerializeField]
        DialogueNode[] nodes;
    }
}


