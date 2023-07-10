using UnityEngine;
using Cinemachine;

public class CameraControls : MonoBehaviour
{
    private CinemachineVirtualCamera _cam1;
    private CinemachineTransposer _camTranspose;
    private CinemachineComposer _camComposer;
    private Transform _playerPos;
    private Transform _cameraPos;
    private float _dotCameraPlayer;
    private InputManager _inputManager;

    public bool isteleporting;

    [Header("Camera settings during manual rotation:")]
    [SerializeField] private float xDampingWhenHold;
    [SerializeField] private float yDampingWhenHold;
    [SerializeField] private float yawDampingWhenHold = 0.3f;
    [SerializeField] private float horizontalDampingWhenHold;
    [SerializeField] private float verticalDampingWhenHold;

    [Header("Camera zoom settings:")] 
    [SerializeField] private float minimumZoom;
    [SerializeField] private float maximumZoom;
    [SerializeField] private float scrollSensitivity;
    private float _zoomLevel = 7f;
    private float _cameraPitchModifier = 0f;
    
    void Start()
    {
        _inputManager = InputManager.GetInstance();
        _cam1 = gameObject.GetComponent<CinemachineVirtualCamera>();
        _cameraPos = Camera.main.transform;
        _playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        _camTranspose = _cam1.GetCinemachineComponent<CinemachineTransposer>();
        _camComposer = _cam1.GetCinemachineComponent<CinemachineComposer>();
    }

    
    void Update()
    {
        //cameraPos.position = new Vector3(cameraPos.position.x, playerPos.position.y, cameraPos.position.z);
        //Debug.Log("Dot product of camera and player forwards:" + Vector3.Dot(cameraPos.forward.normalized, playerPos.forward));

        if(!isteleporting)
        {
            _dotCameraPlayer = Vector3.Dot(_cameraPos.forward.normalized, _playerPos.forward);
            _camTranspose.m_XDamping = Mathf.Clamp(2 * _dotCameraPlayer, 0, 2);
            _camTranspose.m_YDamping = Mathf.Clamp(2 * _dotCameraPlayer, 0, 2);
            _camTranspose.m_YawDamping = Mathf.Clamp(7 * _dotCameraPlayer, 0, 7);
        }

        _camComposer.m_HorizontalDamping = Mathf.Clamp(2 * _dotCameraPlayer, 0, 2);
        _camComposer.m_VerticalDamping = Mathf.Clamp(2 * _dotCameraPlayer, 0, 2);

        if (_inputManager.MouseScroll != 0)
        {
            _zoomLevel -= _inputManager.MouseScroll * scrollSensitivity * Time.deltaTime;
            _zoomLevel = Mathf.Clamp(_zoomLevel, minimumZoom, maximumZoom);
            _camTranspose.m_FollowOffset = new Vector3(0, _zoomLevel + _cameraPitchModifier, _zoomLevel * -1);
        }
        
        if(_inputManager.RightClick)
        {
            _cameraPitchModifier += _inputManager.MouseMovement.y * Time.deltaTime;
            _cameraPitchModifier = Mathf.Clamp(_cameraPitchModifier, -2f, 1f);
            _camTranspose.m_FollowOffset = new Vector3(0, _zoomLevel + _cameraPitchModifier, _zoomLevel * -1);
            
            _camTranspose.m_XDamping = xDampingWhenHold;
            _camTranspose.m_YDamping = yDampingWhenHold;
            _camTranspose.m_YawDamping = yawDampingWhenHold;

            _camComposer.m_HorizontalDamping = horizontalDampingWhenHold;
            _camComposer.m_VerticalDamping = verticalDampingWhenHold;
        }

        isteleporting = false;
    }
}
