using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    //public Variables
    public float Speed,
                 GroundDistance = 0.4f;
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
        float _x = Input.GetAxis("Horizontal");
        float _z = Input.GetAxis("Vertical");

        Vector3 _move = transform.right * _x + transform.forward * _z;
        if (!PointAndClick)
        {
            _navMeshAgent.ResetPath();
            _navMeshAgent.Move(Speed * Time.deltaTime * _move);
            _velocity.y += _gravity * Time.deltaTime;
            //_characterController.Move(_velocity * Time.deltaTime);
            _navMeshAgent.Move(_velocity * Time.deltaTime);
        }
        #endregion

        #region Point&Click
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(_ray, out _raycastHit, Mathf.Infinity, GroundMask))
                {
                    //Vector3 _pointerPos = new Vector3(_raycastHit.point.x, 0 + Pointer.GetComponent<SphereCollider>().radius, _raycastHit.point.z); //for pointer to be always at y = 0
                    //Pointer.GetComponent<GroundSnap>().SetPostiion(_pointerPos); //PointerGroundSet.SetPostiion(_pointerPos);

                    Vector3 _pointerPos = _raycastHit.point; 
                    Pointer.transform.position = _pointerPos;
                    _navMeshAgent.SetDestination(Pointer.transform.position);
                }
            }

            #region Point&Click without NavMesh
            /* without NavMesh
                        if (transform.position != _raycastHit.point)
                        {
                            _movementDirection = _raycastHit.point - transform.position;
                            _characterController.Move(_movementDirection.normalized * Time.deltaTime * Speed);

                        }*/
            #endregion
        }
        #endregion

        #region Temporary for GUI Debug
        _guiText = PointAndClick ? "Point&Click" : "WASD/Gamepad";
        #endregion
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 50, 20), _guiText);
    }
}
