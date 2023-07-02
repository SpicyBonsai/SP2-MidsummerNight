using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate_Tudor : MonoBehaviour
{
    public float Sensitivity;
    public Transform playerTransform;
    float _targetRotation;
    [SerializeField] float desiredTime;
    [SerializeField] float returnToFacingDelay;
    float lerpDuration;
    Coroutine returnToFacing = null;
    float elapsedTime = 0;

    private void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            //_targetRotation = transform.forward.y;
            if(returnToFacing != null)
            {
                StopCoroutine(returnToFacing);
                returnToFacing = null;
            }

            // transform.rotation = Quaternion.Euler(0.0f, playerTransform.rotation.y * -0.1f, 0.0f);
            // _targetRotation = transform.rotation.y;


            //Cursor.lockState = CursorLockMode.Locked;
            _targetRotation += Input.GetAxis("Mouse X") * Sensitivity;
            _targetRotation += Input.GetAxis("RightJoystick_X") * Sensitivity;

            transform.localRotation = Quaternion.Euler(0f, _targetRotation, 0f);
            //transform.localEulerAngles = new Vector3(0, _targetRotation, 0);
        }
        else if(Input.GetMouseButton(0)){
            //_targetRotation = transform.localRotation.y;
            if(Vector3.Dot(transform.forward, playerTransform.forward) < 0.99f && returnToFacing == null)
            {
                
                returnToFacing = StartCoroutine(ReturnToFacing());
            }


            //Cursor.lockState = CursorLockMode.None; 
        }

        if(returnToFacing != null)
        {
            _targetRotation = transform.localRotation.eulerAngles.y;
            // Debug.Log("Rotating");
        }
    }

    IEnumerator ReturnToFacing()
    {
        elapsedTime = 0;
        lerpDuration = desiredTime;
        Vector3 initialDirection = transform.forward;
        yield return new WaitForSeconds(returnToFacingDelay);

        while(lerpDuration < 1 && !Input.GetKey(KeyCode.Q))
        {
            elapsedTime += Time.deltaTime;
            lerpDuration = elapsedTime/desiredTime;
            transform.forward = Vector3.Lerp(initialDirection, playerTransform.forward, lerpDuration);
            
            yield return new WaitForEndOfFrame();
        }

        returnToFacing = null;
    }

    private float LerpFloat(float from, float to, float inTime)
    {
        return (from * (1f - inTime)) + (to * inTime);
    }
}
