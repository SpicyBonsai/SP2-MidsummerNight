using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInitiator : MonoBehaviour, IInteractable
{
    [Header("General Dialogue Parameters:")]
    [SerializeField] private Lyr.Dialogue.Dialogue currentDialogue;
    [SerializeField] private Sprite characterImage;
    [SerializeField] private Vector2 offsetValue;
    [SerializeField] private string characterName;
    [SerializeField] private Color characterColor;
    private Color infoColor;
    bool inDialogue = false;
    public bool InRange { get; private set; }
    private PlayerController _playerController;
    private Transform _playerPos;
    private Lyr.Dialogue.PlayerConversant _playerConversant;
    private DialogueManager _dialogueManager;


    [Header("Interactable Dialogue Parameters:")]

    [Tooltip("This changes the distance from which the dialogue can start")]
    [SerializeField] private float _distanceToInteract = 2f;
    


    // Start is called before the first frame update
    void Start()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        _playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<Lyr.Dialogue.PlayerConversant>();
        _dialogueManager = DialogueManager.GetInstance();
    }

    private void OnEnable() 
    {
        infoColor = new Color(characterColor.r, characterColor.g, characterColor.b, 0.8f);    
    }

    private bool PlayerInRange() => (_playerPos.position - gameObject.transform.position).magnitude <= _distanceToInteract;

    // Update is called once per frame
    void Update()
    {

        if (!_dialogueManager.GetDialogueUI().activeSelf || !PlayerInRange())
        {
            inDialogue = false;
        }

        if(inDialogue) return;

        InRange = PlayerInRange();
        
        if(InRange)
        {
            _dialogueManager.SetInteractableOverlay(PlayerInRange());
        }
        

        // if (InRange && _playerController.IsTryingToInteract)
        // {
        //     Interact();
        // }
    }

    public void Interact()
    {
        _playerConversant.StartDialogue(currentDialogue, this);
        _dialogueManager.OpenDialogue();
        _dialogueManager.SetImage(characterImage);
        _dialogueManager.OffsetImage(offsetValue);
        _dialogueManager.SetName(characterName);
        _dialogueManager.SetNameColor(characterColor);
        _dialogueManager.SetButtonsColor(infoColor);
        _dialogueManager.SetCurrentColor(characterColor);
        
        _dialogueManager.GetDialogueUI().GetComponentInChildren<Lyr.UI.DialogueUI>().InitiateDialogue();


        _dialogueManager.SetInteractableOverlay(false);
        inDialogue = true;
    }

    private void OnDrawGizmos() 
    {
        if(gameObject.tag == "DialogueTrigger")
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Gizmos.DrawCube(gameObject.transform.position, gameObject.transform.localScale);
        }
        else if (gameObject.tag == "Interactable")
        {
            Gizmos.DrawWireSphere(gameObject.transform.position, _distanceToInteract);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(gameObject.tag == "DialogueTrigger")
        {
            Interact();
            gameObject.SetActive(false);
        }    
    }

}
