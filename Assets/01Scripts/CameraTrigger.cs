using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

#if UNITY_EDITOR
using UnityEditor;

public class CameraTrigger : MonoBehaviour
{
    public CustomInspectorObjects customInspectorObjects;

    #region COMPONENTS
    private Collider _collider;
    #endregion

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            // pan the camera
            // Move the camera when the player reaches the point
            // CameraManagement.Instance.PanCameraOnContanct(customInspectorObjects.panDistance,
            //     customInspectorObjects.panTime, customInspectorObjects.panDirection, false);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 exitDirection = (collision.transform.position - _collider.bounds.center).normalized;

            if (customInspectorObjects.swapCameras && customInspectorObjects.cameraOnLeft != null && customInspectorObjects.cameraOnRight != null)
            {
                // swap cameras
                CameraManagement.Instance.SwapCamera(customInspectorObjects.cameraOnLeft, customInspectorObjects.cameraOnRight, exitDirection);
            }

            if (customInspectorObjects.panCamerOnContact)
            {
                // pan the camera
                // CameraManagement.Instance.PanCameraOnContanct(customInspectorObjects.panDistance,
                //     customInspectorObjects.panTime, customInspectorObjects.panDirection, true);
            }
        }
    }
}

[System.Serializable]
public class CustomInspectorObjects 
{
    public bool swapCameras;
    public bool panCamerOnContact;

    [HideInInspector] public CinemachineVirtualCamera cameraOnLeft;
    [HideInInspector] public CinemachineVirtualCamera cameraOnRight;

    [HideInInspector] public PanDirectionEnum panDirection;
    [HideInInspector] public float panDistance = 3f;
    [HideInInspector] public float panTime = 0.35f;
}

public enum PanDirectionEnum 
{
    // Camera movement direction
    Up,
    Down,
    Left,
    Right
}

[CustomEditor(typeof(CameraTrigger))]

public class MyScriptEditor : Editor
{
    CameraTrigger CameraTrigger;

    private void OnEnable()
    {
        CameraTrigger = (CameraTrigger)target;
    }

    public override void OnInspectorGUI() 
    {
        DrawDefaultInspector();

        if (CameraTrigger.customInspectorObjects.swapCameras) 
        {
            CameraTrigger.customInspectorObjects.cameraOnLeft = EditorGUILayout.ObjectField("Camera On Left", CameraTrigger.customInspectorObjects.cameraOnLeft,
                typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;

            CameraTrigger.customInspectorObjects.cameraOnRight = EditorGUILayout.ObjectField("Camera On Right", CameraTrigger.customInspectorObjects.cameraOnRight,
                typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
        }

        if (CameraTrigger.customInspectorObjects.panCamerOnContact)
        {
            CameraTrigger.customInspectorObjects.panDirection = (PanDirectionEnum)EditorGUILayout.EnumPopup("Camera Pan Direction",
                CameraTrigger.customInspectorObjects.panDirection);

            CameraTrigger.customInspectorObjects.panDistance = EditorGUILayout.FloatField("Pan Distance", CameraTrigger.customInspectorObjects.panDistance);
            CameraTrigger.customInspectorObjects.panTime = EditorGUILayout.FloatField("Pan Time", CameraTrigger.customInspectorObjects.panTime);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(CameraTrigger);
        }
    }
}
#endif