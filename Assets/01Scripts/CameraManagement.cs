using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManagement : MonoSingleton<CameraManagement>
{
    protected CameraManagement() { } // �ܺο��� �ν��Ͻ� ������ �����ϰ� �̱��� ������ �����ϱ� ���� ���

    [SerializeField] private CinemachineVirtualCamera[] _allVirtualCameras;

    [Header("Controls for lerping the Y Damping during player jump/fall")]
    [SerializeField] private float _fallPanAmount; // ���� �� ī�޶��� ���� �̵��� �����ϴ� ��
    [SerializeField] private float _fallYPanTime; // ���� ���� �ð�, ���� Y ������ �� ���� ������� ����
    public float fallSpeedYDampingChangeThreshold; // ���� �ӵ��� ���� �Ӱ谪�� ������ Y ������ �����ϴ� �� ���Ǵ� �Ӱ谪
                                                    // ���� �ӵ��� ���� Y ������ �������� ����
    [Space(5)]
    [SerializeField] private float _normYPanAmount; // �Ϲ�����(���� ���� �ƴ� ��) Y ���� ��
                                                    // ���� ���� ���� �ƴ� �� ī�޶� ������ ����

    public bool IsLerpingYDamping { get; private set; } // ����(Damping) ���� �����ϴ� ���� ������ ����
    public bool LerpedFromPlayerFalling { get; set; } // �÷��̾��� ���� ���ۿ��� ���� ���� �����ߴ��� ����
                                                      // ���� ���� ���� �÷��̾�� ������ �־����� ��Ÿ��

    private CinemachineVirtualCamera _currentCamera; // ���� Ȱ��ȭ�� CinemachineVirtualCamera ������Ʈ�� ��Ÿ���� ����
    private CinemachineFramingTransposer _framingTransposer; // ���� ī�޶��� ������ ������ ��Ÿ���� ����
                                                             // ������ ������ ī�޶� �þ߿� ������ �����ϱ� ���� ���

    private Vector3 _startingTrackedObjectOffset;

    private void Awake()
    {
#if UNITY_EDITOR
        UnityEditorInitialize();
#endif
        for (int i = 0; i < _allVirtualCameras.Length; i++)
        {
            if (_allVirtualCameras[i].enabled)
            {
                // Set the current active camera
                _currentCamera = _allVirtualCameras[i];

                // Set the framing transposer
                _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }

        // set the YDamping amount so it' based on the inspector value
        _normYPanAmount = _framingTransposer.m_YDamping;

        // set the starting position of the tracked object offset
        _startingTrackedObjectOffset = _framingTransposer.m_TrackedObjectOffset;
    }

#if UNITY_EDITOR
    private void UnityEditorInitialize()
    {
        for (int i = 0; i < _allVirtualCameras.Length; i++)
        {
            if (_allVirtualCameras[i].enabled)
            {
                // Set the current active camera
                _currentCamera = _allVirtualCameras[i];

                // Set the framing transposer
                _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }

        // set the YDamping amount so it's based on the inspector value
        _normYPanAmount = _framingTransposer.m_YDamping;

        // set the starting position of the tracked object offset
        _startingTrackedObjectOffset = _framingTransposer.m_TrackedObjectOffset;
    }
#endif

    #region LERP THE Y DAMPING
    public void LerpYDamping(bool isPlayerFalling)
    {
        StartCoroutine(nameof(LerpYAction), isPlayerFalling);
    }

    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true; // Y ���� ���� ���� ��

        // grab the starting damping amount 
        float startDampAmount = _framingTransposer.m_YDamping;
        float endDampAmount = 0f;

        // grab the end damping amount 
        if (isPlayerFalling)
        {
            endDampAmount = _fallPanAmount;
            LerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = _normYPanAmount;
        }

        // lerp the pan amount
        float elapsedTime = 0f;
        while (elapsedTime < _fallYPanTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / _fallYPanTime));
            _framingTransposer.m_YDamping = lerpedPanAmount;

            yield return null;
        }
        IsLerpingYDamping = false; // ���� ��
    }
    #endregion

    #region PAN CAMERA
    public void PanCameraOnContanct(float panDistance, float panTime, PanDirectionEnum panDirection, bool panToStartingPos)
    {
        StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
    }

    private IEnumerator PanCamera(float panDistance, float panTime, PanDirectionEnum panDirection, bool panToStartingPos)
    // �̵��� �Ÿ�, �̵��ϴµ� �ɸ��� �ð�, �̵� ����, ���� ��ġ�� ���ư���?
    {
        Vector3 endPos = Vector3.zero;
        Vector3 startingPos = Vector3.zero;

        // handle pan from trigger
        if (!panToStartingPos) // ī�޶� �̵��ϴ� ���
        {
            // set the direction and distance
            switch (panDirection) // �̵� ���� ����
            {
                case PanDirectionEnum.Up:
                    endPos = Vector3.up;
                    break;
                case PanDirectionEnum.Down:
                    endPos = Vector3.down;
                    break;
                case PanDirectionEnum.Left:
                    endPos = Vector3.right;
                    break;
                case PanDirectionEnum.Right:
                    endPos = Vector3.left;
                    break;
                default:
                    break;
            }
            endPos *= panDistance; // endPos�� panDistance��ŭ Ȯ��

            startingPos = _startingTrackedObjectOffset;

            endPos += startingPos;
        }

        // handle the pan back to starting position
        else // ���� ��ġ�� ���ư��� ���
        {
            startingPos = _framingTransposer.m_TrackedObjectOffset;
            endPos = _startingTrackedObjectOffset;
        }

        // handle the actual panning of the camera
        float elapsedTime = 0f;
        while (elapsedTime < panTime)
        {
            elapsedTime += Time.deltaTime;

            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, (elapsedTime / panTime));
            _framingTransposer.m_TrackedObjectOffset = panLerp;

            yield return null;
        }
    }
    #endregion

    #region SWAP CAMERAS
    public void SwapCamera(CinemachineVirtualCamera cameraFromLeft, CinemachineVirtualCamera cameraFromRight, Vector2 triggerExitDirection) // �� �� ����(�� �Ʒ��� ����)
    {
        // if the current camera is the camera on the left and our trigger exit direction was on the right
        // ���� ī�޶� ���ʿ� �ְ� �ⱸ�� ������ �����ʿ� �ִ� ���
        if (_currentCamera == cameraFromLeft && triggerExitDirection.x > 0f)
        {
            // activate the new camera (Ű�޶��� �켱 ������ �����Ͽ� �����ϰ� �۾��� �� �ִ�)
            cameraFromRight.enabled = true;

            // deactivate the old camera
            cameraFromLeft.enabled = false;

            // set the new camera as the current camera
            _currentCamera = cameraFromRight;

            // update our composer variable
            _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        // if the current camera is the camera on the cright and our trigger hit direction was on the left
        else if (_currentCamera == cameraFromRight && triggerExitDirection.x < 0f)
        {
            // activate the new camera (Ű�޶��� �켱 ������ �����Ͽ� �����ϰ� �۾��� �� �ִ�)
            cameraFromLeft.enabled = true;

            // deactivate the old camera
            cameraFromRight.enabled = false;

            // set the new camera as the current camera
            _currentCamera = cameraFromLeft;

            // update our composer variable
            _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }
    #endregion
}