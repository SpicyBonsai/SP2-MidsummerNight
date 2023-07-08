using System.Collections;
using UnityEngine;

public class CameraRotate_Tudor : MonoBehaviour
{
    [SerializeField] private float desiredTime;
    [SerializeField] private float returnToFacingDelay;
    public float sensitivity;
    public Transform playerTransform;
    private float _targetRotation;
    private float _lerpDuration;
    private Coroutine _returnToFacing;
    private float _elapsedTime;

    private void Start() 
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    private void Update()
    {
        if (InputManager.GetInstance().RightClick)
        {
            if(_returnToFacing != null)
            {
                StopCoroutine(_returnToFacing);
                _returnToFacing = null;
            }
            
            _targetRotation += InputManager.GetInstance().MouseMovement.x * sensitivity;
            transform.localRotation = Quaternion.Euler(0f, _targetRotation, 0f);
        }
        else if(InputManager.GetInstance().LeftClick)
        {
            if(Vector3.Dot(transform.forward, playerTransform.forward) < 0.99f && _returnToFacing == null)
            {
                _returnToFacing = StartCoroutine(ReturnToFacing());
            }
        }

        if(_returnToFacing != null)
        {
            _targetRotation = transform.localRotation.eulerAngles.y;
        }
    }

    private IEnumerator ReturnToFacing()
    {
        _elapsedTime = 0;
        _lerpDuration = desiredTime;
        var initialDirection = transform.forward;
        yield return new WaitForSeconds(returnToFacingDelay);

        while(_lerpDuration < 1)
        {
            _elapsedTime += Time.deltaTime;
            _lerpDuration = _elapsedTime/desiredTime;
            transform.forward = Vector3.Lerp(initialDirection, playerTransform.forward, _lerpDuration);
            
            yield return new WaitForEndOfFrame();
        }

        _returnToFacing = null;
    }
}
