using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Undefinable.Input;
using System;

// <summary>
// A script that handles the character movement and input
// Based on the tutorial by Dave:
// https://www.youtube.com/watch?v=f473C43s8nE
//
// May not be perfect, but should work for now.
// Does as of now not handle slopes, stairs or such things well.
// <summary>

public class PlayerController : MonoBehaviour {
    [SerializeField] ControlScheme _input;
    [SerializeField] Rigidbody _rb;
    [SerializeField] Transform _orientation;
    [SerializeField] float _jumpStrength = 7, _movespeed = 150, _airMultiplier = .1f, _castDistance = 1.02f, _drag = 5;
    bool _grounded;
    Vector2 _inputAxis;

    private void Start() {
        _rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update() {
        GroundCheck();
        FetchXYInput();
        MovePlayer();
        LimitSpeed();

        if (Input.GetKeyDown(_input.inputs[0].inputValue) && _grounded) Jump();
    }

    void FetchXYInput() {
        //Convert input to something we can use, Clamp values so that if both KB+Joystick is used it doesn't go out of range.
        _inputAxis = new Vector2(-Mathf.Clamp(-GetIntValue(_input.inputs[3].inputValue) + GetIntValue(_input.inputs[4].inputValue) +
            Input.GetAxis("Horizontal"), -1, 1), Mathf.Clamp(-GetIntValue(_input.inputs[2].inputValue) + GetIntValue(_input.inputs[1].inputValue) + Input.GetAxis("Vertical"), -1, 1));
    }

    // Something that moves the player around, could be a little more compact with inline statements but tired boy hours :)
    void MovePlayer() {
        Vector3 _direction = _orientation.forward * _inputAxis.x + _orientation.right * _inputAxis.y;

        if (_grounded) {
            _rb.drag = _drag;
            _rb.AddForce(_direction.normalized * _movespeed * 10 * Time.deltaTime, ForceMode.Force);
        }
        else {
            _rb.drag = 0;
            _rb.AddForce(_direction.normalized * _movespeed * 10 * Time.deltaTime * _airMultiplier, ForceMode.Force);
        }
    }

    //Check if on ground
    void GroundCheck() {
        _grounded = Physics.Raycast(transform.position, Vector3.down, _castDistance);
        Debug.DrawRay(transform.position, Vector3.down * _castDistance, Color.green);
    }

    // Don't go SPEED
    void LimitSpeed() {
        Vector3 velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        float speed = _movespeed * 10 * Time.deltaTime;

        if (velocity.magnitude > speed) {
            Vector3 tmp = velocity.normalized * speed;
            _rb.velocity = new Vector3(tmp.x, _rb.velocity.y, tmp.z);
        }
    }

    void Jump() {
        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);

        _rb.AddForce(transform.up * _jumpStrength, ForceMode.Impulse);
    }

    int GetIntValue(KeyCode input) { return Convert.ToInt32(Input.GetKey(input)); }

}
