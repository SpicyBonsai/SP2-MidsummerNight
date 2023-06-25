using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInitiator : MonoBehaviour
{
    [SerializeField] GameObject instructionsOverlay;
    [SerializeField] GameObject DialogueUI;
    [SerializeField] CheckDistanceDialogue distanceChecker;
    bool inDialogue = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(inDialogue == false)
        {
            instructionsOverlay.SetActive(distanceChecker.playerInRange);
        }

        if(Input.GetKeyDown(KeyCode.F) && distanceChecker.playerInRange)
        {
            DialogueUI.SetActive(true);
            instructionsOverlay.SetActive(false);
            inDialogue = true;
        }
    }
}
