using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInitiator : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject instructionsOverlay;
    [SerializeField] GameObject UIDialogue;
    [SerializeField] Lyr.Dialogue.Dialogue currentDialogue;
    bool inDialogue = false;
    public bool InRange { get; private set; }
    private PlayerController _playerController;
    private Transform _playerPos;
    private Lyr.Dialogue.PlayerConversant _playerConversant;

    [Header("Parameters to play with")]

    [Tooltip("This changes the distance from which the dialogue can start")]
    [SerializeField] private float _distanceToInteract = 2f;
    


    // Start is called before the first frame update
    void Start()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        _playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<Lyr.Dialogue.PlayerConversant>();
    }

    private bool PlayerInRange() => (_playerPos.position - gameObject.transform.position).magnitude <= _distanceToInteract;

    // Update is called once per frame
    void Update()
    {
        if (!UIDialogue.activeSelf || !PlayerInRange())
        {
            inDialogue = false;
        }

        if(inDialogue) return;

        InRange = PlayerInRange();
        
        if(InRange)
        {
            instructionsOverlay.SetActive(PlayerInRange());
        }
        

        // if (InRange && _playerController.IsTryingToInteract)
        // {
        //     Interact();
        // }
    }

    public void Interact()
    {
        _playerConversant.StartDialogue(currentDialogue, this);
        UIDialogue.SetActive(true);
        UIDialogue.GetComponentInChildren<Lyr.UI.DialogueUI>().InitiateDialogue();
        instructionsOverlay.SetActive(false);
        inDialogue = true;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, _distanceToInteract);
    }
}
