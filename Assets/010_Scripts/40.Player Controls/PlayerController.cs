using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //public Variables
    public float GroundDistance = 0.4f;
    public bool PointAndClick = false; //we will put input type into settings later, now it's a variable in inspector
    public LayerMask GroundMask;
    public Transform GroundCheck;
    public GameObject Pointer;
    //public static GroundSnap PointerGroundSet;

    //private Variables
    float _gravity = -9.81f;
    Vector3 _velocity, _movementDirection;
    bool _isGrounded;
    string _guiText;
    RaycastHit _raycastHit;
    Ray _ray;
    CharacterController _characterController;
    NavMeshAgent _navMeshAgent;

    public bool IsTryingToInteract = false;
    public IInteractable ObjToInteractWith;



    void Start()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        //print(_navMeshAgent);
    }


    void Update()
    {
        #region Gravity
        /*_isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);
        if (_isGrounded && _velocity.y < 0)
            _velocity.y = -2f;*/
        #endregion

        #region WASD/Joystick
        // float _x = Input.GetAxis("Horizontal");
        // float _z = Input.GetAxis("Vertical");

        // Vector3 _move = transform.right * _x + transform.forward * _z;
        if (!PointAndClick)
        {
            _navMeshAgent.ResetPath();
            // _navMeshAgent.Move(Speed * Time.deltaTime * _move);
            _velocity.y += _gravity * Time.deltaTime;
            //_characterController.Move(_velocity * Time.deltaTime);
            _navMeshAgent.Move(_velocity * Time.deltaTime);
        }
        #endregion

        #region Point&Click
        else
        {
            if (InputManager.GetInstance().InteractButtonPressed)
            {
                _ray = Camera.main.ScreenPointToRay(InputManager.GetInstance().MousePosition);
                if (Physics.Raycast(_ray, out _raycastHit, Mathf.Infinity, GroundMask))
                {
                    GameObject hitObject = _raycastHit.collider.gameObject;
                    Vector3 _pointerPos = _raycastHit.point; 
                    Pointer.transform.position = _pointerPos;
                    _navMeshAgent.SetDestination(Pointer.transform.position);

                    if(hitObject.layer == LayerMask.NameToLayer("Interactable"))
                    {

                        IInteractable interact = hitObject.GetComponent(typeof(IInteractable)) as IInteractable;

                        if(interact.InRange)
                        {
                            _navMeshAgent.ResetPath();
                            Pointer.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                            interact.Interact();
                        }
                        else
                        {
                            ObjToInteractWith = interact;
                            if(Physics.Raycast(_raycastHit.point, Vector3.down, out _raycastHit, Mathf.Infinity, LayerMask.NameToLayer("Ground")))
                            {
                                Pointer.transform.position = _raycastHit.point;
                                _navMeshAgent.SetDestination(Pointer.transform.position);
                            }

                            //here we could create a custom effect around the interactable object that gets activated when the player clicks him
                        }
                    }
                    else
                    {
                        IsTryingToInteract = false;
                        ObjToInteractWith = null;
                    }
                }

            }

        }
        #endregion

        if(ObjToInteractWith != null)
        {
            if(ObjToInteractWith.InRange)
            {
                ObjToInteractWith.Interact();
                _navMeshAgent.ResetPath();
                ObjToInteractWith = null;
            }    
        }

        #region Temporary for GUI Debug
        _guiText = PointAndClick ? "Point&Click" : "WASD/Gamepad";
        #endregion
    }

    public void StopWalking()
    {
        _navMeshAgent.ResetPath();
    }




    // private void OnGUI()
    // {
    //     GUI.Label(new Rect(10, 10, 50, 20), _guiText);
    // }
}
