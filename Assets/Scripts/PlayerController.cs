using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public Variables
    public float Speed,
                 GroundDistance = 0.4f;
    public LayerMask GroundMask;
    public Transform GroundCheck;
    public GameObject Pointer;

    //private Variables
    float _gravity = -9.81f;
    Vector3 _velocity;
    bool _isGrounded;
    CharacterController _characterController;


    void Start()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
    }


    void Update()
    {
        _isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);
        if (_isGrounded && _velocity.y < 0)
            _velocity.y = -2f;

        #region WASD/Joystick
        float _x = Input.GetAxis("Horizontal");
        float _z = Input.GetAxis("Vertical");

        Vector3 _move = transform.right * _x + transform.forward * _z;
        #endregion

        _characterController.Move(_move * Speed * Time.deltaTime);

        _velocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }
}
