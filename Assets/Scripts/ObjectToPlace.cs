using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToPlace : MonoBehaviour
{
    public bool IsGrabbing = false;
    
    [SerializeField] private Material _material;
    [SerializeField] private Axis _axis;

    private Vector3 _previousPosition;
    private Quaternion _previousRotation;

    public BoxCollider Collider { get; private set; }
    public Material Material => _material;

    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
    }

    public void ResetPosition()
    {
        transform.position = _previousPosition;
        transform.rotation = _previousRotation;
    }

    public void SetPreviousPosition(Vector3 previousPosition)
    {
        _previousPosition = previousPosition;
        _previousRotation = transform.rotation;
    }

    public void SetCollider(bool isActive)
    {
        Collider.enabled = isActive;
    }

    public void Rotate(float speed)
    {
        speed *= Time.deltaTime;

        switch (_axis)
        {
            case Axis.X:
                {
                    transform.Rotate(speed, 0f, 0f);
                    break;
                }
            case Axis.Y:
                {
                    transform.Rotate(0f, speed, 0f);
                    break;
                }
            case Axis.Z:
                {
                    transform.Rotate(0f, 0f, speed);
                    break;
                }
        }
    }
}

public enum Axis
{
    X,
    Y,
    Z
}
