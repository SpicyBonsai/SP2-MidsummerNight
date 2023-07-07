using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IUIInterface : MonoBehaviour
{
    private void Update()
    {
        if (MouseHover.HoveredObj == this.gameObject)
            print("hovering " + MouseHover.HoveredObj);
    }
}
