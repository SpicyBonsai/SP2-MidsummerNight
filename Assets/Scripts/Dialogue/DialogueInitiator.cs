using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInitiator : MonoBehaviour, IInteractable
{
    [Header("General Dialogue Parameters:")]
    [SerializeField] Lyr.Dialogue.Dialogue currentDialogue;
    [SerializeField] Sprite characterImage;
    [SerializeField] Vector2 offsetValue;
    [SerializeField] string characterName;
    bool inDialogue = false;
    public bool InRange { get; private set; }
    private PlayerController _playerController;
    private Transform _playerPos;
    private Lyr.Dialogue.PlayerConversant _playerConversant;

    [Header("Interactable Dialogue Parameters:")]

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

        if (!DialogueManager.GetInstance().GetDialogueUI().activeSelf || !PlayerInRange())
        {
            inDialogue = false;
        }

        if(inDialogue) return;

        InRange = PlayerInRange();
        
        if(InRange)
        {
            DialogueManager.GetInstance().SetInteractableOverlay(PlayerInRange());
        }
        

        // if (InRange && _playerController.IsTryingToInteract)
        // {
        //     Interact();
        // }
    }

    public void Interact()
    {
        _playerConversant.StartDialogue(currentDialogue, this);
        DialogueManager.GetInstance().OpenDialogue();
        DialogueManager.GetInstance().SetImage(characterImage);
        DialogueManager.GetInstance().OffsetImage(offsetValue);
        DialogueManager.GetInstance().SetName(characterName);

        DialogueManager.GetInstance().GetDialogueUI().GetComponentInChildren<Lyr.UI.DialogueUI>().InitiateDialogue();


        DialogueManager.GetInstance().SetInteractableOverlay(false);
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
