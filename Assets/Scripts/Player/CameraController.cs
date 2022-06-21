using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Undefinable.Input;

/// <summary>
/// A script that handles the camera and it's rotation.
/// ---------------------------------------------------------------------------------
/// References:
/// [1] On application focus:
///     https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnApplicationFocus.html
/// [2] Lock the mousecursor
///     https://docs.unity3d.com/ScriptReference/Cursor-lockState.html
/// ---------------------------------------------------------------------------------
/// (b ^-^)b <(Nico was here) ~
/// 
/// </summary>

public class CameraController : MonoBehaviour, ITeleportListener
{
    [SerializeField] ControlScheme _cs;
    [SerializeField] Transform _body;
    [SerializeField] float _offset = -90;
    Vector2 _cameraInput;
    Vector2 _invertedValue;

    //Fetch and set some values.
    private void Start()
    {
        _invertedValue.x = _cs.invertedX ? -1 : 1 ;
        _invertedValue.y = _cs.invertedY ? -1 : 1 ;
    }

    //Capture and lock the mouse
    private void OnApplicationFocus(bool focus) {
        if (focus) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Limit mouse and apply to object
    void Update() {   
        //Correct all the input
        _cameraInput.y -= Input.GetAxis("Mouse Y") * _cs.sensivity * _invertedValue.y;
        _cameraInput.x += Input.GetAxis("Mouse X") * _cs.sensivity * _invertedValue.x;
        _cameraInput.y = Mathf.Clamp(_cameraInput.y, -90f, 90);
       
        //Rotate camera and body for movement
        transform.rotation = Quaternion.Euler(_cameraInput.y, _cameraInput.x, 0);
        _body.rotation = Quaternion.Euler(0, _cameraInput.x +  _offset, 0);
    }

    public void OnTeleport(Portal sender, Portal reciepient)
    {
        _cameraInput.x = transform.rotation.eulerAngles.y;
        _cameraInput.y = transform.rotation.eulerAngles.x;
    }
}
