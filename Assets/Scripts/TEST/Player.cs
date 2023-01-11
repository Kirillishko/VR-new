using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _accelerationSpeed;

    private CharacterController _characterController;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (TryGetDirection(out var direction))
            Move(direction);
    }

    private bool TryGetDirection(out Vector3 direction)
    {
        var value = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            value.y += 1;
        if (Input.GetKey(KeyCode.S))
            value.y -= 1;
        if (Input.GetKey(KeyCode.A))
            value.x -= 1;
        if (Input.GetKey(KeyCode.D))
            value.x += 1;

        direction = transform.forward * value.y + transform.right * value.x;
        return direction != Vector3.zero;
    }

    private void Move(Vector3 direction)
    {
        float speed = _speed;

        if (Input.GetKey(KeyCode.LeftShift))
            speed = _accelerationSpeed;

        var offset = direction * (speed * Time.deltaTime);
        _characterController.Move(offset);
    }
}