using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _sensibility;

    private Vector3 _currentRotation = new Vector3(0, 0, 0);

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MouseRotate();
    }

    private void MouseRotate()
    {
        float x = Input.GetAxisRaw("Mouse X") * _sensibility;
        float y = Input.GetAxisRaw("Mouse Y") * _sensibility;

        _currentRotation.x -= y;
        _currentRotation.y += x;
        _currentRotation.x = Mathf.Clamp(_currentRotation.x, -90, 90);

        transform.eulerAngles = _currentRotation;
        _player.transform.eulerAngles = new Vector3(0, _currentRotation.y, 0);
    }
}