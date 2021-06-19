using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;

    private CharacterController _controller;
    private Vector2 _direction;

    private void Start()
    {
        transform.parent = null;
        _controller = GetComponent<CharacterController>();
        var vCam = GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<CinemachineVirtualCamera>();
        vCam.Follow = transform;
        vCam.LookAt = transform;
    }
    public void Update()
    {
        _controller.Move(new Vector3(_direction.x, 0, _direction.y) * _speed * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        _direction = ctx.ReadValue<Vector2>();
    }
}
 